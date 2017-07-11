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

            //public bool IsValid(out IList<ValidationFailure> validationFailures)
            //{
            //    var validator = new CategoryRequestModelsValidator.GetPageRequestModelValidator();
            //    var results = validator.Validate(this);
            //    validationFailures = results.Errors;

            //    return results.IsValid;
            //}
        }

        public abstract class BaseRequestModel<T> : ValidatorFactoryBase
        {
            public bool IsValid(out IList<ValidationFailure> validationFailures)
            {
                var vt = typeof(AbstractValidator<>);
                var et = typeof(T).GetType();
                var evt = vt.MakeGenericType(et);


                IValidator validator = (IValidator)evt;
                //if (validator != null) //if there was a validator
                //{
                //    var validationResult = validator.Validate(nvp.Value); //Validate based upon the Fluent rules registered

                //    if (!validationResult.IsValid) //If there was an error, return it to the front end
                //        throw new HttpResponseException(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Join(",", (validationResult.Errors.Select(err => err.ErrorMessage)))));
                //}
                


                var validatorType = FindValidatorType(Assembly.GetEntryAssembly(), evt);

              //  var validator = CreateInstance(validatorType); //new CategoryRequestModelsValidator.GetPageRequestModelValidator();
                var results = validator.Validate(this);
                validationFailures = results.Errors;

                return results.IsValid;

            }

            public static Type FindValidatorType(Assembly assembly, Type evt)
            {
                if (assembly == null) throw new ArgumentNullException("assembly");
                if (evt == null) throw new ArgumentNullException("evt");
                return assembly.GetTypes().FirstOrDefault(t => t.GetType().GetTypeInfo().IsSubclassOf(evt));
            }

            public override IValidator CreateInstance(Type validatorType)
            {
                return base.GetValidator(validatorType);
            }
        }
    }
}
