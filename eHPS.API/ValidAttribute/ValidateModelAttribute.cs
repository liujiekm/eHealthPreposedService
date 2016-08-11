//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//=================================================================================== 
// 对API 请求的对象模型进行验证
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  liu
// 创建时间：2016/4/6 17:25:13
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace eHPS.API.ValidAttribute
{
    /// <summary>
    /// 对API 请求的对象模型进行验证
    /// </summary>
    public class ValidateModelAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(!actionContext.ModelState.IsValid)
            {
                var errors = new List<String>();
                foreach (var item in actionContext.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                var errorMsgConent = String.Join(",", errors);
                var response = new ResponseMessage<string> { HasError = 1, ErrorMessage = errorMsgConent };

                actionContext.Response = actionContext.Request.CreateResponse<ResponseMessage<string>>(response);
            }
        }
    }
}