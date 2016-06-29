using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public class dl_csm
    {
        private long ll_yhid;
        private long ls_shbmid;
        private string ls_yhxm;
        public dl_csm(long YHID, long SHBMID, string YHXM)
        {
            ll_yhid = YHID;
            ls_shbmid = SHBMID;
            ls_yhxm = YHXM;
        }

        public long uf_notify(string as_sm, string as_phone_numbers, string as_xxlb, int li_ztbz, string bz, ref string as_rtn)
        {
            if ((as_phone_numbers == "") || (as_xxlb == "") || (as_phone_numbers.Length < 11))
            {
                return 0; //空就返回掉
            }
            DateTime ldt_null = Convert.ToDateTime("1900-01-01 00:00:00.000");
            return uf_send(as_sm, as_phone_numbers, "0", as_xxlb, ldt_null, li_ztbz, bz, ref as_rtn);
            //return 0;
        }

        public long uf_send(string as_sm, string as_phone_numbers, string as_xxlx, string as_xxlb, DateTime ad_dsfssj, int li_ztbz, string as_bz, ref string as_rtn)
        {
            long temp = -1;
            int ll_cnt = 0;//记录多少条记录
            DateTime ldt_now = DateTime.Now;
            if ((string.IsNullOrEmpty(as_sm)) || (string.IsNullOrEmpty(as_phone_numbers)))
            { return 0; }
            else
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                {
                    conn.Open();
                    System.Data.OracleClient.OracleTransaction myTran = conn.BeginTransaction();
                    var command = conn.CreateCommand();
                    command.Transaction = myTran;

                    string[] phonenum = as_phone_numbers.Split(',');
                    for (int i = 0; i < phonenum.Length; i++)
                    {
                        if (phonenum[i].Length < 11) continue;
                        try
                        {
                            temp = OracleHelper.GetNextValue("seq_xtgl_ism_send_fsid");
                            m_xtgl_ism_sendmx msendmx = new m_xtgl_ism_sendmx();
                            msendmx.FSID = temp;
                            msendmx.FSSJ = ldt_now;
                            msendmx.FSYHID = ll_yhid;
                            msendmx.SJHM = phonenum[i].Trim();
                            msendmx.TXFY = 0.05M;
                            msendmx.CJSJ = ldt_now;
                            msendmx.BZ = as_bz;
                            //var r = new dl_xtgl_ism_sendmx().AddMX(msendmx);//this nr_xtgl_ism_sendmx
                            var fields = new List<string>();
                            command.Parameters.Clear();
                            if (msendmx.FSID != null) fields.Add("FSID"); if (msendmx.FSID != null) command.Parameters.Add(new OracleParameter(":FSID", msendmx.FSID));
                            if (msendmx.FSSJ != null) fields.Add("FSSJ"); if (msendmx.FSSJ != null) command.Parameters.Add(new OracleParameter(":FSSJ", msendmx.FSSJ));
                            if (msendmx.FSYHID != null) fields.Add("FSYHID"); if (msendmx.FSYHID != null) command.Parameters.Add(new OracleParameter(":FSYHID", msendmx.FSYHID));
                            if (msendmx.SJHM != null) fields.Add("SJHM"); if (msendmx.SJHM != null) command.Parameters.Add(new OracleParameter(":SJHM", msendmx.SJHM));
                            if (msendmx.TXFY != null) fields.Add("TXFY"); if (msendmx.TXFY != null) command.Parameters.Add(new OracleParameter(":TXFY", msendmx.TXFY));
                            if (msendmx.CJSJ != null) fields.Add("CJSJ"); if (msendmx.CJSJ != null) command.Parameters.Add(new OracleParameter(":CJSJ", msendmx.CJSJ));
                            if (msendmx.BZ != null) fields.Add("BZ"); if (msendmx.BZ != null) command.Parameters.Add(new OracleParameter(":BZ", msendmx.BZ));
                            command.CommandText = string.Format("insert into xtgl_ism_sendmx({0}) values(:{1})", string.Join(",", fields), string.Join(",:", fields));
                            var r = command.ExecuteNonQuery();
                            if (r == 0)
                            {
                                myTran.Rollback();
                            }
                            else { ll_cnt++; }


                            if (ll_cnt == 0) { return 0; }
                            else
                            {
                                m_xtgl_ism_send msend = new m_xtgl_ism_send();
                                msend.FSID = temp;
                                msend.MBS = ll_cnt;
                                msend.XXLX = as_xxlx;
                                msend.XXLB = as_xxlb;
                                msend.XXNR = as_sm;
                                msend.DSFSSJ = ad_dsfssj;
                                msend.ZTBZ = li_ztbz;
                                msend.BJSJ = ldt_now;
                                msend.BJYHID = ll_yhid;
                                msend.SHSJ = ldt_now;
                                msend.SHBMID = ls_shbmid;
                                msend.SHYHID = ll_yhid;
                                msend.BZ = as_bz;
                                // r = new dl_xtgl_ism_send().AddSend(msend);//this nr_xtgl_ism_send
                                 command.Parameters.Clear();
                                 fields.Clear();
                                 if (msend.FSID != null) fields.Add("FSID"); if (msend.FSID != null) command.Parameters.Add(new OracleParameter(":FSID", msend.FSID));
                                 if (msend.MBS != null) fields.Add("MBS"); if (msend.MBS != null) command.Parameters.Add(new OracleParameter(":MBS", msend.MBS));
                                 if (msend.XXLX != null) fields.Add("XXLX"); if (msend.XXLX != null) command.Parameters.Add(new OracleParameter(":XXLX", msend.XXLX));
                                 if (msend.XXLB != null) fields.Add("XXLB"); if (msend.XXLB != null) command.Parameters.Add(new OracleParameter(":XXLB", msend.XXLB));
                                 if (msend.XXNR != null) fields.Add("XXNR"); if (msend.XXNR != null) command.Parameters.Add(new OracleParameter(":XXNR", msend.XXNR));
                                 if (msend.DSFSSJ != null) fields.Add("DSFSSJ"); if (msend.DSFSSJ != null) command.Parameters.Add(new OracleParameter(":DSFSSJ", msend.DSFSSJ));
                                 if (msend.ZTBZ != null) fields.Add("ZTBZ"); if (msend.ZTBZ != null) command.Parameters.Add(new OracleParameter(":ZTBZ", msend.ZTBZ));
                                 if (msend.BJSJ != null) fields.Add("BJSJ"); if (msend.BJSJ != null) command.Parameters.Add(new OracleParameter(":BJSJ", msend.BJSJ));
                                 if (msend.BJYHID != null) fields.Add("BJYHID"); if (msend.BJYHID != null) command.Parameters.Add(new OracleParameter(":BJYHID", msend.BJYHID));
                                 if (msend.SHSJ != null) fields.Add("SHSJ"); if (msend.SHSJ != null) command.Parameters.Add(new OracleParameter(":SHSJ", msend.SHSJ));
                                 if (msend.SHBMID != null) fields.Add("SHBMID"); if (msend.SHBMID != null) command.Parameters.Add(new OracleParameter(":SHBMID", msend.SHBMID));
                                 if (msend.SHYHID != null) fields.Add("SHYHID"); if (msend.SHYHID != null) command.Parameters.Add(new OracleParameter(":SHYHID", msend.SHYHID));
                                 if (msend.BZ != null) fields.Add("BZ"); if (msend.BZ != null) command.Parameters.Add(new OracleParameter(":BZ", msend.BZ));
                                 command.CommandText = string.Format("insert into xtgl_ism_send({0}) values(:{1})", string.Join(",", fields), string.Join(",:", fields));
                                 r= command.ExecuteNonQuery();
                                if (r == 0)
                                {
                                    myTran.Rollback(); return 0;
                                }

                                as_rtn = ll_cnt.ToString();
                                myTran.Commit();
                                return temp;
                            }
                        }
                        catch (Exception ex)
                        {
                            myTran.Rollback();
                            Lenovo.Tool.Log4NetHelper.Error(ex);
                            return 0;
                        }
                    }
                    return 0;
                }
            }
        }
    }
}
