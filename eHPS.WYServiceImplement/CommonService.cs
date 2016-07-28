//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 温附一业务实现，数据库逻辑帮助类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/5 13:24:14
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.WYServiceImplement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using eHPS.CrossCutting.NetFramework.Caching;
using Oracle.ManagedDataAccess.Client;
using eHPS.Contract.Model;

namespace eHPS.WYServiceImplement
{
    /// <summary>
    /// 温附一业务实现，数据库逻辑帮助类
    /// </summary>
    public class CommonService
    {



        /// <summary>
        /// 从附一的webservice中返回的格式字符串解析出Treamtments
        /// </summary>
        /// <param name="content">格式字符串
        /// 示例  $$brbh@@zh^8|mc^药品|@@zh^7|mc^针管|$$
        /// </param>
        /// <returns></returns>
        public static List<Treatment> RetriveFromString(String content)
        {
            var result = new List<Treatment>();
            //分组出患者
            var patients = content.Split(new String[] { "$$" },StringSplitOptions.RemoveEmptyEntries);
            foreach (var patient in patients)
            {
                //分离出患者编号与项目信息
                var patientItem = patient.Split(new String[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                var patientId = patientItem[0];//索引0为病人编号
                for (int i = 1; i < patientItem.Length; i++)//从索引1 开始为具体项目
                {
                    var itemPropertys = patientItem[i].Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var property in itemPropertys)
                    {
                        //property 都是以 ^ 来分割名称与值
                        //var propertyName = 
                    }
                }




                



            }


            return result;
        }




        /// <summary>
        /// 获取多值表值
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static MultiValue GetValue(String category,String code)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT MC FROM XTGL_DDLBN WHERE LB = :LB AND DM = :DM AND ZTBZ='1'";
                var condition = new { LB=category,DM=code };

                var result = con.Query<MultiValue>(command, condition).FirstOrDefault();

                return result;
            }
        } 


        /// <summary>
        /// 获取门诊科室信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Int32, String> GetDepts()
        {

            if (CacheProvider.Exist("ehps_Depts"))
            {
                return (Dictionary<Int32, String>)CacheProvider.Get("ehps_Depts");
            }
            else
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    var command = @"SELECT BMID, BMMC FROM XTGL_BMDM WHERE SJBM=1 AND ZTBZ='1'";
                    var result = con.Query(command).ToDictionary(k => (Int32)k.BMID, v => (String)v.BMMC);
                    CacheProvider.Set("ehps_Depts", result);
                    return result;
                    
                }
            }
                
        }


        /// <summary>
        /// 根据科室标识、获取科室名称
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static string GetDeptName(Int32 deptId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT BMID, BMMC FROM XTGL_BMDM WHERE BMID="+deptId+" AND ZTBZ='1'";
                var result = con.Query(command).FirstOrDefault();
                if(null!=result)
                {
                    return (string)result.BMMC;
                }
                return "";

            }
            
        }

        /// <summary>
        /// 生成序列值
        /// </summary>
        /// <param name="sequenceName">oracle序列名称</param>
        /// <returns></returns>
        public static Int64 GetNextValue(string sequenceName)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                
                var command = string.Format("SELECT {0}.NEXTVAL FROM DUAL", sequenceName);

                var result = con.Query(command).FirstOrDefault();

                return (Int64)result.NEXTVAL;
            }
        }

        /// <summary>
        /// 生成序列值
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        /// <param name="con">oracle连接对象</param>
        /// <returns></returns>
        public static long GetNextValue(string sequenceName,OracleConnection con)
        {
            var command = string.Format("select {0}.nextval from dual", sequenceName);
            var result = con.ExecuteScalar(command);
            return (long)result;
           
        }

        /// <summary>
        /// 获取医生作息时间
        /// </summary>
        /// <param name="indicate"></param>
        /// <returns></returns>
        public static string GetTimetable(string indicate)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select zd2 ZTBZ from yyfz_gxdy where lb='YFSJ' and zd1=:Indicate";
                var condition = new { Indicate=indicate };

                var result = con.Query(command, condition).FirstOrDefault();

                return (string)result.ZTBZ;
            }
        }


        /// <summary>
        /// 根据工种代码获取职级
        /// </summary>
        /// <param name="jobTitleId"></param>
        /// <returns></returns>
        public static  string GetJobTitle(String jobTitleId)
        {
            if (CacheProvider.Exist("ehps_jobTitles"))
            {
                var titles = (Dictionary<String, String>)CacheProvider.Get("ehps_jobTitles");
                return titles[jobTitleId];
            }
            else
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    var jobTitlesCommand = @"select DM,MC from s_gz_zwdm where DM!='0000'";

                    var result = con.Query(jobTitlesCommand).ToDictionary(k => (string)k.DM, v => (string)v.MC);

                    CacheProvider.Set("ehps_jobTitles", result);

                    return result[jobTitleId];
                }
            }
        }


    }
}
