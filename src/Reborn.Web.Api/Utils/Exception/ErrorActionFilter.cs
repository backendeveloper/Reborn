using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace Reborn.Web.Api.Utils.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
                return;

            var exceptionModel = new ExceptionModel()
            {
                Type = ExceptionModel.ExceptionType.System.ToString(),
                Key = "error",
                Errors = new Dictionary<string, string>() {
                     {
                       actionExecutedContext.Exception.Source,
                       actionExecutedContext.Exception.Message
                     }
                }
            };

            actionExecutedContext.ExceptionHandled = true;
            actionExecutedContext.Result = new BadRequestObjectResult(exceptionModel);
        }
    }
}
