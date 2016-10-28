//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 上海曙光医院基础信息服务
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/10/28 13:27:11
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
    public class BasicService : IBasicInfo
    {
        public string GetDeptName(string deptId)
        {
            throw new NotImplementedException();
        }

        public List<Organization> GetDepts(string areaId)
        {
            throw new NotImplementedException();
        }

        public Doctor GetDoctorById(string doctorId)
        {
            throw new NotImplementedException();
        }

        public List<Doctor> GetDoctors(string deptId)
        {
            throw new NotImplementedException();
        }

        public List<Doctor> GetDoctors(string name, string spelling)
        {
            throw new NotImplementedException();
        }

        public Patient GetPatientInfo(string patientId)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetPatientInfoByMobile(string mobile)
        {
            throw new NotImplementedException();
        }

        public decimal GetRegisteredAmount(string diagnosisTypeId, string jobTitleId)
        {
            throw new NotImplementedException();
        }
    }
}
