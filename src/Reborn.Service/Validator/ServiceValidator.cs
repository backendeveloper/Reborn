using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Reborn.Common.Core.Exceptions;

namespace Reborn.Service.Validator
{
    public interface IServiceValidator
    {
        bool ModelValidate<T>(T model);
    }

    public class ServiceValidator : IServiceValidator
    {
        private readonly IValidatorFactory _validatorFactory;
        public ServiceValidator(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public bool ModelValidate<T>(T model)
        {
            var type = model.GetType();
            var validator = _validatorFactory.GetValidator(type);
            if (validator == null)
                return true;

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
                throw new InvalidModelException(validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));

            return true;
        }
    }
}
