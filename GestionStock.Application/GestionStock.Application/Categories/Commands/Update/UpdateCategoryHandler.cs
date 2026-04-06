using GestionStock.Application.Abstractions;
using GestionStock.Application.Categories.DTOs;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;

namespace GestionStock.Application.Categories.Commands.Update;

public class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categories;
    private readonly IUnitOfWork _uow;

    public UpdateCategoryHandler(ICategoryRepository categories, IUnitOfWork uow)
    {
        _categories = categories;
        _uow = uow;
    }

    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        var category = await _categories.GetByIdAsync(command.TenantId, command.Id, ct);
        if (category is null)
            throw new NotFoundException("Category not found.");

        category.Update(command.Name, command.Description); // لازم تكون عندك فـ Domain
        _categories.Update(category);

        await _uow.SaveChangesAsync(ct);

        var dto = new CategoryDto(category.Id, category.TenantId, category.Name, category.Description, category.CreatedAt, category.UpdatedAt);
        return Result<CategoryDto>.Ok(dto);
    }
}