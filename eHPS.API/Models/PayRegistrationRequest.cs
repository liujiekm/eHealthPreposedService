//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 挂号支付服务的请求对象
// 
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/7 10:10:09
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHPS.API.Models
{
    /// <summary>
    /// 挂号支付服务的请求对象
    /// </summary>
    public class PayRegistrationRequest
    {



        /// <summary>
        /// 预约标识
        /// </summary>
        public String AppointId { get; set; }

        /// <summary>
        /// 交易标识
        /// </summary>

        public String TradingId { get; set; }

        /// <summary>
        /// 实际的充值金额
        /// </summary>
        public decimal Amount { get; set; }


    }
}