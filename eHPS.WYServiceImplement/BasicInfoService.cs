//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基础信息服务，温附一HIS系统实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 11:14:54
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

using Dapper;
using eHPS.Contract.Model;

namespace eHPS.WYServiceImplement
{
    public class BasicInfoService : IBasicInfo
    {




        public List<Tuple<string, string, string>> GetDepts()
        {
            var depts = new List<Tuple<string, string, string>>();
            using (OracleConnection con = DapperFactory.CrateOracleConnection())
            {
                var getDepts = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where  cxbz='1' and ztbz='1'";
                var results = con.Query(getDepts);

                foreach (var item in results)
                {
                    var dept = new Tuple<string, string, string>(
                            (string)item.dm,
                            (string)item.fdm,
                            (string)item.mc);
                    depts.Add(dept);
                    
                }
                return depts;

            }
            
        }



        /// <summary>
        /// 根据用户标识获取用户姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public string GetNameById(string userId)
        //{
        //    using (var con = DapperFactory.CrateOracleConnection())
        //    {
        //        var command = @"select yhxm from xtgl_yhxx where yhid =:UserId";

        //        var condition = new { UserId = userId };

        //        var result = con.Query(command, condition).FirstOrDefault();

        //        return (string)result.yhxm;
        //    }
        //}



        public Doctor GetDoctorById(string doctorId)
        {
            var doctor = default(Doctor);
            //using (var con = DapperFactory.CrateOracleConnection())
            //{
            //    var getDoctorById = @"";


            //}

            return doctor;
        }

        public List<Doctor> GetDoctors()
        {
            var result = new List<Doctor>();
            //using (var con = DapperFactory.CrateOracleConnection())
            //{
            //    var getDoctors = @"";

            //}

            return result;
        }
    }
}
