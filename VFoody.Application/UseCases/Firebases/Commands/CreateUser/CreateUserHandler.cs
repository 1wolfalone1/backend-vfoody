using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Firebases.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result>
{
    private readonly IFirebaseAuthenticateUserService _firebaseAuthenticate;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IFirebaseAuthenticateUserService firebaseAuthenticate, ILogger<CreateUserHandler> logger)
    {
        _firebaseAuthenticate = firebaseAuthenticate;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var firebaseUid = await this._firebaseAuthenticate.CreateUser(request.Email, request.PhoneNumber,
                request.Password, request.DisplayName,
                request.AvatarUrl);
            return Result.Success(new
            {
                Uid = firebaseUid
            });
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw new InvalidBusinessException("Không thể tạo user trên firebase");
        }
    }
}