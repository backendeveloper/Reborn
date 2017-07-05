using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reborn.Service;
using Reborn.Service.RequestModels;

namespace Reborn.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private readonly ICategoryService _categoryService;

        public ValuesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = _categoryService.GetPage(1, 20, true);

            var rm = new CategoryRequestModel
            {
                [nameof(CategoryRequestModel.ShowOnMenu)] = true,
                [nameof(CategoryRequestModel.Slug)] = "turkiye"
            };
            // rm[nameof(rm.Status)] = 2;

            var ct = await _categoryService.FirstOrDefaultAsync(rm);



            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {



            var category = await _categoryService.GetByIdAsync(id);

            return Ok(new
            {
                category.Title,
                category.Description
            });
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
