using System;
using AutoMapper;
using Reborn.Common.Dto;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Repository;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Reborn.Domain.Model;
using Reborn.Service.RequestModels;

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
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<CategoryDto> GetByIdAsync(StandartRequestModels.GetByIdRequestModel requestModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<PagedList<CategoryDto>> GetPageAsync(CategoryRequestModels.GetPageRequestModel requestModel);
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

        #region ICategoryService Implementation

        public async Task<CategoryDto> GetByIdAsync(StandartRequestModels.GetByIdRequestModel requestModel)
        {
            var category = await _categoryRepository.GetByIdAsync(requestModel.Id);

            return await Task.FromResult(_mapper.Map<CategoryDto>(category));
        }

        public async Task<PagedList<CategoryDto>> GetPageAsync(CategoryRequestModels.GetPageRequestModel requestModel)
        {
            Expression<Func<Category, bool>> predicate = (p) => (p.Status == requestModel.Status);
            var pagedCategory = _categoryRepository
                                .GetPage<Guid>(new Pagination(requestModel.Page, requestModel.PageSize), predicate, o => o.Id, false, requestModel.TotalCount);
            var result = new PagedList<CategoryDto>(pagedCategory.Data.Select(_mapper.Map<CategoryDto>).ToList(), pagedCategory.TotalCount);

            return await Task.FromResult(result);
        }

        #endregion
    }
}
