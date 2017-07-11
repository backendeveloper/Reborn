using FluentValidation.Attributes;
using Reborn.Web.Api.V1.Models.Validators;

namespace Reborn.Web.Api.V1.Models
{
    /// <summary>
    /// Category Create Model
    /// </summary>
    [Validator(typeof(CategoryCreateViewModelValidator))]
    public class CategoryCreateViewModel 
    {      
        /// <summary>
        /// Title alanıdır
        /// </summary>
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
