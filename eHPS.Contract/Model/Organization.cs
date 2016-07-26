//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 医院科室模型
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/25 9:50:51
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{
    public class Organization
    {

        /// <summary>
        /// 父级科室
        /// </summary>
        public Department ParentDept { get; set; }

        /// <summary>
        /// 子级科室
        /// </summary>
        public List<Department> Subdivision { get; set; }
    }
}
