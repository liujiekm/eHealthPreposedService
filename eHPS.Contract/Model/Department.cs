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
// 创建时间：2016/7/8 15:55:00
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
    /// 医院科室模型
    /// </summary>
    public class Department
    {

        /// <summary>
        /// 科室标识
        /// </summary>
        public string DeptId { get; set; }


        /// <summary>
        /// 科室名称
        /// </summary>
        public string DeptName { get; set; }


        /// <summary>
        /// 子科室
        /// </summary>
        public List<Department> Subdivision { get; set; }

        /// <summary>
        /// 父级科室标识
        /// </summary>
        //public string ParentDeptId { get; set; }


        ///// <summary>
        ///// 院区标识
        ///// </summary>
        //public string AreaId { set; get; }
    }
}
