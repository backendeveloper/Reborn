using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using Reborn.Common.Core.Exceptions;

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
            };

            var exception = actionExecutedContext.Exception;
            if (exception is InvalidModelException)
            {
                exceptionModel.Key = "model_is_invalid";
            }
            else
            {
                exceptionModel.Key = "error";
                exceptionModel.Errors = new Dictionary<string, string>() {
                     {
                       actionExecutedContext.Exception.Source,
                       actionExecutedContext.Exception.Message
                     }
                };
            }

            if (actionExecutedContext.Exception.Data != null)
                exceptionModel.Errors = (Dictionary<string, string>)actionExecutedContext.Exception.Data;

            actionExecutedContext.ExceptionHandled = true;
            actionExecutedContext.Result = new BadRequestObjectResult(exceptionModel);
        }
    }
}
