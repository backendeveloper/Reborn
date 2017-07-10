using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace Reborn.Web.Api.Utils.Exception
{
    /// <summary>
    /// exception olarak fırlatılmayan hataları yakalamak için kullanılır (badrequest veya modelstate üzerindeki mesajları yakalar)
    /// controller da modelstate e ekleme yapıldıysa veya badrequest dönüşü yapıldı ise apiexception olarak entity dönüyoruz
    /// </summary>
    public class ErrorActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.ModelState.IsValid)
            {
                var errors = actionExecutedContext.ModelState
                    .Select(s => new ErrorEntity()
                    {
                        Key = s.Key,
                        Value = string.Join("|", s.Value.Errors.Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(c => c.ErrorMessage))
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Value)).ToList();

                var exceptionModel = new
                {
                    Type = BaseException.ExceptionType.Validation.ToString(),
                    Key = "request_is_invalid",
                    Errors = errors
                };

                actionExecutedContext.ExceptionHandled = true;
                actionExecutedContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadGateway;
                actionExecutedContext.Result = new JsonResult(exceptionModel);

                return;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
