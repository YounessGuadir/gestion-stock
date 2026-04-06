namespace GestionStock.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string?> SaveProductImageAsync(
            Stream fileStream,
            string fileName,
            CancellationToken cancellationToken = default);
    }
}