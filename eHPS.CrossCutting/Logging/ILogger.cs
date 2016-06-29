//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 日志记录接口
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.CrossCutting.Logging
{
    public interface ILogger
    {
      
        void Debug(string message, params object[] args);

      
        void Debug(string message, Exception exception, params object[] args);

     
        void Debug(object item);

      
        void Fatal(string message, params object[] args);

      
        void Fatal(string message, Exception exception, params object[] args);

       
        void Info(string message, params object[] args);

     
        void Warning(string message, params object[] args);

      
        void Error(string message, params object[] args);

       
        void Error(string message, Exception exception, params object[] args);
    }
}