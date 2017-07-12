using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Reborn.Service;
using Reborn.Service.RequestModels;
using Reborn.Web.Api.Utils.Exception;
using Reborn.Web.Api.V1.Models;
using System.Threading.Tasks;

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
            var result = await _categoryService.GetPageAsync(new CategoryRequestModels.GetPageRequestModel()
            {
                Paging = new StandartRequestModels.BasePagingModel()
                {
                    Page = 1,
                    PageSize = 20,
                    TotalCount = true,
                },
                Slug = null,
                Status = 2
            });

            return Ok(result);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var category = await _categoryService.GetByIdAsync(new StandartRequestModels.GetByIdRequestModel()
            {
                Id = id
            });

            return Ok(category);
        }

        // POST api/categories        
        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Note that the key is a GUID and not an integer.
        ///  
        ///     POST /Categories
        ///     {
        ///        "Title": "Item1",
        ///        "Description": "desc"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>New Created Category Item</returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryCreateViewModel), 200)]
        [ProducesResponseType(typeof(ExceptionModel), 400)]
        public async Task<IActionResult> Post([FromBody]CategoryCreateViewModel model)
        {

            return Ok("asd");
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
