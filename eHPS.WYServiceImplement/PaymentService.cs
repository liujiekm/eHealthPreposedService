//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 支付服务温附一实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/13 16:37:12
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;

namespace eHPS.WYServiceImplement
{
    /// <summary>
    /// 支付服务温附一实现
    /// </summary>
    public class PaymentService : IPayment
    {

        /// <summary>
        /// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        /// 则本方法实现需主动轮询，未支付的收费项目
        /// </summary>
        /// <param name="patientIds"></param>
        /// <returns></returns>
        public List<Order> AwareOrderBooked(List<string> patientIds)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalOrderId">医院订单标识
        /// 如果是药品，取药品组号
        /// 如果是检查、检验、治疗 去申请单标识
        /// </param>
        /// <param name="hospitalId">医院标识</param>
        /// <returns></returns>
        public ResponseMessage<string> Pay(List<string> hospitalOrderId, string hospitalId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 挂号收费
        /// 收费成功后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalId">医院标识</param>
        /// <param name="appointId">预约标识</param>
        /// <returns></returns>
        public ResponseMessage<string> PayRegistration(string hospitalId, string appointId)
        {
            throw new NotImplementedException();
        }
    }
}
