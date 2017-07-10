using FluentValidation;

namespace Reborn.Web.Api.V1.Models.Validators
{
    public class CategoryCreateViewModelValidator : FluentValidation.AbstractValidator<CategoryCreateViewModel>
    {
        public CategoryCreateViewModelValidator()
        {
            base.RuleFor(x => x.Title).NotNull().WithMessage("title_is_requeired");
        }
    }
}
