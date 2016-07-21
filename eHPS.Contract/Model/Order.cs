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

using Jil;

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
        public decimal OrderAmount {

            get {

                decimal amount = 0;
                foreach (var item in OrderItems)
                {
                    amount += (decimal)item.ItemCount * item.ItemUnitPrice;
                }
                return amount;

            } set {

            } }


        /// <summary>
        /// 订单类型
        /// </summary>

        [JilDirective(TreatEnumerationAs =typeof(Int32))]
        public OrderType OrderType { get; set; }


        /// <summary>
        /// 医院HIS内部标识订单的标识
        /// 为了支持一张处方内的项目可以单独收费
        /// 比如一张化验单则取化验单的唯一标识+前缀(医院标识)
        /// 如果是担保挂号则取诊疗活动标识
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
        [JilDirective(TreatEnumerationAs =typeof(Int32))]
        public OrderState OrderState { get; set; }


        /// <summary>
        /// 收费项目详情
        /// </summary>
        public List<OrderItem> OrderItems { get; set; }

    }


    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 未支付
        /// </summary>
        Nonpayment=0
    }


    public enum OrderType
    {
        /// <summary>
        /// 挂号费
        /// </summary>
        Registration =0,

        /// <summary>
        /// 药品费
        /// </summary>
        Medicine=1,

        /// <summary>
        /// 检查
        /// </summary>
        Inspection=2,

        /// <summary>
        /// 检验
        /// </summary>
        Laboratory=3,
        /// <summary>
        /// 治疗
        /// </summary>
        Cure =4

    }
}
