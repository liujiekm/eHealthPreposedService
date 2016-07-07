﻿//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者预约历史
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/6 14:26:21
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
    /// 患者预约历史
    /// </summary>
    public class BookHistory
    {


        /// <summary>
        /// 排班标识
        /// </summary>
        public String ArrangeId { get; set; }


        /// <summary>
        /// 预约标识
        /// </summary>
        public String BookId { get; set; }




    }
}
