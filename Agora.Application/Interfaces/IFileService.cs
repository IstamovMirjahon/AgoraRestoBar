using Microsoft.AspNetCore.Http;

namespace Agora.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile image, string folder, CancellationToken cancellationToken = default);
    }
}
