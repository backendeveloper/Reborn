using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Reborn.Web.Api.Utils.Exception
{

    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var errors = context.ModelState
                .Where(x => x.Value != null)
                .ToDictionary(s => s.Key,
                    s => string.Join("|", s.Value.Errors.Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(c => c.ErrorMessage)));

            var exceptionModel = new ExceptionModel()
            {
                Type = ExceptionModel.ExceptionType.Validation.ToString(),
                Key = "model_is_invalid",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(exceptionModel);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
