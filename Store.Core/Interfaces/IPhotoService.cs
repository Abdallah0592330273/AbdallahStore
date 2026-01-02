using Microsoft.AspNetCore.Http;

namespace Store.Core.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
        Task<string> DeletePhotoAsync(string publicId);
    }

    public record PhotoUploadResult(string PublicId, string Url, string? Error = null);
}
