using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Reborn.Common.Dto;
using Reborn.Domain.Model;
using Reborn.Domain.Repository;
using Xunit;

namespace Reborn.Service.Tests
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly ICategoryService _categoryService;
        private readonly Mock<IMapper> _mapper;

        public CategoryServiceTest()
        {
            _mapper = new Mock<IMapper>();
            _mockRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockRepository.Object, _mapper.Object);
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

            _mapper
                .Setup(c => c.Map<CategoryDto>(expectedResult))
                    .Returns(() => new CategoryDto()
                    {
                        Description = expectedResult.Description,
                        Title = expectedResult.Title
                    });

            //When
            var result = await _categoryService.GetByIdAsync("ff02e91b-9e6e-4ddc-8d2b-cce0ac565b60");

            //Then
            Assert.IsType<CategoryDto>(result);

            Assert.Equal(result.Title, expectedResult.Title);
            Assert.Equal(result.Description, expectedResult.Description);
        }
    }
}
