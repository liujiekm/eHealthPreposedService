//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 预约服务，温附一HIS系统实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 11:17:07
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;
using Dapper;
using eHPS.CrossCutting.NetFramework.Caching;

namespace eHPS.WYServiceImplement
{
    public class AppointmentService : IAppointment
    {
        public void CancelTheAppointment(string apponintId)
        {
            throw new NotImplementedException();
        }

        public List<BookHistory> GetAppointmentHistory(string patientId)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 获取医生可预约信息
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public List<BookableDoctor> GetBookableInfo(string doctorId)
        {
            throw new NotImplementedException();
        }




        public void MakeAnAppointment(MakeAnAppointment appointment)
        {
            throw new NotImplementedException();
        }

        public List<BookableDoctor> PushBookableDoctors()
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosisType">诊疗类型</param>
        /// <param name="jobTitle">挂牌工种</param>
        /// <param name="itemType">挂号相关 收费项目类型</param>
        private void GetRegisteredAmount(string diagnosisType,string jobTitle,string itemType)
        {
            Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>> auxiliaryData;
            if (CacheProvider.Exist("ehps_auxiliaryData"))
            {
                auxiliaryData = (Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>>)CacheProvider.Get("ehps_auxiliaryData");
            }
            else
            {
                auxiliaryData = CacheAuxiliaryData();
            }

            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"";
            }






        }


        /// <summary>
        /// 缓存获取挂号费用的辅助键值对信息
        /// 诊疗类型、工种代码，收费项目
        /// </summary>
        /// <returns></returns>
        private Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>> CacheAuxiliaryData()
        {
            //获取诊疗类型键值对
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var diagnosisTypesCommand = @"select DM,MC from  xtgl_ddlbn where lb='0051'";
                var jobTitlesCommand = @"select DM,MC from  xtgl_ddlbn where lb='0022'";
                var itemTypesCommand = @"select XMID,MC from cw_sfxm where xmid in (select xmid from  cw_zhxmmx where zhid=12998)";

                var diagnosisTypes = con.Query(diagnosisTypesCommand).ToDictionary(k => (string)k.DM, v => (string)v.MC);
                var jobTitles = con.Query(jobTitlesCommand).ToDictionary(k => (string)k.DM, v => (string)v.MC);
                var itemTypes = con.Query(itemTypesCommand).ToDictionary(k => (string)k.XMID, v => (string)v.MC);

                var auxiliaryData = new Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>>(diagnosisTypes, jobTitles, itemTypes);
                //插入缓存
                CacheProvider.Set("ehps_auxiliaryData", auxiliaryData);

                return auxiliaryData;
            }
        }
    }
}
