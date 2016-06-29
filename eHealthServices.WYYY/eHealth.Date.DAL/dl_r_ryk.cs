using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
   
    public partial class dl_r_ryk
    {
        

        public  m_r_ryk GetPICByID2(Int64 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                {
                    //conn.Open();
                    //var command = conn.CreateCommand();
                    //command.CommandText = "insert into tmp_ryk select id,rypic from r_ryk where id=:id";
                    //command.Parameters.Clear();
                    //command.Parameters.Add(new OracleParameter(":id", ID));
                    //command.ExecuteNonQuery();
                    //command.CommandText = "select rypic from tmp_ryk where id=:id";
                    //var result = OracleHelper.GetDataItems<m_r_ryk>(command);
                    //return result.FirstOrDefault();
                    conn.Open();
                    System.Data.OracleClient.OracleTransaction myTran = conn.BeginTransaction();
                    var command = conn.CreateCommand();
                    command.Transaction = myTran;
                    command.CommandText = "insert into tmp_ryk select id,rypic from r_ryk where id=" + ID;
                    //command.Parameters.Clear();
                    //command.Parameters.Add(new OracleParameter(":id", ID));
                    command.ExecuteNonQuery();
                    command.CommandText = "select rypic from tmp_ryk where id=" + ID;
                    var result = OracleHelper.GetDataItems<m_r_ryk>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new m_r_ryk();
            }
        }

    }
}
