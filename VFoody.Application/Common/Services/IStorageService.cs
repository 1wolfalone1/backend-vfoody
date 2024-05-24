using Microsoft.AspNetCore.Http;

namespace VFoody.Application.Common.Services;

public interface IStorageService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string fileName);
}