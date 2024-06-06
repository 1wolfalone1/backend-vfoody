using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Infrastructure.Services;

public class FirebaseVerifyIDTokenService : IBaseService, IFirebaseVerifyIDTokenService
{
    public void CreateFirebaseAuth()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./firebase_credentials.json")
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

        // Map to FirebaseUser object
        FirebaseUser firebaseUser = new FirebaseUser
        {
            UserId = userId,
            PhoneNumber = phoneNumber,
            Email = email
        };

        return firebaseUser;
    }
    
    
}