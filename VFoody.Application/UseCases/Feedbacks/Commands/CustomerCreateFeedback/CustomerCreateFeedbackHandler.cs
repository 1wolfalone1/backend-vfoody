using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Feedbacks.Models;
using VFoody.Application.UseCases.Orders.Commands.CreateOrders;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Commands.CustomerCreateFeedback;

public class CustomerCreateFeedbackHandler : ICommandHandler<CustomerCreateFeedbackCommand, Result>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly IFirebaseNotificationService _firebaseNotificationService;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopRepository _shopRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<CustomerCreateOrderHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public CustomerCreateFeedbackHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, IStorageService storageService, IFirebaseNotificationService firebaseNotificationService, ICurrentPrincipalService currentPrincipalService, IOrderRepository orderRepository, IShopRepository shopRepository, INotificationRepository notificationRepository, ILogger<CustomerCreateOrderHandler> logger, IAccountRepository accountRepository, IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _firebaseNotificationService = firebaseNotificationService;
        _currentPrincipalService = currentPrincipalService;
        _orderRepository = orderRepository;
        _shopRepository = shopRepository;
        _notificationRepository = notificationRepository;
        _logger = logger;
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(CustomerCreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Validate
        this.CheckIsOrderOfCustomer(request.OrderId);
        this.CheckIsOrderReceiveFeedback(request.OrderId);
        
        // Upload Image
        var listImages = await this.GetImageLinkAfterUploadAsync(request.RequestModel.Images).ConfigureAwait(false);
        
        // Save Feedback
        var feed = await this.CreateFeedbackAsync(request, listImages).ConfigureAwait(false);
        
        // Send notification
        var order = this._orderRepository.GetById(request.OrderId);
        var shopAccount = this._shopRepository.GetAccountByShopId(order.ShopId);
        var customerAccount = this._accountRepository.GetById(order.AccountId);
        await this.SendNotificationAsync(shopAccount.Id,
            shopAccount.DeviceToken,
            NotificationMessageConstants.Feedback_Shop_Title,
            string.Format(NotificationMessageConstants.Feedback_Shop_Content, customerAccount.LastName,
                request.OrderId),
            (int)Domain.Enums.Roles.Shop,
            customerAccount.AvatarUrl).ConfigureAwait(false);
        
        return Result.Success(this._mapper.Map<CreateFeedbackResponse>(feed));
    }

    private void CheckIsOrderReceiveFeedback(int orderId)
    {
        var feed = this._feedbackRepository.Get(f => f.OrderId == orderId
                                                     && f.AccountId == this._currentPrincipalService.CurrentPrincipalId
                                                         .Value).SingleOrDefault();
        if (feed != default)
            throw new InvalidBusinessException("Bạn đã cung cấp phản hồi cho đơn hàng này rồi");
    }
    private void CheckIsOrderOfCustomer(int orderId)
    {
        var order = this._orderRepository.GetById(orderId);
        if (this._currentPrincipalService.CurrentPrincipalId != order.AccountId)
            throw new InvalidBusinessException($"Bạn không có quyền cung cấp phản hồi cho đơn hàng VFD{orderId}");
    }
    
    private async Task<List<string>> GetImageLinkAfterUploadAsync(IFormFile[] images)
    {
        var listImagesUrl = new List<string>();
        if (images != null && images.Length > 0)
        {
            foreach (var image in images)
            {
                var imagePath = await this._storageService.UploadFileAsync(image);
                listImagesUrl.Add(imagePath);
            }
        }

        return listImagesUrl;
    }

    private async Task<Feedback> CreateFeedbackAsync(CustomerCreateFeedbackCommand request, List<string> images)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            Feedback feed = new Feedback()
            {
                OrderId = request.OrderId,
                AccountId = _currentPrincipalService.CurrentPrincipalId.Value,
                Rating = (int)request.RequestModel.Rating,
                Comment = request.RequestModel.Comment,
                ImagesUrl = string.Join(",", images)
            };

            await this._feedbackRepository.AddAsync(feed).ConfigureAwait(false);
            
            //Increase total rating, star of shop
            var order = this._orderRepository.GetById(request.OrderId);
            var shop = this._shopRepository.GetById(order.ShopId);
            shop.TotalRating += 1;
            shop.TotalStar += (int)request.RequestModel.Rating;
            this._shopRepository.Update(shop);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return feed;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            throw;
        }
    }
    
    private async Task SendNotificationAsync(int accountId, string deviceToken, string title,
        string content, int role, string imageUrl)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            this._firebaseNotificationService.SendNotification(deviceToken, title, content, imageUrl);
            Notification noti = new Notification()
            {
                AccountId = accountId,
                Readed = 0,
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                RoleId = role,
            };
            await this._notificationRepository.AddAsync(noti).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
        }
    }
}