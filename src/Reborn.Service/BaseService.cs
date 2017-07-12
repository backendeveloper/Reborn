using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using Reborn.Common.Core.Exceptions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Reborn.Service
{
    public abstract class BaseService
    {

    }
}

public interface IServiceValidator
{
    bool ModelValidate(object obj);
    Task<bool> ModelValidateAsync(object obj);
    IValidator GetValidator(object obj);
}

public class ServiceValidator : IServiceValidator
{
    public IValidator GetValidator(object obj)
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

    public bool ModelValidate(object obj)
    {
        var validator = GetValidator(obj);
        if (validator == null)
            return true;

        var validationResult = validator.Validate(obj);

        if (!validationResult.IsValid)
            throw new InvalidModelException(validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));

        return true;
    }

    public async Task<bool> ModelValidateAsync(object obj)
    {
        var validator = GetValidator(obj);
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(obj);
            if (!validationResult.IsValid)
                throw new InvalidModelException(validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));
        }

        return await Task.FromResult(true);
    }
}
