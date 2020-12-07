using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Vrnz2.Challenge.Payments.WebApi.CustomResults
{
    public class InternalServerErrorObjectResult
        : ObjectResult
    {
        public InternalServerErrorObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
            : base(modelState)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        public InternalServerErrorObjectResult([ActionResultObjectValue] object error)
            : base(error)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
