//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者就诊时，医生在HIS系统开具的医嘱对应的收费项目详细清单
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/12 17:23:13
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
    /// 患者就诊时，医生在HIS系统开具的医嘱对应的收费项目详细清单
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// 收费项目名称
        /// </summary>
        public String ItemName { get; set; }


        /// <summary>
        /// 收费项目类型
        /// </summary>
        public String  ItemType { get; set; }


        /// <summary>
        /// 收费项目单价
        /// </summary>
        public decimal ItemUnitPrice { get; set; }



        /// <summary>
        /// 收费项目数量
        /// </summary>
        public double ItemCount { get; set; }
    }
}
