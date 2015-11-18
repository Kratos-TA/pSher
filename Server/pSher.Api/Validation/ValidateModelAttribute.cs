namespace PSher.Api.Validation
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using MyTested.WebApi.Common.Extensions;
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

                if (string.IsNullOrEmpty(error))
                {
                    error = string.Format(ErrorMessages.InvalidRequestModel);
                }

                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
        }
    }
}