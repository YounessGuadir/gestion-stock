using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;

namespace GestionStock.Application.Categories.Commands.Delete;

public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork _uow;

    public DeleteCategoryHandler(ICategoryRepository categories, IUnitOfWork uow)
    {
        _categories = categories;
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand command, CancellationToken ct)
    {
        var category = await _categories.GetByIdAsync(command.TenantId, command.Id, ct);
        if (category is null)
            throw new NotFoundException("Category not found.");

        _categories.Delete(category);
        await _uow.SaveChangesAsync(ct);

        return Result<bool>.Ok(true);
    }
}