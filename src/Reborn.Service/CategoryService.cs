using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Reborn.Common.Core.Extensions;
using Reborn.Common.Dto;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;
using Reborn.Domain.Repository;
using Reborn.Service.RequestModels;

namespace Reborn.Service
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(string id);
        PagedList<CategoryDto> GetPage(int page, int pageSize, bool totalCount);

        Task<CategoryDto> FirstOrDefaultAsync(CategoryRequestModel requestModel);
    }

    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService()
        {
            //For unit testing only
        }

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> GetByIdAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return await Task.FromResult(AutoMapper.Mapper.Map<CategoryDto>(category));
        }

        public PagedList<CategoryDto> GetPage(int pageNumber, int pageSize, bool totalCount)
        {
            var categoryPage = _categoryRepository.GetPage(new Pagination(pageNumber, pageSize), x => x.Id != null, o => o.Id, totalCount);

            return new PagedList<CategoryDto>(categoryPage.Data.Select(AutoMapper.Mapper.Map<CategoryDto>).ToList(), categoryPage.TotalCount);
        }

        public async Task<CategoryDto> FirstOrDefaultAsync(CategoryRequestModel requestModel)
        {
            Expression<Func<Category, bool>> expression = null;
            if (requestModel.IsChanged(nameof(requestModel.ShowOnMenu)) && requestModel.ShowOnMenu)
            {
                expression = (p) => p.ShowOnMenu;
            }
            if (requestModel.IsChanged(nameof(requestModel.Slug)) && !string.IsNullOrEmpty(requestModel.Slug))
            {
                expression = expression.And((p) => p.Slug == requestModel.Slug);
            }
            if (requestModel.IsChanged(nameof(requestModel.Status)) && requestModel.Status > 0)
            {
                expression = expression.And((p) => p.Status == requestModel.Status);
            }

            var category = await _categoryRepository.FirstOrDefaultAsync(expression);

            return await Task.FromResult(AutoMapper.Mapper.Map<CategoryDto>(category));
        }
    }
}
