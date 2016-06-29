using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public partial class dl_yyfz_zkfj_wh
    {
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetAll()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where  cxbz='1' and ztbz='1'";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }


        /// <summary>
        /// 根据代码获取部门ID
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public IList<m_yyfz_zkfj_wh> GetBMIDListByDM(string dm)
        {
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "select dm,fdm,mc,bmid,cxbz from yyfz_zkfj_wh where DM='" + dm + "' and cxbz='1' and ztbz='1'";
                IList<m_yyfz_zkfj_wh> list_m_ddlbn = OracleHelper.GetDataItems<m_yyfz_zkfj_wh>(command);
                if (list_m_ddlbn.Count == 0) return list_m_ddlbn;
                m_yyfz_zkfj_wh m = list_m_ddlbn.FirstOrDefault();
                if (m != null && !m.BMID.HasValue)
                {
                    command.CommandText = "select dm,fdm,mc,bmid,cxbz from yyfz_zkfj_wh where FDM='" + dm + "'  and ztbz='1'";
                    list_m_ddlbn = OracleHelper.GetDataItems<m_yyfz_zkfj_wh>(command);
                    //if (list_m_ddlbn.Count == 0) return null;
                }
                return list_m_ddlbn;
            }

        }
        /// <summary>
        /// 获取专科ID和名称根据bmid
        /// </summary>
        /// <param name="bmid"></param>
        /// <returns></returns>
        public string  GetMCByBMID(int bmid)
        {
            var mks = GetMCByID(bmid);
            if (mks != null && mks.CXBZ == "0")
                mks = GetByFDM(mks.FDM).FirstOrDefault();
            if (mks != null)
                return mks.MC ;
            return "";
        }
    }
}
