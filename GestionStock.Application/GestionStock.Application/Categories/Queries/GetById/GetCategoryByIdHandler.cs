using GestionStock.Application.Abstractions;
using GestionStock.Application.Categories.DTOs;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;

namespace GestionStock.Application.Categories.Queries.GetById;

public class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryRepository _categories;

    public GetCategoryByIdHandler(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        var category = await _categories.GetByIdAsync(query.TenantId, query.Id, ct);
        if (category is null)
            throw new NotFoundException("Category not found.");

        return new CategoryDto(
            category.Id, category.TenantId, category.Name, category.Description, category.CreatedAt, category.UpdatedAt
        );
    }
}