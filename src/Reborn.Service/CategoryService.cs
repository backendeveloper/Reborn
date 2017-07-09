using System;
using AutoMapper;
using Reborn.Common.Dto;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Repository;
using Reborn.Service.FilterModels;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Reborn.Domain.Model;

namespace Reborn.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        Task<CategoryDto> GetByIdAsync(StandartFilterModels.GetByIdFilterModel filterModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        Task<PagedList<CategoryDto>> GetPageAsync(CategoryFilterModels.GetPageFilterModel filterModel);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CategoryService : BaseService, ICategoryService
    {
        #region Private Members

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        #endregion

        #region ICategoryService implements

        public async Task<CategoryDto> GetByIdAsync(StandartFilterModels.GetByIdFilterModel filterModel)
        {
            var category = await _categoryRepository.GetByIdAsync(filterModel.Id);

            return await Task.FromResult(_mapper.Map<CategoryDto>(category));
        }

        public async Task<PagedList<CategoryDto>> GetPageAsync(CategoryFilterModels.GetPageFilterModel filterModel)
        {
            Expression<Func<Category, bool>> predicate = (p) => (p.Status == filterModel.Status);
            var pagedCategory = _categoryRepository
                                .GetPage<Guid>(new Pagination(filterModel.Page, filterModel.PageSize), predicate, o => o.Id,false, filterModel.TotalCount);
            var result = new PagedList<CategoryDto>(pagedCategory.Data.Select(_mapper.Map<CategoryDto>).ToList(), pagedCategory.TotalCount);

            return await Task.FromResult(result);
        }

        #endregion
    }
}
