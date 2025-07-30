using Microsoft.AspNetCore.Http;

namespace Agora.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    }
}
