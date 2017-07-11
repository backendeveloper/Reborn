using FluentValidation.Attributes;
using Reborn.Web.Api.V1.Models.Validators;

namespace Reborn.Web.Api.V1.Models
{
    [Validator(typeof(CategoryCreateViewModelValidator))]
    public class CategoryCreateViewModel 
    {      
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
