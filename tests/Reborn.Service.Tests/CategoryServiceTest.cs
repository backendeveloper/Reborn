using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver.Linq;
using Moq;
using Reborn.Common.Dto;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;
using Reborn.Domain.Repository;
using Reborn.Service.RequestModels;
using Xunit;

namespace Reborn.Service.Tests
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly ICategoryService _categoryService;
        private readonly Mock<IMapper> _mockMapper;


        public CategoryServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task should_get_category_by_id_returns_expected_dto()
        {
            //Given
            var expectedResult = new Category
            {
                Id = Guid.Parse("ff02e91b-9e6e-4ddc-8d2b-cce0ac565b60"),
                Title = "Expected Category",
                Description = "Expected Description"
            };

            _mockRepository
                .Setup(c => c.GetByIdAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(expectedResult));

            _mockMapper
                .Setup(c => c.Map<CategoryDto>(expectedResult))
                .Returns(() => new CategoryDto()
                {
                    Id = expectedResult.Id,
                    Description = expectedResult.Description,
                    Title = expectedResult.Title
                });

            //When
            var result = await _categoryService.GetByIdAsync(new StandartRequestModels.GetByIdRequestModel()
            {
                Id = "ff02e91b-9e6e-4ddc-8d2b-cce0ac565b60"
            });

            //Then
            Assert.IsType<CategoryDto>(result);

            Assert.Equal(result.Title, expectedResult.Title);
            Assert.Equal(result.Description, expectedResult.Description);
            Assert.Equal(result.Id, expectedResult.Id);
        }

        [Fact]
        public async Task should_get_category_page_returns_dto_paged_list()
        {
            var data = new List<Category>()
            {
                new Category()
                {
                    Id = Guid.Parse("ff02e91b-9e6e-4ddc-8d2b-cce0ac565b60"),
                    Title = "Expected Category",
                    Description = "Expected Description",
                    Status = 2
                },
                new Category()
                {
                    Id = Guid.Parse("ff02e91b-9e6e-4ddc-8d2b-cce0ac565b60"),
                    Title = "Expected Category",
                    Description = "Expected Description",
                    Status = 2
                }
            };
            var expectedResult = new PagedList<Category>(data, 8);

            _mockRepository
                .Setup(c => c.GetPage(It.IsAny<Pagination>(), It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Expression<Func<Category, Guid>>>(), false, true))
                .Returns(() => expectedResult);

            _mockMapper
                .Setup(c => c.Map<PagedList<CategoryDto>>(expectedResult))
                .Returns(() => new PagedList<CategoryDto>(new List<CategoryDto>(), 8));

            //When
            var result = await _categoryService.GetPageAsync(new CategoryRequestModels.GetPageRequestModel()
            {
                Paging = new StandartRequestModels.BasePagingModel()
                {
                    Page = 1,
                    PageSize = 20,
                    TotalCount = true,
                },
                Slug = "turkiye",
                Status = 2
            });

            //Then
            Assert.IsType<PagedList<CategoryDto>>(result);

            Assert.Equal(result.TotalCount, expectedResult.TotalCount);
            Assert.Equal(result.Data.Count, expectedResult.Data.Count);
        }
    }
}
