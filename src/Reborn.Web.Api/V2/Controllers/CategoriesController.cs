using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reborn.Service;

namespace Reborn.Web.Api.V2.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : BaseV2Controller
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
            return Ok();
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok();
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
