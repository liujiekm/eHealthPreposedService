//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & Net 开发组
//=================================================================================== 
// 对通过URL Encode 方式传递给API 的参数进行Decode
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  liu
// 创建时间：2016/4/8 10:40:50
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace eHPS.API.Filter
{
    public class UrlDecodeParameterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var parameters = actionContext.ActionArguments.ToDictionary(para => para.Key, para => para.Value);

            actionContext.ActionArguments.Clear();

            foreach (var item in parameters)
            {
                if (item.Key.ToLower().Equals("content"))
                {
                    actionContext.ActionArguments.Add(item.Key, UrlDecode(item.Value.ToString()));
                }
                else
                {
                    actionContext.ActionArguments.Add(item.Key, item.Value);
                }
            }



            base.OnActionExecuting(actionContext);
        }



        public String UrlDecode(String content)
        {
            return HttpUtility.UrlDecode(content);
        }
    }
}