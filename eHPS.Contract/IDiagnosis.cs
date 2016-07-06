//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 诊疗服务契约
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/5 10:27:59
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
    public interface IDiagnosis
    {

        /// <summary>
        /// 根据用户标识，获取用户诊疗记录
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        List<DiagnosisRecord> GetDiagnosisRecord(String patientId);



        


        
    }
}
