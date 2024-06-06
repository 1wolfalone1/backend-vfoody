using VFoody.Domain.Shared;

namespace VFoody.Application.Common.Services;

public interface IFirebaseVerifyIDTokenService
{
    Task<bool> VerifyIdToken(string idToken);

    Task<FirebaseUser> GetFirebaseUser(string idToken);
}