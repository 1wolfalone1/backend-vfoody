using System.Linq.Expressions;
using System.Xml.Linq;
using FluentValidation;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.CreateOrders;

public class CustomerCreateOrderHandler : ICommandHandler<CustomerCreateOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOptionRepository _optionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IPlatformPromotionRepository _platformPromotionRepository;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly IPersonPromotionRepository _personPromotionRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipal;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IOrderDetailOptionRepository _orderDetailOptionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseNotificationService _firebaseNotificationService;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly ILogger<CustomerCreateOrderHandler> _logger;
    private readonly IFirebaseFirestoreService _firebaseFirestoreService;
    
    private List<int> OptionIds { get; set; }
    private List<int> ProductIds { get; set; }
    public CustomerCreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IOptionRepository optionRepository, IQuestionRepository questionRepository, IPlatformPromotionRepository platformPromotionRepository, IShopPromotionRepository shopPromotionRepository, IPersonPromotionRepository personPromotionRepository, IShopRepository shopRepository, ICurrentPrincipalService currentPrincipal, IOrderDetailRepository orderDetailRepository, IOrderDetailOptionRepository orderDetailOptionRepository, IFirebaseNotificationService firebaseNotificationService, ILogger<CustomerCreateOrderHandler> logger, INotificationRepository notificationRepository, ICurrentAccountService currentAccountService, IFirebaseFirestoreService firebaseFirestoreService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _optionRepository = optionRepository;
        _questionRepository = questionRepository;
        _platformPromotionRepository = platformPromotionRepository;
        _shopPromotionRepository = shopPromotionRepository;
        _personPromotionRepository = personPromotionRepository;
        _shopRepository = shopRepository;
        _currentPrincipal = currentPrincipal;
        _orderDetailRepository = orderDetailRepository;
        _orderDetailOptionRepository = orderDetailOptionRepository;
        _firebaseNotificationService = firebaseNotificationService;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _currentAccountService = currentAccountService;
        _firebaseFirestoreService = firebaseFirestoreService;
    }

    public async Task<Result<Result>> Handle(CustomerCreateOrderCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            // Validate 
            this.CheckProductIsAllAvailable(request.Products);
            this.CheckToppingAvailable(request.Products);
            this.CheckTotalProductPrice(request.Products, request.OrderPrice.TotalProduct);
            this.CheckVoucherAvailable(request.Voucher, request.OrderPrice.Voucher, request.OrderPrice.TotalProduct);
            this.CheckShippingFee(request.ShopId, request.OrderPrice.ShippingFee, request.OrderPrice.TotalProduct,
                request.Ship.Distance);

            // Save
            var order = new Order()
            {
                Status = (int)OrderStatus.Pending,
                ShippingFee = (float)request.OrderPrice.ShippingFee,
                ShopId = request.ShopId,
                AccountId = _currentPrincipal.CurrentPrincipalId.Value,
                TotalPrice = (float)request.OrderPrice.TotalProduct,
                TotalPromotion = (float)request.OrderPrice.Voucher,
                FullName = request.OrderInfo.FullName,
                PhoneNumber = request.OrderInfo.PhoneNumber,
                IsRefund = 0,
                RefundStatus = (int)RefundOrderStatus.NoRefund,
                Distance = (float)request.Ship.Distance,
                DurationShipping = DateTime.Now.AddMinutes(request.Ship.Duration),
            };

            if (request.Voucher.PromotionType == PromotionTypes.PersonPromotion)
            {
                order.PersonalPromotionId = request.Voucher.Id;
            }
            else if (request.Voucher.PromotionType == PromotionTypes.PlatformPromotion)
            {
                order.PlatformPromotionId = request.Voucher.Id;
            }
            else if (request.Voucher.PromotionType == PromotionTypes.ShopPromotion)
            {
                order.ShopPromotionId = request.Voucher.Id;
            }

            // Building
            Building buil = new Building()
            {
                Name = request.OrderInfo.Building.Address,
                Latitude = (float)request.OrderInfo.Building.Latitude,
                Longitude = (float)request.OrderInfo.Building.Longitude
            };
            order.Building = buil;

            // Transaction
            Transaction transaction = new Transaction()
            {
                Amount = (float)(request.OrderPrice.TotalProduct + request.OrderPrice.ShippingFee -
                                 request.OrderPrice.Voucher),
                Status = (int)TransactionStatus.Pending,
                TransactionType = (int)TransactionTypes.Cash
            };
            order.Transaction = transaction;

            // Note
            order.Note = this.CreateNoteForOrder(request.Products);
            await this._orderRepository.AddAsync(order).ConfigureAwait(false);
            await this._unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            // Order Detail
            foreach (var pro in request.Products)
            {
                var product = this._productRepository.GetById(pro.Id);
                product.TotalOrder += 1;
                OrderDetail or = new OrderDetail()
                {
                    Quantity = pro.Quantity,
                    Price = product.Price,
                    OrderId = order.Id,
                    ProductId = pro.Id
                };

                this._productRepository.Update(product);
                await this._orderDetailRepository.AddAsync(or).ConfigureAwait(false);
                await this._unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                // Save order option
                List<OrderDetailOption> orderDetailOptions = new List<OrderDetailOption>();
                foreach (var checkBox in pro.Topping.CheckBox)
                {
                    foreach (var option in checkBox.OptionIds)
                    {
                        var optionDb = this._optionRepository.GetById(option);
                        OrderDetailOption opd = new OrderDetailOption
                        {
                            OrderDetailId = or.Id,
                            OptionId = option,
                            Price = optionDb.Price,
                        };
                        orderDetailOptions.Add(opd);
                    }
                }

                foreach (var radio in pro.Topping.Radio)
                {
                    var optiondb = this._optionRepository.GetById(radio.OptionId);
                    OrderDetailOption opd = new OrderDetailOption
                    {
                        OrderDetailId = or.Id,
                        OptionId = radio.OptionId,
                        Price = optiondb.Price,
                    };
                    orderDetailOptions.Add(opd);
                }

                await this._orderDetailOptionRepository.AddRangeAsync(orderDetailOptions).ConfigureAwait(false);
            }

            // Update Shop Total Order
            var shop = this._shopRepository.Get(sh => sh.Id == request.ShopId
                ,includes: new List<Expression<Func<Domain.Entities.Shop, object>>>()
                {
                    shp => shp.Account,
                }).SingleOrDefault();
            shop.TotalOrder += 1;
            this._shopRepository.Update(shop);

            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var customerAccount = this._currentAccountService.GetCurrentAccount();
            await this.SendNotificationAsync(customerAccount.Email, _currentPrincipal.CurrentPrincipalId.Value,
                customerAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                string.Format(NotificationMessageConstants.Order_Pending_Content, shop.Name),
                (int)Domain.Enums.Roles.Customer,
                shop.LogoUrl);
            
            await this.SendNotificationAsync(shop.Account.Email, shop.Account.Id,
                shop.Account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                string.Format(NotificationMessageConstants.Order_Shop_Pending, customerAccount.LastName),
                (int)Domain.Enums.Roles.Shop,
                customerAccount.AvatarUrl);

            return Result.Success(new
            {
                OrderID = order.Id
            });
        }
        catch (InvalidBusinessException exception)
        {
            throw exception;
        }
        catch (Exception exception)
        {
            var shop = this._shopRepository.Get(sh => sh.Id == request.ShopId
            ,includes: new List<Expression<Func<Domain.Entities.Shop, object>>>()
            {
                shp => shp.Account,
            }).SingleOrDefault();
            var customerAccount = this._currentAccountService.GetCurrentAccount();
            await this.SendNotificationAsync(shop.Account.Email, shop.AccountId,
                shop.Account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                string.Format(NotificationMessageConstants.Order_Fail, customerAccount.LastName),
                (int)Domain.Enums.Roles.Shop,
                customerAccount.AvatarUrl
                );
            this._unitOfWork.RollbackTransaction();
            throw exception;
        }
    }

    private async Task SendNotificationAsync(string accountEmail, int accountId, string deviceToken, string title,
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
            if (role == (int)Domain.Enums.Roles.Customer)
            {
                this._firebaseFirestoreService.AddNewNotifyCollectionToUser(accountEmail,
                    FirebaseStoreConstants.Order_Type,
                    (int)OrderStatus.Pending,
                    content);
            }else if (role == (int)Domain.Enums.Roles.Shop)
            {
                this._firebaseFirestoreService.AddNewNotifyCollectionToShop(accountEmail,
                    FirebaseStoreConstants.Order_Type,
                    (int)OrderStatus.Pending,
                    content);
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
        }
    }

    private string CreateNoteForOrder(List<ProductInOrderRequest> productRequest)
    {
        List<string> notes = new List<string>();
        foreach (var pro in productRequest)
        {
            var product = this._productRepository.GetById(pro.Id);
            string note = product.Id + StringPatterConstants.SEPARATE_ORDERID + product.Name + ":" + pro.Note;
            notes.Add(note);
        }

        return string.Join(StringPatterConstants.SEPARATE_ORDER_PRODUCT, notes);
    }

    private void CheckProductIsAllAvailable(List<ProductInOrderRequest> products)
    {
        var listProducts = this._productRepository.Get(predicate: pro => products.Select(x => x.Id).Contains(pro.Id)).ToList();
        if (listProducts.Any(pro => pro.Status != (int)ProductStatus.Active))
            throw new InvalidBusinessException($"Sản phẩm với ids {string.Join(",",
                listProducts.Where(pro => pro.Status == (int)ProductStatus.Active).Select(pro => pro.Id).ToList())} không còn tồn tại");
    }

    private void CheckToppingAvailable(List<ProductInOrderRequest> products)
    {
        var listToppingId = products.SelectMany(pro => pro.Topping.Radio.Select(x => x.Id));
        listToppingId = listToppingId.Concat(products.SelectMany(pro => pro.Topping.CheckBox.Select(x => x.Id)));
        var listQuestion = this._questionRepository.Get(predicate: que => listToppingId.Any(x => x == que.Id)).ToList();
        
        if (listQuestion.Any(que => que.Status != (int)QuestionStatus.Active))
            throw new InvalidBusinessException($"Câu hỏi với ids {string.Join(",",
                listQuestion.Where(x => x.Status != (int)QuestionStatus.Active).Select(x => x.Id).ToList())}");

        var listOptionId = products.SelectMany(pro => pro.Topping.CheckBox.SelectMany(x => x.OptionIds));
        listOptionId = listOptionId.Concat(products.SelectMany(pro => pro.Topping.Radio.Select(x => x.OptionId)));
        var listOption = this._optionRepository.Get(predicate: op => listOptionId.Any(x => x == op.Id)).ToList();
        this.OptionIds = listOptionId.ToList();

        if(listOption.Any(op => op.Status != (int)OptionStatus.Active))
            throw new InvalidBusinessException($"Lựa chọn với ids {string.Join(",",
                listOption.Where(x => x.Status != (int)OptionStatus.Active).Select(x => x.Id).ToList())}");
    }

    private void CheckTotalProductPrice(List<ProductInOrderRequest> products, double totalProductPrice)
    {
        double sumProductPrice = 0;
        foreach (var pro in products)
        {
            var product = this._productRepository.GetById(pro.Id);
            sumProductPrice += product.Price * pro.Quantity;

            foreach (var checkBoxTopping in pro.Topping.CheckBox)
            {
                foreach (var optionID in checkBoxTopping.OptionIds)
                {
                    var option = this._optionRepository.GetById(optionID);
                    sumProductPrice += option.Price * pro.Quantity;
                }
            }

            foreach (var radioToppting in pro.Topping.Radio)
            {
                var option = this._optionRepository.GetById(radioToppting.OptionId);
                sumProductPrice += option.Price * pro.Quantity;
            }
        }
        
        if (sumProductPrice != totalProductPrice)
            throw new InvalidBusinessException($"Giá của tổng sản phẩm hiện tại là {sumProductPrice} khác với giá {totalProductPrice}");
    }

    private void CheckVoucherAvailable(VoucherInOrderRequest voucher, double totalVoucherPrice, double totalProductPrice)
    {
        if (voucher.PromotionType == PromotionTypes.PersonPromotion)
        {
            var vou = this._personPromotionRepository.GetById(voucher.Id);
            
            if (vou.Status != (int)PersonPromotionStatus.Active)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn tồn tại");

            if (vou.StartDate > DateTime.Now || vou.EndDate < DateTime.Now)
                throw new InvalidBusinessException("Phiếu giảm giá đã hết hạn");

            if (vou.NumberOfUsed > vou.UsageLimit)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn lượt sử dụng");

            if (vou.MinimumOrderValue > totalProductPrice)
                throw new InvalidBusinessException($"Yếu cầu giá trị đơn hàng tối thiểu là {vou.MinimumOrderValue} để sử dụng phiếu giảm giá");

            if (vou.ApplyType == (int)PromotionApplyTypes.Percent)
            {
                var voucherAmount = (int)Math.Ceiling(vou.AmountRate * totalProductPrice / 100);
                if (vou.MaximumApplyValue <= voucherAmount)
                {
                    if (totalVoucherPrice != vou.MaximumApplyValue)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {voucherAmount}");
                    }
                }
                else
                {
                    if (totalVoucherPrice != voucherAmount)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {voucherAmount}");
                    }
                }
            }
            else
            {
                if (totalProductPrice >= vou.MinimumOrderValue)
                {
                    if (totalVoucherPrice != vou.AmountValue)
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {vou.AmountValue}");
                }
                    
            }

        }else if (voucher.PromotionType == PromotionTypes.PlatformPromotion)
        {
            var vou = this._platformPromotionRepository.GetById(voucher.Id);
            
            if (vou.Status != (int)PersonPromotionStatus.Active)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn tồn tại");
            
            if (vou.StartDate > DateTime.Now || vou.EndDate < DateTime.Now)
                throw new InvalidBusinessException("Phiếu giảm giá đã hết hạn");

            if (vou.NumberOfUsed > vou.UsageLimit)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn lượt sử dụng");

            if (vou.MinimumOrderValue > totalProductPrice)
                throw new InvalidBusinessException($"Yếu cầu giá trị đơn hàng tối thiểu là {vou.MinimumOrderValue} để sử dụng phiếu giảm giá");

            if (vou.ApplyType == (int)PromotionApplyTypes.Percent)
            {
                var voucherAmount = (int)Math.Ceiling(vou.AmountRate * totalProductPrice / 100);
                if (vou.MaximumApplyValue <= voucherAmount)
                {
                    if (totalVoucherPrice != vou.MaximumApplyValue)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {voucherAmount}");
                    }
                }
                else
                {
                    if (totalVoucherPrice != voucherAmount)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {voucherAmount}");
                    }
                }
            }
            else
            {
                if (totalProductPrice >= vou.MinimumOrderValue)
                {
                    if (totalVoucherPrice != vou.AmountValue)
                        throw new InvalidBusinessException($"Tổng tiền giảm không đúng {vou.AmountValue}");
                }
                    
            }
        }else if (voucher.PromotionType == PromotionTypes.ShopPromotion)
        {
            var vou = this._shopPromotionRepository.GetById(voucher.Id);
            
            if (vou.Status != (int)PersonPromotionStatus.Active)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn tồn tại");
            
            if (vou.StartDate > DateTime.Now || vou.EndDate < DateTime.Now)
                throw new InvalidBusinessException("Phiếu giảm giá đã hết hạn");

            if (vou.NumberOfUsed > vou.UsageLimit)
                throw new InvalidBusinessException($"Phiếu giảm giá không còn lượt sử dụng");

            if (vou.MinimumOrderValue > totalProductPrice)
                throw new InvalidBusinessException($"Yếu cầu giá trị đơn hàng tối thiểu là {vou.MinimumOrderValue} để sử dụng phiếu giảm giá");

            if (vou.ApplyType == (int)PromotionApplyTypes.Percent)
            {
                var voucherAmount = (int)Math.Ceiling(vou.AmountRate * totalProductPrice / 100);
                if (vou.MaximumApplyValue <= voucherAmount)
                {
                    if (totalVoucherPrice != vou.MaximumApplyValue)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm phải là {vou.MaximumApplyValue}");
                    }
                }
                else
                {
                    if (totalVoucherPrice != voucherAmount)
                    {
                        throw new InvalidBusinessException($"Tổng tiền giảm phải là {voucherAmount}");
                    }
                }
            }
            else
            {
                if (totalProductPrice >= vou.MinimumOrderValue)
                {
                    if (totalVoucherPrice != vou.AmountValue)
                        throw new InvalidBusinessException($"Tổng tiền giảm phải là {vou.AmountValue}");
                }
                    
            }
        }
    }

    private void CheckShippingFee(int shopId, double shippingFee, double totalPrice, double distance)
    {
        var shop = this._shopRepository.GetById(shopId);
        if (distance > 5)
        {
            if (totalPrice >= shop.MinimumValueOrderFreeship)
            {
                if (shippingFee > 0)
                    throw new InvalidBusinessException("Phí ship phải bằng 0");
            }
            else
            {
                if (shippingFee == 0)
                {
                    throw new InvalidBusinessException($"Phí ship phải bằng {shop.ShippingFee}");
                }
            }
        }
        
    }
    
}