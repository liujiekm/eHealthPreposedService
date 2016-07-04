//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 具体化验项目详情
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/4 13:42:08
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
    /// 具体化验项目详情
    /// </summary>
    public class LaboratoryItemDetail
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 项目结果值
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 结果值与正常值比较
        /// </summary>
        public string IsError { get; set; }
        /// <summary>
        /// 正常值
        /// </summary>
        public string Normal { get; set; }
    }
}
