using GestionStock.Application.Common.Interfaces;

namespace GestionStock.Infrastructure.Files;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(string rootPath)
    {
        _rootPath = rootPath;
    }

    public async Task<string?> SaveProductImageAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var uploadsFolder = Path.Combine(_rootPath, "uploads", "products");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(fileName);
        var safeFileName = $"{Guid.NewGuid()}{extension}";

        var fullPath = Path.Combine(uploadsFolder, safeFileName);

        using var output = new FileStream(fullPath, FileMode.Create);

        await fileStream.CopyToAsync(output, cancellationToken);

        return $"/uploads/products/{safeFileName}";
    }
}