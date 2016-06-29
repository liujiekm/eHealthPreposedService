using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public class dl_e_zyblnr
    {

        public me_zyblnr GetBlnrByDM(Int64 blid, string dm)
        {
            m_e_zybldm lm_dm = new m_e_zybldm();
            dl_e_zybldm ldl_dm = new dl_e_zybldm();
            lm_dm = ldl_dm.GetNrb(dm);
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select DM,JG from " + lm_dm.NRB + " where dm='" + dm + "' and id=" + blid.ToString();
                    var result = OracleHelper.GetDataItems<me_zyblnr>(command).FirstOrDefault();
                    return result == null ? new me_zyblnr() : result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new me_zyblnr();
            }
           

        }
    }
    public class me_zyblnr
    {
        public string DM{ get;set; }
        public string JG{ get;set; }
    }
}
