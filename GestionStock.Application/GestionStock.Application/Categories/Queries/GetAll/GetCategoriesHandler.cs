using GestionStock.Application.Abstractions;
using GestionStock.Application.Categories.DTOs;
using GestionStock.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Queries.GetAll
{
    public class GetCategoriesHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categories;

        public GetCategoriesHandler(ICategoryRepository categories)
        {
            _categories = categories;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken token)
        {
            var list = await _categories.GetAllAsync(query.TenantId, token);

            return list.Select(c => new CategoryDto(c.Id, c.TenantId, c.Name, c.Description, c.CreatedAt, c.UpdatedAt)).ToList();
        }

    }
}
