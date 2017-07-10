using FluentValidation;
using FluentValidation.Attributes;

namespace Reborn.Web.Api.V1.Models
{

    [Validator(typeof(CategoryCreateViewModelValidator))]
    public class CategoryCreateViewModel
    {
        public string Title { get; set; } 
    }


}
