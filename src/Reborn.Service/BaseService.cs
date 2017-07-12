using Reborn.Service.Validator;

namespace Reborn.Service
{
    public abstract class BaseService
    {
        protected readonly IServiceValidator _serviceValidator;

        protected BaseService(IServiceValidator serviceValidator)
        {
            _serviceValidator = serviceValidator;
        }
    }
}




