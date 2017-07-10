using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Reborn.Web.Api.Utils.Exception
{

    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.HttpContext.Request.Method.ToLower() != HttpMethod.Post.ToString().ToLower()
                && actionContext.HttpContext.Request.Method.ToLower() != HttpMethod.Put.ToString().ToLower()) return;

            var modelState = actionContext.ModelState;
            var argumentName = GetActionModelArgumentName(actionContext);
            var argumentIsNull = actionContext.ActionArguments.Any(x => argumentName == x.Key && x.Value == null);

            var errorMessage = string.Empty;
            if (argumentIsNull)
                errorMessage = "model_is_not_null";
            else if (!modelState.IsValid)
                errorMessage = "model_validation_exception";

            if (!string.IsNullOrEmpty(errorMessage))
                throw new ValidationException(errorMessage, modelState);
        }

        private static string GetActionModelArgumentName(ActionExecutingContext actionContext)
        {
            return "model";


            //return actionContext.ActionDescriptor.GetParameters()
            //    .Where(x => x.ParameterType.BaseType == typeof(object))
            //    .Select(s => s.ParameterName)
            //    .FirstOrDefault();
        }
    }
}
