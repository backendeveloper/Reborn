using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using Reborn.Common.Core.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace Reborn.Service
{
    public abstract class BaseService
    {
        protected bool ModelValidate(object obj) 
        {
            var validator = GetValidator(obj);
            if (validator == null)
                return true;

            var validationResult = validator.Validate(obj);

            if (!validationResult.IsValid)
                throw new InvalidModelException(validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));

            return true;
        }
         
        private IValidator GetValidator(object obj)
        {
            Type type = obj.GetType();
            var validatorAttributes = type.GetTypeInfo().GetCustomAttributes(typeof(ValidatorAttribute), true).ToList();

            if (validatorAttributes.Count > 0)
            {
                var validatorAttribute = (ValidatorAttribute)validatorAttributes[0];
                return Activator.CreateInstance(validatorAttribute.ValidatorType) as IValidator;
            }
            return null;
        }
    }
}
