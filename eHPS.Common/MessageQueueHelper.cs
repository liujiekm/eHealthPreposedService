//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 消息队列操作控制帮助类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/27 15:35:15
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

using Jil;

namespace eHPS.Common
{
    /// <summary>
    /// 消息队列操作控制帮助类
    /// </summary>
    public class MessageQueueHelper
    {

        private static readonly string  address= ConfigurationManager.AppSettings["eHPS_Sys_Address"];
        private static readonly Int32 port = Int32.Parse(ConfigurationManager.AppSettings["eHPS_Sys_Port"]);
        


        /// <summary>
        /// 返回值 0失败；1成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 PushMessage<T>(String queueName,T message)
        {
            //调用消息队列服务，把treatments推送到消息队列
            using (TTransport transport = new TSocket(address, port))
            {
                TProtocol protocol = new TCompactProtocol(transport);
                CacheMQService.Calculator.Client client = new CacheMQService.Calculator.Client(protocol);
                transport.Open();
                var result = client.serverSendMsg(queueName, JSON.Serialize<T>(message));
                return result;

            }
        }


    }
}
