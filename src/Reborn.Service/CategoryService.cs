using AutoMapper;
using Reborn.Common.Dto;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;
using Reborn.Domain.Repository;
using Reborn.Service.RequestModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Reborn.Service.Validator;

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

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IServiceValidator serviceValidator) : base(serviceValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        #endregion

        #region ICategoryService Implementation

        public async Task<CategoryDto> GetByIdAsync(StandartRequestModels.GetByIdRequestModel requestModel)
        {
            _serviceValidator.ModelValidate(requestModel);
            var category = await _categoryRepository.GetByIdAsync(requestModel.Id);

            return await Task.FromResult(_mapper.Map<CategoryDto>(category));
        }

        public async Task<PagedList<CategoryDto>> GetPageAsync(CategoryRequestModels.GetPageRequestModel requestModel)
        {
            _serviceValidator.ModelValidate(requestModel);

            Expression<Func<Category, bool>> predicate = (p) => (p.Status == requestModel.Status);
            var pagedCategory = _categoryRepository
                                .GetPage<Guid>(new Pagination(requestModel.Paging.Page, requestModel.Paging.PageSize), predicate, o => o.Id, false, requestModel.Paging.TotalCount);
            var result = new PagedList<CategoryDto>(pagedCategory.Data.Select(_mapper.Map<CategoryDto>).ToList(), pagedCategory.TotalCount);

            return await Task.FromResult(result);
        }

        #endregion 
    }
}
