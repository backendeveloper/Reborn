using System;
using Xunit;
using Reborn.Domain.Repository;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;
using Reborn.Service;
using Moq;
using Reborn.Common.Dto;
using System.Threading.Tasks;

namespace Service.Test
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTest()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockRepository.Object);
        }

        [Fact]
        public async Task should_get_category_by_id_returns_expected_dto()
		{
            //Given
            var expectedResult = new Category();
            expectedResult.Title = "Expected Category";
            expectedResult.Description = "Expected Description";

            _mockRepository
                .Setup(c => c.GetByIdAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(expectedResult));

            //When
            var result = await _categoryService.GetByIdAsync("testId");

            //Then
            Assert.IsType<CategoryDto>(result);

            Assert.Equal(result.Title, expectedResult.Title);
            Assert.Equal(result.Description, expectedResult.Description);
		}
    }
}
