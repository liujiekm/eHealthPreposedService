//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基于Unity Policy Injection 的异常日志处理类
// 当方法发生异常时，系统记录错误信息
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace eHPS.CrossCutting.NetFramework.ExceptionHandler
{
    public class LogingExceptionHandler : ICallHandler
    {
        public int Order
        {
            get;
            set;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var methodReturn = getNext()(input, getNext);
            if(methodReturn.Exception!=null)
            {
                LoggerFactory.CreateLog().Error(String.Format(
                  "方法 {0} 抛出异常 {1}\r\n堆栈信息：{2}", methodReturn.Exception.TargetSite.Name,
                  methodReturn.Exception.Message,
                  methodReturn.Exception.StackTrace), methodReturn.Exception);
                return null;
            }

            return methodReturn;

        }
    }
}
