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
    public interface IInspection
    {
        List<InspectionReport> GetInspectionReportByPatientId(string patientId);



        void GetInspectionReportByPatientIds(List<string> patientIds);
    }
}
