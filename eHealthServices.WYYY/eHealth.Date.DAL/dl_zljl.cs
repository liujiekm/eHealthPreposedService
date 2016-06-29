using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public class dl_zljl
    {
        /// <summary>
        /// 获取我的诊疗记录
        /// </summary>
        /// <param name="ls_brbh"></param>
        public IList<m_lszlcx> GetZLJLByBRBH(string ls_brbh)
        {
            dl_yl_zljl lb_zlhd = new dl_yl_zljl();
            dl_e_zyblnr lb_lszlcx = new dl_e_zyblnr();
            IList<m_yl_zljl> list_zlhd = lb_zlhd.GetZLHDByBRBH(ls_brbh);
            IList<m_lszlcx> list = new List<m_lszlcx>();
            foreach (var m in list_zlhd)
            {
                m_lszlcx lm_lszlcx = new m_lszlcx();
                lm_lszlcx.HDID = m.ZLHDID + "";
                lm_lszlcx.KSMC = f_jzzk(m.JZZKID);
                lm_lszlcx.YSXM = f_jzys(m.JZYSYHID);
                lm_lszlcx.ZDSJ = m.KSSJ;
                lm_lszlcx.PTMC = f_zlbt(m.ZLLX, m.SCZLHDID + "");
                lm_lszlcx.ZDJG = f_lczd(m.ZLHDID);

                Int64 li_jlid = lb_zlhd.GetJLID(m.ZLHDID).JLID;
                //病史
                lm_lszlcx.BS = lb_lszlcx.GetBlnrByDM(li_jlid, "010001").JG;
                //体检
                lm_lszlcx.TJ = lb_lszlcx.GetBlnrByDM(li_jlid, "010002").JG;
                //化验数据
                lm_lszlcx.HYTJ = lb_lszlcx.GetBlnrByDM(li_jlid, "010003").JG;
                list.Add(lm_lszlcx);
            }
            return list;

        }
        /// <summary>
        /// 获取诊疗记录通过zlhdid
        /// </summary>
        /// <param name="ls_brbh"></param>
        public m_lszlcx GetZLJLByID(long zlhdid)
        {
            dl_yl_zljl lb_zlhd = new dl_yl_zljl();
            dl_e_zyblnr lb_lszlcx = new dl_e_zyblnr();
            m_yl_zljl m = lb_zlhd.GetZLHDByID(zlhdid);
            m_lszlcx lm_lszlcx = new m_lszlcx();
            lm_lszlcx.HDID = m.ZLHDID + "";
            lm_lszlcx.KSMC = f_jzzk(m.JZZKID);
            lm_lszlcx.YSXM = f_jzys(m.JZYSYHID);
            lm_lszlcx.ZDSJ = m.KSSJ;
            lm_lszlcx.PTMC = f_zlbt(m.ZLLX, m.SCZLHDID + "");
            lm_lszlcx.ZDJG = f_lczd(m.ZLHDID);

            Int64 li_jlid = lb_zlhd.GetJLID(m.ZLHDID).JLID;
            //病史
            lm_lszlcx.BS = lb_lszlcx.GetBlnrByDM(li_jlid, "010001").JG;
            //体检
            lm_lszlcx.TJ = lb_lszlcx.GetBlnrByDM(li_jlid, "010002").JG;
            //化验数据
            lm_lszlcx.HYTJ = lb_lszlcx.GetBlnrByDM(li_jlid, "010003").JG;

            return lm_lszlcx;

        }
        /// <summary>
        /// 诊疗标题
        /// </summary>
        /// <param name="as_zllx"></param>
        /// <returns></returns>
        public string f_zlbt(string as_zllx, string as_sczlhdid)
        {
            dl_xtgl_ddlbn lb_ddlbn = new dl_xtgl_ddlbn();
            string ls_zlmc = lb_ddlbn.GetByLBDM("0051", as_zllx).MC;
            if (string.IsNullOrEmpty(ls_zlmc))
            {
                if (string.IsNullOrEmpty(as_sczlhdid))
                {
                    return "首次诊疗";
                }
                else
                {
                    return "诊疗复查";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(as_sczlhdid))
                {
                    return "首次" + ls_zlmc;
                }
                else
                {
                    return ls_zlmc + "复查";
                }
            }
        }
        /// <summary>
        /// 接诊专科
        /// </summary>
        /// <param name="as_jzzkid"></param>
        /// <returns></returns>
        public string f_jzzk(long as_jzzkid)
        {
            if (as_jzzkid==0)
            {
                return "";
            }
            else
            {
                dl_xtgl_bmdm lb_bmdm = new dl_xtgl_bmdm();
                string ls_zkmc = lb_bmdm.GetBMMCByBMID(as_jzzkid).BMMC;
                if (string.IsNullOrEmpty(ls_zkmc))
                {
                    return "";
                }
                else
                {
                    return ls_zkmc;
                }
            }
        }
        /// <summary>
        /// 接诊医生
        /// </summary>
        /// <param name="as_jzysid"></param>
        /// <returns></returns>
        public string f_jzys(long as_jzysid)
        {
            if (as_jzysid==0)
            {
                return "";
            }
            else
            {
                dl_xtgl_yhxx lb_yhxx = new dl_xtgl_yhxx();
                string ls_yhxm = lb_yhxx.GetYHXMByYHID(as_jzysid).YHXM;
                if (string.IsNullOrEmpty(ls_yhxm))
                {
                    return "";
                }
                else
                {
                    return ls_yhxm;
                }
            }
        }
        /// <summary>
        /// 临床诊断
        /// </summary>
        /// <param name="as_zlhdid"></param>
        /// <returns></returns>
        public string f_lczd(long as_zlhdid)
        {
            string ls_lczd = "";
            if (as_zlhdid != 0)
            {
                dl_yl_zlzd lb_zlzd = new dl_yl_zlzd();
                IList<m_yl_zlzd> list_zlzd = lb_zlzd.GetZKZDByZLHDID(as_zlhdid);
                foreach (m_yl_zlzd lm_zlzd in list_zlzd)
                {
                    string ls_qz = lm_zlzd.QZ;
                    string ls_hz = lm_zlzd.HZ;
                    if (!string.IsNullOrEmpty(ls_qz))
                    {
                        ls_qz = "(" + ls_qz + ")";
                    }
                    if (!string.IsNullOrEmpty(ls_hz))
                    {
                        ls_hz = "(" + ls_hz + ")";
                    }
                    ls_lczd += ls_qz + lm_zlzd.LCZD + ls_hz + ", ";
                }
            }
            return ls_lczd;
        }
    }
    public class m_lszlcx
    {
        public string HDID { get; set; }
        public string KSMC { get; set; }
        public string YSXM { get; set; }
        public DateTime ZDSJ { get; set; }
        public string PTMC { get; set; }
        public string ZDJG { get; set; }
        public string BS { get; set; }
        public string TJ { get; set; }
        public string HYTJ { get; set; }
    }
}
