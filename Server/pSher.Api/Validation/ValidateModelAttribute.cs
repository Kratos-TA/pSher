namespace PSher.Api.Validation
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using PSher.Common.Constants;

    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(p => p.Value == null))
            {
                actionContext.ModelState.AddModelError(string.Empty, ErrorMessages.RequestCannotBeEmpty);
            }

            if (!actionContext.ModelState.IsValid)
            {
                var error = actionContext
                    .ModelState
                    .Values
                    .SelectMany(v => v.Errors.Select(er => er.ErrorMessage))
                    .First();

                actionContext.Response = actionContext.Request.CreateResponse(new ResultObject(false, error));
            }
        }
    }
}