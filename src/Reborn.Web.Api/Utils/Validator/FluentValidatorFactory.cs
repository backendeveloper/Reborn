using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Reborn.Web.Api.Utils.Validator
{

    public class FluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FluentValidatorFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetService(validatorType) as IValidator;
            }
        }
    }
}
