//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Dapper Connection创建工厂
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/30 10:13:57
// 版本号：  V1.0.0.0
//===================================================================================




using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.WYServiceImplement
{
    public class DapperFactory
    {
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["eHPS"].ToString();

        public static OracleConnection CrateOracleConnection()
        {
            var connection = new OracleConnection(connectionString);
            connection.Open();
            return connection;
        }

    }
}
