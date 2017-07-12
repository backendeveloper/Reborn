using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using Reborn.Common.Core.Exceptions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Reborn.Service.RequestModels;

namespace Reborn.Service
{
    public abstract class BaseService
    {

    }
}

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
