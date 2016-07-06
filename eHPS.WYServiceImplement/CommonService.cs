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
                var command = @"select mc from xtgl_ddlbn where lb = :LB and dm = :DM and ztbz='1'";
                var condition = new { LB=category,DM=code };

                var result = con.Query<MultiValue>(command, condition).FirstOrDefault();

                return result;
            }
        } 
    }
}
