//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者单次就诊，诊疗活动信息
// 包含 就诊科室、诊断信息等
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/28 15:54:08
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
    /// 患者单次就诊，诊疗活动信息
    /// 包含 就诊科室、诊断信息等
    /// </summary>
    public class TreatmentActivityInfo
    {
        /// <summary>
        /// 诊疗活动标识
        /// </summary>
        public String TreatmentId { get; set; }

        /// <summary>
        /// 本次诊疗活动所有收费项目的金额
        /// </summary>
        public Decimal Amount { get {

                decimal amount = 0.0M;
                foreach (var item in Orders)
                {
                    amount += Math.Round(item.ItemUnitPrice * Convert.ToDecimal(item.ItemCount),2, MidpointRounding.AwayFromZero);
                }
                return amount;

            } }

        /// <summary>
        /// 科室标识
        /// </summary>
        public String DeptdId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String DeptName { get; set; }



        /// <summary>
        /// 医生标识
        /// </summary>
        public String DoctorId { get; set; }



        /// <summary>
        /// 医生姓名
        /// </summary>
        public String DoctorName { get; set; }


        /// <summary>
        /// 主诉
        /// </summary>
        public String Complaint { get; set; }
        
        /// <summary>
        /// 支付诊疗活动项目的过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        
        /// <summary>
        /// 患者诊断信息
        /// </summary>
        public List<Diagnostics> Diagnostics { get; set; }
        
        /// <summary>
        /// 患者订单信息
        /// </summary>
        public List<OrderItem> Orders { get; set; }
    }
} 
