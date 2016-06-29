//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 日志操作类，基于NLOG的实现
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
using eHPS.CrossCutting.Logging;
using NLog;

namespace eHPS.CrossCutting.NetFramework.Logging
{
    public sealed class eHPSNLog : CrossCutting.Logging.ILogger
    {
        private readonly Logger _logger;


        public eHPSNLog()
        {
            _logger = LogManager.GetCurrentClassLogger();
            
        }

        public void Debug(object item)
        {
            _logger.Debug(item);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            _logger.Debug(exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            _logger.Error(exception, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            _logger.Fatal(exception, message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }
    }
}
