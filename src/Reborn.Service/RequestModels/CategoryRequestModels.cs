using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Reborn.Service.RequestModels.Validators;
using System.Collections.Generic;
using FluentValidation.Attributes;
using System.Reflection;
using System.Linq;

namespace Reborn.Service.RequestModels
{
    public class CategoryRequestModels
    {
        [FluentValidation.Attributes.Validator(typeof(CategoryRequestModelsValidator.GetPageRequestModelValidator))]
        public class GetPageRequestModel : BaseRequestModel<GetPageRequestModel>
        {
            public int Status { get; set; }
            public string Slug { get; set; }
            public StandartRequestModels.BasePagingModel Paging { get; set; }
        }

        public abstract class BaseRequestModel<T>
        {
           
           
        }
    }
}
