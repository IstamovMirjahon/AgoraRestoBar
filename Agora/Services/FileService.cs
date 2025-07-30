// Infrastructure/Services/FileService.cs
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Agora.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        string baseFolder = Path.Combine(_env.WebRootPath, folder);
        if (!Directory.Exists(baseFolder))
            Directory.CreateDirectory(baseFolder);

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string fullPath = Path.Combine(baseFolder, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"/{folder}/{fileName}".Replace("\\", "/");
    }

}
