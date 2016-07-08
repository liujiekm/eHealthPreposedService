//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 医生信息模型
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/30 17:53:11
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
    /// 医生信息模型
    /// </summary>
    public class Doctor
    {

        /// <summary>
        /// 医生标识
        /// </summary>
        public string DoctorId { get; set; }


        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        ///// <summary>
        ///// 医院标识
        ///// </summary>
        //public string HospitalId { get; set; }
       

        ///// <summary>
        ///// 医院名称
        ///// </summary>
        //public string HospitalName { get; set; }
        /// <summary>
        /// 科室名
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 科室标识
        /// </summary>
        public string DeptId { get; set; }

        /// <summary>
        /// 职称 主任医师 等
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public string Expert { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        //public string PhoneNum { get; set; }
    }
}
