//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 支付服务，包含：
// 医嘱收费，挂号收费
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/24 16:51:43
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.API.Models;
using eHPS.Contract;
using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace eHPS.API.Controllers
{
    /// <summary>
    /// 支付服务，包含：
    /// 医嘱收费，挂号收费
    /// </summary>
    [RoutePrefix("Payment"),Authorize]
    public class PaymentController : ApiController
    {

        private IPayment paymentService;

        public PaymentController(IPayment paymentService)
        {
            this.paymentService = paymentService;
        }




        ///// <summary>
        ///// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        ///// 则本方法实现需主动轮询，未支付的收费项目
        ///// </summary>
        ///// <param name="patientIds"></param>
        ///// <returns></returns>
        //public List<Order> AwareOrderBooked(List<String> patientIds)
        //{
        //    return paymentService.AwareOrderBooked(patientIds);
        //}

        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns></returns>
        [Route("Pay"),HttpPost, ResponseType(typeof(ResponseMessage<String>))]
        public ResponseMessage<String> Pay(PayModelRequest payModel)
        {
            return paymentService.Pay(payModel.ActivityId, payModel.Amount);
        }


        /// <summary>
        /// 挂号收费
        /// 收费成功后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalId">医院标识</param>
        /// <param name="appointId">预约标识</param>
        /// <returns></returns>
        [Route("PayRegistration"), HttpPost, ResponseType(typeof(ResponseMessage<String>))]
        public ResponseMessage<String> PayRegistration(String hospitalId, String appointId)
        {
            return paymentService.PayRegistration(hospitalId, appointId);
        }

    }
}
