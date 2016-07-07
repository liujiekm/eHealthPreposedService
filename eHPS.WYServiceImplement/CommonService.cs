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

namespace eHPS.WYServiceImplement
{
    /// <summary>
    /// 温附一业务实现，数据库逻辑帮助类
    /// </summary>
    public class CommonService
    {

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
        /// 根据科室标识、获取科室名称
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static string GetDepts(Int32 deptId)
        {
            var depts = new Dictionary<Int32, String>();
            if(CacheProvider.Exist("ehps_Depts"))
            {
                depts = (Dictionary<Int32, String>)CacheProvider.Get("ehps_Depts");
                return depts[deptId];
            }
            else
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    var command = @"SELECT BMID, BMMC FROM XTGL_BMDM WHERE SJBM=1 AND ZTBZ='1'";
                    var result = con.Query(command).ToDictionary(k=>(Int32)k.BMID,v=>(String)v.BMMC);
                    return result[deptId];
                }
            }
        }
    }
}
