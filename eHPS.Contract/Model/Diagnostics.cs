//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者一次诊疗的诊断信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/18 10:58:58
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
    /// 患者一次诊疗的诊断信息
    /// </summary>
    public class Diagnostics
    {
        public String ICD { get; set; }

        public String DiagnosisName { get; set; }
    }
}
