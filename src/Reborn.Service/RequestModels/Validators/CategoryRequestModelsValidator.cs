using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace Reborn.Service.RequestModels.Validators
{
    public class CategoryRequestModelsValidator
    {
        public class GetPageRequestModelValidator : FluentValidation.AbstractValidator<CategoryRequestModels.GetPageRequestModel>
        {
            public GetPageRequestModelValidator()
            {
                RuleFor(x => x.Status).GreaterThan(0).WithMessage("status_is_greater_than_to_zero");
                RuleFor(x => x.Slug).NotNull().WithMessage("slug_is_not_null");
            }
        }
    }
}
