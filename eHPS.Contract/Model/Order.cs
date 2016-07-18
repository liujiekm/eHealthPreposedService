//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者就诊之后，医生在HIS系统中开具的医嘱及其收费项目明细模型
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/12 17:05:51
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{
    /// <summary>
    /// 患者就诊之后，医生在HIS系统中开具的医嘱及其收费项目明细模型s
    /// </summary>
    public class Order
    {





        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }


        /// <summary>
        /// 订单类型
        /// </summary>
        public String OrderType { get; set; }


        /// <summary>
        /// 医院HIS内部标识订单的标识
        /// 为了支持一张处方内的项目可以单独收费
        /// 比如一张化验单则取化验单的唯一标识+前缀
        /// </summary>
        public String HospitalOrderId { get; set; }


        /// <summary>
        /// 订单描述
        /// </summary>
        public String OrderDescribe { get; set; }


        /// <summary>
        /// 订单备注
        /// </summary>
        public String Remark { get; set; }


        /// <summary>
        /// 下订单时间
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 订单过期时间
        /// 如果是挂号订单，则需设置订单过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }


        /// <summary>
        /// 订单状态
        /// </summary>
        public String OrderState { get; set; }


        /// <summary>
        /// 诊疗活动标识
        /// </summary>
        public String TreatmentId { get; set; }


        /// <summary>
        /// 收费项目详情
        /// </summary>
        public List<OrderItem> OrderItems { get; set; }

    }
}
