using System.Collections;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using VFoody.Application.Common.Services;

namespace VFoody.Infrastructure.Services;

public class FirebaseFirestoreService : BaseService, IFirebaseFirestoreService
{
    private readonly IConfiguration _configuration;

    private readonly FirestoreDb _database;
    public FirebaseFirestoreService(IConfiguration configuration)
    {
        _configuration = configuration;
        this.CreateFirebaseAuth();
        _database = FirestoreDb.Create(_configuration["PROJECT_ID"]);
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

    public async Task<bool> AddNewNotifyCollectionToUser(string email, string type, int status, string message)
    {
        CollectionReference notificationsCollection = _database.Collection("Notify").Document($"{email}:user").Collection("Notification");

        Dictionary<string, object> notification = new Dictionary<string, object>()
        {
            { "Type", type },
            { "Status", status },
            { "Message", message },
            { "Created_Date", DateTime.UtcNow },
            { "Updated_Date", DateTime.UtcNow },
        };

        await notificationsCollection.AddAsync(notification);
        return true;
    }
    
    public async Task<bool> AddNewNotifyCollectionToShop(string email, string type, int status, string message)
    {
        CollectionReference notificationsCollection = _database.Collection("Notify").Document($"{email}:shop").Collection("Notification");

        Dictionary<string, object> notification = new Dictionary<string, object>()
        {
            { "Type", type },
            { "Status", status },
            { "Message", message },
            { "Created_Date", DateTime.UtcNow },
            { "Updated_Date", DateTime.UtcNow },
        };

        await notificationsCollection.AddAsync(notification);
        return true;
    }
}