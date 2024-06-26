using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using VFoody.Application.Common.Services;

namespace VFoody.Infrastructure.Services;

public class FirebaseAuthenticateUserService : BaseService, IFirebaseAuthenticateUserService
{
    private readonly IConfiguration _configuration;

    public FirebaseAuthenticateUserService(IConfiguration configuration)
    {
        _configuration = configuration;
        this.CreateFirebaseAuth();
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

    public async Task<string> CreateUser(string email, string phoneNumber, string password, string name, string avatar)
    {
        try
        {
            phoneNumber = phoneNumber != null ? "+84" + phoneNumber.Substring(1) : null;
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = email,
                EmailVerified = true,
                PhoneNumber = phoneNumber,
                Password = password,
                DisplayName = name,
                PhotoUrl = avatar,
                Disabled = false,
            };
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
            
            // See the UserRecord reference doc for the contents of userRecord.
            Console.WriteLine($"Successfully created new user: {userRecord.Uid}");
            return userRecord.Uid;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error created new user: " + e.Message);
            throw;
        }
    }
    
    public async Task<bool> IsEmailRegisteredAsync(string email)
    {
        try
        {
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
            return userRecord != null;
        }
        catch (FirebaseAuthException ex)
        {
            if (ex.AuthErrorCode == AuthErrorCode.UserNotFound)
            {
                return false;
            }
            throw;
        }
    }

    public async Task<string> CreateCustomerToken(string uid)
    {
        try
        {
            string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);
            Console.WriteLine($"Firebase Customer Token: {uid}");
            return customToken;
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine("Error created Custom token: " + ex.Message);
            throw;
        }
    }
}