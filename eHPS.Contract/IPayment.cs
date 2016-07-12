//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 支付服务接口
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/12 17:41:22
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract
{
    /// <summary>
    /// 支付服务接口：
    /// 包含 挂号收费、HIS系统内部开具的医嘱收费
    /// </summary>
    public interface IPayment
    {

        /// <summary>
        /// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        /// 则本方法实现需主动轮询，未支付的收费项目
        /// </summary>
        /// <param name="patientIds"></param>
        /// <returns></returns>
        List<Order> AwareOrderBooked(List<String> patientIds);

        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalOrderId">医院订单标识</param>
        /// <param name="hospitalId">医院标识</param>
        /// <returns></returns>
        ResponseMessage<String> Pay(List<String> hospitalOrderId, String hospitalId);


        /// <summary>
        /// 挂号收费
        /// 收费成功后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalId">医院标识</param>
        /// <param name="appointId">预约标识</param>
        /// <returns></returns>
        ResponseMessage<String> PayRegistration(String hospitalId, String appointId);


    }
}
