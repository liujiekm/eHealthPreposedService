using eHealthServices.WYYY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace eHealthServices.WYYY.App_Start
{
    public class DoExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Lenovo.Tool.Log4NetHelper.Error(context.Exception);
            if (context.Exception is NotImplementedException)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new BaseModel() { HasError = 1, ErrorMessage = context.Exception.Message });

            }
        }
    }
}