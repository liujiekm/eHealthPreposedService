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
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns>
        /// HasError：0 支付成功
        /// HasError：1 交易标识、诊疗活动标识、总金额、实际交易金额不可为空
        /// HasError：2 预存充值失败
        /// HasError：3 充值成功，结算失败，余额存入医院预存账户
        /// </returns>
        [Route("Pay"),HttpPost, ResponseType(typeof(ResponseMessage<String>))]
        public ResponseMessage<String> Pay([FromBody]PayModelRequest payModel)
        {
            return paymentService.Pay(payModel.TradingId,payModel.ActivityId, payModel.Amount,payModel.ActualAmount);
        }




        /// <summary>
        /// 获取指定患者的医院账户可用金额（预存for温附一）
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <returns></returns>
        [Route("GetPatientAvaliableAmount"), HttpPost, ResponseType(typeof(TradingAccount))]
        public TradingAccount GetPatientAvaliableAmount([FromBody]string patientId)
        {
            return paymentService.GetPatientAvaliableAmount(patientId);
        }





        /// <summary>
        /// 挂号收费
        /// </summary>
        /// <param name="request">院区标识以及预约标识</param>
        /// <returns>
        /// HasError :0 充值成功
        /// HasError :1 交易标识、预约不能为空/患者未用就诊卡预约，无法挂号/不存在预约记录
        /// HasError :2 挂号充值失败(ErrorMessage 包含错误信息)
        /// </returns>
        [Route("PayRegistration"), HttpPost, ResponseType(typeof(ResponseMessage<String>))]
        public ResponseMessage<String> PayRegistration([FromBody]PayRegistrationRequest request)
        {
            return paymentService.Recharge(request.TradingId,request.AppointId);
        }





    }
}
