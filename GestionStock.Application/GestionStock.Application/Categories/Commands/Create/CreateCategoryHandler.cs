using GestionStock.Application.Abstractions;
using GestionStock.Application.Categories.DTOs;
using GestionStock.Application.Common;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Commands.Create
{
    public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly ICategoryRepository _categories;
        private readonly IUnitOfWork _uow;


        public CreateCategoryHandler(ICategoryRepository categories, IUnitOfWork uow)
        {
            _categories = categories;
            _uow = uow;
        }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand command,CancellationToken token)
        {
            var category = new Category(command.TenantId,command.Name,command.Description);
            await _categories.AddAsync(category, token);
            await _uow.SaveChangesAsync(token);

            var dto = new CategoryDto(category.Id,category.TenantId,category.Name,category.Description,category.CreatedAt,category.UpdatedAt);
            return Result<CategoryDto>.Ok(dto);

        }
    }
}
