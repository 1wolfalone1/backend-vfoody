using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Services;

namespace VFoody.Infrastructure.Services;

public class StorageService : IStorageService, IBaseService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _client;

    public StorageService(ILogger<StorageService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _client = new AmazonS3Client(
            new BasicAWSCredentials(_configuration["AWS_ACCESS_KEY"] ?? "", _configuration["AWS_SECRET_KEY"] ?? ""),
            new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_configuration["AWS_REGION"] ?? "")
            }
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var bucketName = _configuration["AWS_BUCKET_NAME"] ?? "";
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Guid.NewGuid()}";
        if (file.ContentType.StartsWith("image/"))
        {
            fileName = "image/" + fileName;
        }
        else if (file.ContentType.StartsWith("video/"))
        {
            fileName = "video/" + fileName;
        }
        var uploadRequest = new TransferUtilityUploadRequest()
        {
            InputStream = ms,
            Key = fileName,
            BucketName = bucketName,
            ContentType = file.ContentType
        };
        var transferUtility = new TransferUtility(_client);
        await transferUtility.UploadAsync(uploadRequest);
        var imageUrl = _configuration["AWS_BASE_URL"] + fileName;
        _logger.LogInformation("Push to s3 success with link url {0}", imageUrl);
        return imageUrl;
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var bucketName = _configuration["AWS_BUCKET_NAME"] ?? "";
        var deleteRequest = new DeleteObjectRequest()
        {
            BucketName = bucketName,
            Key = fileName
        };
        var response = await _client.DeleteObjectAsync(deleteRequest);
        if (response.HttpStatusCode == HttpStatusCode.OK) return true;
        _logger.LogError("Delete file S3 fail");
        return false;
    }
}