using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Infrastructure.Services;

public class FirebaseVerifyIDTokenService : IBaseService, IFirebaseVerifyIDTokenService
{
    private readonly IConfiguration _configuration;

    public FirebaseVerifyIDTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CreateFirebaseAuth()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = _configuration["PROJECT_ID"],
            });
        }
        
    }
    
    public async Task<bool> VerifyIdToken(string idToken){
        CreateFirebaseAuth();
        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
            .VerifyIdTokenAsync(idToken);
        return decodedToken != null;
    }

    public async Task<FirebaseUser> GetFirebaseUser(string idToken)
    {
        CreateFirebaseAuth();
        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
            .VerifyIdTokenAsync(idToken);
        
        string userId = decodedToken.Uid;
        string phoneNumber = decodedToken.Claims.ContainsKey("phone_number") ? decodedToken.Claims["phone_number"].ToString() : null;
        string email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : null;
        bool emailVerified = decodedToken.Claims.ContainsKey("email_verified") && (bool)decodedToken.Claims["email_verified"];
        string name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : null;
        string picture = decodedToken.Claims.ContainsKey("picture") ? decodedToken.Claims["picture"].ToString() : null;

        // Map to FirebaseUser object
        FirebaseUser firebaseUser = new FirebaseUser
        {
            UserId = userId,
            PhoneNumber = phoneNumber,
            Email = email,
            Name = name,
            Picture = picture,
            EmailVerified = emailVerified
        };

        return firebaseUser;
    }
    
    
}