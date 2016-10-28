//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 上海曙光医院检查检验报告
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/10/28 13:30:05
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;

namespace eHPS.SGServiceImplement
{
    public class InspectionService : IInspection
    {
        public List<InspectionReportDetail> GetInspectionReportDetailByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
