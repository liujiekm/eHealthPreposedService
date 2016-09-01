//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 获取医生可预约情况的请求包装对象
// 
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/24 16:51:43
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHPS.API.Models
{
    /// <summary>
    /// 获取医生可预约情况的请求包装对象
    /// </summary>
    public class BookableInfoRequest
    {
        /// <summary>
        /// 医院院区代码
        /// </summary>
        public String areaId { get; set; }


        /// <summary>
        /// 科室标识
        /// </summary>
        public String deptId { get; set; }

        /// <summary>
        /// 医生标识
        /// </summary>
        public String doctorId { get; set; }


        /// <summary>
        /// 预约排班查询开始时间
        /// </summary>
        public DateTime? startTime { get; set; }


        /// <summary>
        /// 预约排班查询结束时间
        /// </summary>
        public DateTime? endTime { get; set; }


        /// <summary>
        /// 表明是挂号还是预约
        /// Register 挂号
        /// Appointment预约
        /// </summary>
        public String RegisterOrAppointment { get; set; }

    }


   
}