//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//=================================================================================== 
// 通过Elmah来记录API 的错误日志
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  liu
// 创建时间：2016/4/8 10:56:50
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace eHPS.API.LogAttribute
{
    public class ElmahErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(actionExecutedContext.Exception));

                //Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
            }

            base.OnException(actionExecutedContext);
        }
    }
}