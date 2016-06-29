//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基于Exception Handler Application Block的异常日志处理类
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using eHPS.CrossCutting.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.CrossCutting.NetFramework.ExceptionHandler
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class LoggingHandler : IExceptionHandler
    {
        public LoggingHandler()
        {

        }
        public LoggingHandler(NameValueCollection ignore)
        {
            
        }

        /// <summary>
        /// 主要工作是记录异常信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="handlingInstanceId"></param>
        /// <returns></returns>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            
            LoggerFactory.CreateLog().Error(String.Format(
                  "方法 {0} 抛出异常 {1} 异常编码：{2} \r\n堆栈信息：{3}",exception.TargetSite.Name,
                  exception.Message,
                  handlingInstanceId.ToString(),exception.StackTrace), exception);

            return exception;
        }
    }
}
