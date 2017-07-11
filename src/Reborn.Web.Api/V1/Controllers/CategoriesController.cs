using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reborn.Service;
using Reborn.Service.FilterModels;
using Reborn.Web.Api.V1.Models;
using Reborn.Web.Api.V2.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            throw new Exception("asdasd");

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
        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Note that the key is a GUID and not an integer.
        ///  
        ///     POST /Todo
        ///     {
        ///        "key": "0e7ad584-7788-4ab1-95a6-ca0a5b444cbb",
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>New Created Category Item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryCreateViewModel), 201)]
        [ProducesResponseType(typeof(CategoryCreateViewModel), 400)]
        public async Task<IActionResult> Post(CategoryCreateViewModel model)
        {

            return Ok();
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
