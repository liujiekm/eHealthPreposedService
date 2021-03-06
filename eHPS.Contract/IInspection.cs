﻿//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者检查检验信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 11:48:46
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract
{
    /// <summary>
    /// 患者检查检验信息
    /// </summary>
    public interface IInspection
    {




        /// <summary>
        /// 根据用户标识获取检查检验报告详情
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <returns>患者详细的报告单信息</returns>
        List<InspectionReportDetail> GetInspectionReportDetailByPatientId(string patientId);
        
    }
}
