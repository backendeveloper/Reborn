using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reborn.Service;
using Reborn.Service.FilterModels;
using Reborn.Web.Api.V2.Controllers;

namespace Reborn.Web.Api.V1.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : BaseV1Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET api/categories
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetPageAsync(new CategoryFilterModels.GetPageFilterModel()
            {
                Page = 1,
                PageSize = 20,
                TotalCount = true,
                Slug = "turkiye",
                Status = 2
            });

            return Ok(result);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var category = await _categoryService.GetByIdAsync(new StandartFilterModels.GetByIdFilterModel()
            {
                Id = id
            });

            return Ok(category);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
