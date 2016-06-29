using eHealth.Date.DAL;
using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Service
{
    public class PhoneYYXX
    {
        #region 签到
        /// <summary>
        /// 有卡预约的签到
        /// </summary>
        /// <param name="yyxxid"></param>
        /// <returns></returns>
        public ReturnCode SignInCard(long yyxxid,string brbh)
        {
            dl_yyfz_yyxx dl_yyxx = new dl_yyfz_yyxx();
            //查找m_yyfz_yyxx预约记录
            m_yyfz_yyxx m = dl_yyxx.GetByFZYYID(yyxxid);

            if (m.ZTBZ != "1")
            {
                return new ReturnCode(-3, "该预约当前状态无法签到");
            }
            DateTime swxbsj = DateTime.Now.Date.AddHours(12);
            DateTime xwxbsj = DateTime.Now.Date.AddHours(16.5);
            if (m.YYSJ > swxbsj && DateTime.Now > xwxbsj)
                return new ReturnCode(-4, "医生已下班");
            if (m.YYSJ < swxbsj && DateTime.Now > swxbsj)
                return new ReturnCode(-4, "医生已下班");
            //判断brbh是否为空
            if (string.IsNullOrWhiteSpace(m.BRBH))//无卡预约的签到
            {
                if (string.IsNullOrWhiteSpace(brbh))
                    return new ReturnCode(-1, "该预约记录卡号为空,缺少参数：用户卡号");
                var brxx = new dl_cw_khxx().GetKHXXByBRBH(brbh);
                if (m.BRXM != brxx.BRXM)
                {
                    return new ReturnCode(-4, "该预约信息与签到人员信息不一致");
                }
                //修改ztbz=9 
                m.ZTBZ = "9";
                //update
                if (dl_yyxx.UpdateZTBZ(m.ZTBZ, yyxxid) <= 0)
                {
                    return new ReturnCode(-1, "预约信息更新失败");
                }
                var yspb = new dl_yyfz_pbxx().GetPBByID((long)m.PBID);
                if (yspb == null || yspb.ZTBZ != "1")
                {
                    return new ReturnCode(-1, "该排班已经被取消！");
                }
                //添加brbh信息，ztbz=2//签到
                m.BRBH = brbh;

                m.BRXM = brxx.BRXM;
                m.BRXB = brxx.BRXB;
                m.ZTBZ = "2";
                m.QDSJ = DateTime.Now;
                //插入新的记录
                if (dl_yyxx.Add(m)<=0)
                {
                    return new ReturnCode(-2, "预约信息添加失败");
                }
            }
            else //有卡预约的签到
            {
                //修改ztbz=2//签到
                m.ZTBZ = "2";
                m.QDSJ = DateTime.Now;
                //update
                if (dl_yyxx.UpdateQDBZ(m.ZTBZ, m.QDSJ, yyxxid) <= 0)
                {
                    return new ReturnCode(-2, "预约信息更新失败");
                }
            }
            return new ReturnCode(1, "");
        }
        #endregion

        #region 预约
        /// <summary>
        /// 获取医生排班信息
        /// </summary>
        /// <param name="rykid"></param>
        /// <returns></returns>
        public Dictionary<long, List<m_yyfz_pbxx>> GetDoctorPBXX(long rykid)
        {
            Dictionary<long, List<m_yyfz_pbxx>> list = new Dictionary<long, List<m_yyfz_pbxx>>();
            var pbxx = new dl_yyfz_pbxx().GetByRYKID(rykid,DateTime.Now.Date,DateTime.Now.Date.AddDays(14));
            foreach (var item in pbxx)
            {
                if (list.ContainsKey(item.ZKID.Value))
                {
                    list[item.ZKID.Value].Add(item);
                }
                else
                {
                    List<m_yyfz_pbxx> pb=new List<m_yyfz_pbxx>();
                    pb.Add(item);
                    list.Add(item.ZKID.Value, pb);
                }
            }
            return list;
        }

        /// <summary>
        /// 说明：获取具体预约号及时间点根据排班id。参数：排班id
        /// </summary>
        /// <param name="ls_pbid"></param>
        /// <returns></returns>
        public List<YYSJD> GetDoctorAvailable(long ls_pbid)
        {
            var dl_pb = new dl_yyfz_pbxx();
            List<YYSJD> list = new List<YYSJD>();
            m_yyfz_pbxx m_pb = dl_pb.GetPBByID(ls_pbid);

            if (m_pb.BCLB == "09" && Lenovo.Tool.Util.f_SecondsAfter(m_pb.SBSJ.Value, m_pb.XBSJ.Value) / 3600 > 5)   //全天
            {
                DateTime[] ldt_sbsj = new DateTime[2];
                DateTime[] ldt_xbsj = new DateTime[2];

                ldt_sbsj[0] = m_pb.SBSJ.Value;  //上午上班时间
                string timeAME = "", zd1AME = "";
                if (m_pb.SBSJ.Value.Month > 9)
                {
                    zd1AME = m_pb.SBSJ.Value.Month + "AME";//begin AMEnd
                }
                else
                {
                    zd1AME = "0" + m_pb.SBSJ.Value.Month + "AME";//begin AMEnd
                }
                timeAME = dl_pb.GetSXBSJ(zd1AME).ZTBZ;

                ldt_xbsj[0] = new DateTime(ldt_sbsj[0].Year, ldt_sbsj[0].Month, ldt_sbsj[0].Day, Convert.ToInt32(timeAME.Split(':')[0]), Convert.ToInt32(timeAME.Split(':')[1]), 0);  //上午下班时间

                ldt_xbsj[1] = m_pb.XBSJ.Value;  //下午下班时间

                string timePMB = "", zd1PMB = "";
                if (Convert.ToDateTime(m_pb.SBSJ).Month > 9)
                {
                    zd1PMB = m_pb.SBSJ.Value.Month + "PMB";
                }
                else
                {
                    zd1PMB = "0" + m_pb.SBSJ.Value.Month + "PMB";
                }
                timePMB = dl_pb.GetSXBSJ(zd1PMB).ZTBZ;
                ldt_sbsj[1] = new DateTime(ldt_sbsj[0].Year, ldt_sbsj[0].Month, ldt_sbsj[0].Day, Convert.ToInt32(timePMB.Split(':')[0]), Convert.ToInt32(timePMB.Split(':')[1]), 0);  //下午上班时间

                //计算上下午秒数
                long sjd0 = Lenovo.Tool.Util.f_SecondsAfter(ldt_sbsj[0], ldt_xbsj[0]);
                long sjd1 = Lenovo.Tool.Util.f_SecondsAfter(ldt_sbsj[1], ldt_xbsj[1]);

                //计算上下午专科限号数
                int zkxh0 = Convert.ToInt32(Math.Floor(Convert.ToDouble((Convert.ToDouble(sjd0) / (sjd0 + sjd1)) * m_pb.ZKXH)));
                int zkxh1 = Convert.ToInt32(m_pb.ZKXH) - zkxh0;

                CreateTimePoint(ldt_sbsj[0], ldt_xbsj[0], 1, zkxh0, zkxh0, ref list, Convert.ToInt64(ls_pbid));
                CreateTimePoint(ldt_sbsj[1], ldt_xbsj[1], zkxh0 + 1, zkxh0 + zkxh1, zkxh1, ref list, Convert.ToInt64(ls_pbid));
            }
            else
            {
                CreateTimePoint(Convert.ToDateTime(m_pb.SBSJ), Convert.ToDateTime(m_pb.XBSJ), 1, Convert.ToInt32(m_pb.ZKXH), Convert.ToInt32(m_pb.ZKXH), ref list, Convert.ToInt64(ls_pbid));
            }
            return list.FindAll(d => d.YYSJ > DateTime.Now).OrderBy(d => d.YYXH).ToList();
        }
     
        /// <summary>
        /// 说明：保存预约信息(有brbh登录)。参数：病人编号,排班id,预约时间(格式：2012-10-10 23:59:59),预约序号
        /// </summary>
        /// <param name="ls_brbh"></param>
        /// <param name="ls_pbid"></param>
        /// <param name="ls_yysj"></param>
        /// <param name="ls_yyxh"></param>
        /// <param name="ls_czzid"></param>
        /// <returns></returns>
        public ReturnCode SaveOrderMessage(string ls_brbh, long ls_pbid, DateTime ls_yysj, int yyxh)
        {
            var dl_khxx = new dl_cw_khxx();
            var dl_yyxx = new dl_yyfz_yyxx();
            var dl_yspb = new dl_yyfz_pbxx();
            var dl_yyls = new dl_yyfz_yyls();
            var dl_ddlbn = new dl_xtgl_ddlbn();
            if (ls_brbh == null || ls_brbh == "")
            {
                return new ReturnCode(-1, "未读到健康号，保存失败");
            }
            else
            {
                if (yyxh == 0)
                {
                    return new ReturnCode(-1, "预约序号不能为0");
                }
                if (ls_yysj <= DateTime.Now)
                {
                    return new ReturnCode(-1, "预约的时间小于当前时间");
                }
                if (dl_yyxx.CheckYYCountByBRBH(ls_brbh, ls_pbid).YYXH >= 2)
                {
                    return new ReturnCode(-1, "一个半天限约两个号源");
                }
                if (dl_yyxx.CheckExist1(ls_brbh, ls_pbid).YYXH > 0)
                {
                    return new ReturnCode(-1, "您在此排班已经有预约");
                }
                if (dl_yyxx.CheckYYSJ(ls_pbid, ls_yysj).YYXH > 0)
                {
                    return new ReturnCode(-1, "该时间点已被其他人预约，请返回刷新");
                }
                if (dl_yspb.CheckPB(ls_pbid).PBID <= 0)
                {
                    return new ReturnCode(-1, "该排班已被取消！");
                }

                m_yyfz_yyxx m_xx = new m_yyfz_yyxx(); //构建yyxx model
                m_cw_khxx m_khxx = dl_khxx.GetKHXXByBRBH(ls_brbh);
                m_xx.BRXM = m_khxx.BRXM;
                m_xx.BRBH = m_khxx.BRBH;
                m_xx.BRXB = m_khxx.BRXB;
                m_xx.CSRQ = m_khxx.CSRQ;
                m_xx.BZ = "";
                m_xx.LXDH = string.IsNullOrWhiteSpace(m_khxx.YDDH) ? m_khxx.LXDH : string.IsNullOrWhiteSpace(m_khxx.LXDH) ? m_khxx.YDDH : m_khxx.YDDH + "," + m_khxx.LXDH; //移动电话在前，用于发送短信
                m_xx.SFZ = m_khxx.SFZH;
                m_xx.LXDZ = m_khxx.LXDZ;
                m_xx.YYXH = yyxh;
                m_xx.PDH = m_xx.YYXH;
                m_xx.YYSJ = ls_yysj;
                m_xx.PBID = ls_pbid;

                m_yyfz_pbxx m_pb = dl_yspb.GetPBByID(ls_pbid);
                m_xx.FZZBH = m_pb.FZZBH;
                m_xx.GZDM = m_pb.GZDM;
                m_xx.ZKID = m_pb.ZKID;
                m_xx.YSYHID = m_pb.YSYHID;
                m_xx.YYFS = "8"; //手机预约
                //由表格维护是否自动签到
                string ls_flag = new dl_yyfz_zkyysz().GetSFQD(m_pb.ZKID.Value).SFQD;
                if (ls_flag == "0")
                {
                    m_xx.ZTBZ = "2";
                }
                else
                {
                    m_xx.ZTBZ = "1";
                }
                m_xx.ZLLX = dl_yspb.GetZLLXByPBID(ls_pbid).ZLLX;
                m_xx.GHID = 0;
                m_xx.DJRYID = 19058;
                m_xx.CZZID = 19058;
                m_xx.XGSJ = DateTime.Now;
                m_xx.PBID = m_pb.PBID;
                m_xx.ZBYY = m_pb.ZBYY;
                m_xx.HJLX = "1";
                m_xx.DJSJ = DateTime.Now;


                if (dl_yyxx.Add(m_xx) > 0)
                {
                    m_yyfz_yyls m_yyls = new m_yyfz_yyls();
                    m_yyls.BRBH = m_xx.BRBH;
                    m_yyls.FZYYID = m_xx.FZYYID;
                    m_yyls.YYFSSJ = DateTime.Now;
                    m_yyls.YYJZSJ = m_xx.YYSJ;
                    m_yyls.YSXM = m_pb.YSXM;
                    m_yyls.ZTBZ = "0";
                    dl_yyls.Add(m_yyls);
                    string ls_rtn = "";
                    string ls_dxnr = "";
                    var ddlbn = dl_ddlbn.GetYQMC(m_pb.YQDM);
                    string ls_yq = ddlbn == null ? "" : ddlbn.MC;

                    string ls_zk = "";
                    m_yyfz_fjxx m_fjxx = new dl_yyfz_fjxx().GetInfo((int)m_pb.FJID.Value);
                    if (m_pb.ZBYY != "" && m_pb.ZBYY != null)
                    {
                        ddlbn = dl_ddlbn.GetZBYYByDM(m_pb.ZBYY);
                        ls_zk = ddlbn == null ? "" : ddlbn.MC;
                        var ZBYYDX = new dl_yyfz_hjmb().GetZBMBNR(m_pb.ZBYY);
                        if (ZBYYDX != null)
                        {
                            ls_dxnr = ZBYYDX.MBNR;
                        }
                        else
                        {
                            ls_dxnr = "";
                        }

                    }
                    else
                    {
                        var bmdm = new dl_xtgl_bmdm().GetBMMCByBMID(m_xx.ZKID.Value);
                        ls_zk = bmdm == null ? "" : bmdm.BMMC;
                        var ZKYYDX = new dl_yyfz_hjmb().GetYYMBNR();
                        if (ZKYYDX != null)
                        {
                            ls_dxnr = ZKYYDX.MBNR;
                        }
                        else
                        {
                            ls_dxnr = "";
                        }

                    }
                    ls_dxnr = ls_dxnr.Replace("brxm", m_xx.BRXM).Replace("yyrq", Convert.ToDateTime(ls_yysj).ToString("MM月dd日")).Replace("zk", ls_zk).Replace("zb", ls_zk).Replace("zs", m_fjxx.FJMC).Replace("yyxh", m_xx.YYXH.ToString()).Replace("yysj", Convert.ToDateTime(ls_yysj).ToString("HH时mm分")).Replace("fjwz", m_fjxx.FJWZ).Replace("yq", ls_yq).Replace("(8)", "");


                    //发短信
                    dl_csm cc = new dl_csm(19058, 2291, "手机预约");
                    cc.uf_notify(ls_dxnr, m_xx.LXDH.Split(',')[0], "101", 1, "", ref ls_rtn);
                    return new ReturnCode(1, m_xx.FZYYID.ToString());
                }
                else
                {
                    return new ReturnCode(-1, "保存失败");
                }
            }

        }

        /// <summary>
        /// 说明：保存预约信息(无brbh未登录)。参数：姓名,性别,移动电话,医生姓名,排班id,预约时间(格式：2012-10-10 23:59:59),预约序号
        /// </summary>
        /// <param name="ls_brxm"></param>
        /// <param name="ls_brxb"></param>
        /// <param name="ls_yddh"></param>
        /// <param name="ls_ysxm"></param>
        /// <param name="ls_pbid"></param>
        /// <param name="ls_yysj"></param>
        /// <param name="ls_yyxh"></param>
        /// <returns></returns>
        public ReturnCode OrderWithoutLogin(string ls_brxm, string ls_yddh, long ls_pbid, DateTime ls_yysj, int yyxh)
        {
            try
            {
                if (yyxh == 0)
                {
                    return new ReturnCode(-1, "预约序号不能为0");
                }
                if (ls_yysj <= DateTime.Now)
                {
                    return new ReturnCode(-1, "预约的时间小于当前时间");
                }
                var bl_xx = new dl_yyfz_yyxx();
                var bl_pb = new dl_yyfz_pbxx();
                var dl_khxx = new dl_cw_khxx();
                var dl_yyls = new dl_yyfz_yyls();
                var dl_ddlbn = new dl_xtgl_ddlbn();
                if (bl_xx.CheckYYCountBySJH2(ls_pbid, "%" + ls_yddh + "%").YYXH >= 2)
                {
                    return new ReturnCode(-1, "一个半天限约两个号源");
                }
                if (bl_xx.CheckExist2(ls_brxm, "%" + ls_yddh + "%", ls_pbid).YYXH > 0)
                {
                    return new ReturnCode(-1, "您在此排班已经有预约");
                }
                if (bl_xx.CheckYYSJ(ls_pbid, ls_yysj).YYXH > 0)
                {
                    return new ReturnCode(-1, "该时间点已被其他人预约，请返回刷新");
                }
                if (bl_pb.CheckPB(ls_pbid).PBID <= 0)
                {
                    return new ReturnCode(-1, "该排班已被取消！");
                }
                m_yyfz_yyxx m_xx = new m_yyfz_yyxx(); //构建yyxx model
                m_xx.BRXM = ls_brxm.Trim();
                m_xx.BRBH = "";
                m_xx.BRXB = "";
                m_xx.CSRQ = null;
                m_xx.BZ = null;
                m_xx.LXDH = ls_yddh;
                m_xx.SFZ = null;
                m_xx.LXDZ = "手机预约";
                m_xx.YYXH = yyxh;
                m_xx.YYSJ = ls_yysj;
                m_xx.PBID = ls_pbid;

                m_yyfz_pbxx m_pb = bl_pb.GetPBByID(ls_pbid);
                m_xx.FZZBH = m_pb.FZZBH;
                m_xx.GZDM = m_pb.GZDM;
                m_xx.ZKID = m_pb.ZKID;
                m_xx.YSYHID = m_pb.YSYHID;
                m_xx.YYFS = "8";
                m_xx.ZTBZ = "1";
                m_xx.ZLLX = bl_pb.GetZLLXByPBID(ls_pbid).ZLLX;
                m_xx.GHID = 0;
                m_xx.DJRYID = 19058;
                m_xx.CZZID = 19058;
                m_xx.XGSJ = DateTime.Now;
                m_xx.PBID = m_pb.PBID;
                m_xx.ZBYY = m_pb.ZBYY;
                m_xx.HJLX = "1";
                m_xx.DJSJ = DateTime.Now;

                m_yyfz_yyls m_yyls = new m_yyfz_yyls();
                m_yyls.BRBH = m_xx.BRBH;
                m_yyls.FZYYID = m_xx.FZYYID;
                m_yyls.YYFSSJ = DateTime.Now;
                m_yyls.YYJZSJ = m_xx.YYSJ;
                m_yyls.YSXM = m_pb.YSXM;
                m_yyls.ZTBZ = "0";

                if (bl_xx.CheckYYSJ(ls_pbid, ls_yysj).YYXH > 0)
                {
                    return new ReturnCode(-1, "该时间点已有预约");
                }

                if (bl_xx.Add(m_xx) > 0)
                {
                    string ls_dxnr = "";
                    string ls_rtn = "";
                    var ddlbn = dl_ddlbn.GetYQMC(m_pb.YQDM);
                    string ls_yq = ddlbn == null ? "" : ddlbn.MC;

                    m_yyfz_fjxx m_fjxx = new dl_yyfz_fjxx().GetInfo((int)m_pb.FJID.Value);
                    string ls_zk = "";
                    if (m_pb.ZBYY != "" && m_pb.ZBYY != null)
                    {
                        ddlbn = dl_ddlbn.GetZBYYByDM(m_pb.ZBYY);
                        ls_zk = ddlbn == null ? "" : ddlbn.MC;
                        var ZBYYDX = new dl_yyfz_hjmb().GetZBMBNR(m_pb.ZBYY);
                        ls_dxnr = ZBYYDX == null ? "" : ZBYYDX.MBNR;
                    }
                    else
                    {
                        var bmdm = new dl_xtgl_bmdm().GetBMMCByBMID(m_xx.ZKID.Value);
                        ls_zk = bmdm == null ? "" : bmdm.BMMC;
                        var ZKYYDX = new dl_yyfz_hjmb().GetYYMBNR();
                        ls_dxnr = ZKYYDX == null ? "" : ZKYYDX.MBNR;
                    }
                    ls_dxnr = ls_dxnr.Replace("brxm", m_xx.BRXM).Replace("yyrq", Convert.ToDateTime(ls_yysj).ToString("MM月dd日")).Replace("zk", ls_zk).Replace("zb", ls_zk).Replace("zs", m_fjxx.FJMC).Replace("yyxh", m_xx.YYXH.ToString()).Replace("yysj", Convert.ToDateTime(ls_yysj).ToString("HH时mm分")).Replace("fjwz", m_fjxx.FJWZ).Replace("yq", ls_yq).Replace("(8)", "");

                    dl_csm cc = new dl_csm(19058, 2291, "手机预约");
                    cc.uf_notify(ls_dxnr, ls_yddh, "101", 1, "", ref ls_rtn);
                    return new ReturnCode(1, m_xx.FZYYID.ToString());
                }
                else
                {
                    return new ReturnCode(-1, "保存失败");
                }

            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                throw ex;
            }
        }
        
        /// <summary>
        /// 说明：删除自己的预约。参数：分诊预约id
        /// </summary>
        /// <param name="ls_fzyyid"></param>
        /// <returns></returns>
        public ReturnCode CancelOrder(long ls_fzyyid)
        {
            var dl_xx = new dl_yyfz_yyxx();
            var dl_ls = new dl_yyfz_yyls();
            var dl_ghmx = new dl_cw_ghmx();
            m_yyfz_yyxx m_xx = dl_xx.GetByFZYYID(ls_fzyyid);
            m_xx.XGSJ = DateTime.Now;
            m_xx.CZZID = 19058;
            m_xx.ZTBZ = "9";
            dl_ls.DelYYLS(ls_fzyyid);
            m_cw_ghmx m_ghmx = dl_ghmx.GetByYYID(ls_fzyyid);
            if (m_ghmx==null||m_ghmx.GHID == 0)   //即根据fzyyid没找到gh记录
            {
                if (dl_xx.UpdateZTBZCZZ(m_xx.ZTBZ, m_xx.CZZID, m_xx.XGSJ, ls_fzyyid) > 0)
                {
                    string ysxm="";
                    if(m_xx.YSYHID.HasValue){
                        var ysxx = new dl_r_ryk().GetDoctorByYHID(m_xx.YSYHID.Value);
                        if (ysxx != null)
                            ysxm = ysxx.XM;
                        else if (m_xx.PBID.HasValue)
                        {
                            var pb = new dl_yyfz_pbxx().GetPBByID(m_xx.PBID.Value);
                            if (pb != null)
                                ysxm = pb.YSXM;
                        }
                    }
                    string ls_rtn = "";
                    string ls_dxnr = m_xx.BRXM + ", 您已取消" + m_xx.YYSJ.Value.ToString("yyyy-MM-dd HH:mm:ss") + "," + ysxm + "医师的预约记录。谢谢配合，祝您健康。";
                    dl_csm cc = new dl_csm(19058, 2291, "手机预约");

                    cc.uf_notify(ls_dxnr, m_xx.LXDH.Split(',')[0], "101", 1, "", ref ls_rtn);
                }
                else
                {
                    return new ReturnCode(-1, "删除失败");
                }
            }
            else
            {
                m_ghmx.YYID = 0;  //设置此挂号还未用过
                if (dl_xx.UpdateZTBZCZZ(m_xx.ZTBZ, m_xx.CZZID, m_xx.XGSJ, ls_fzyyid) > 0 &&
                dl_ghmx.UpdateYYIDByGHID(m_ghmx.YYID, m_ghmx.GHID) > 0)
                {
                    string ls_rtn = "";
                    string ls_dxnr = m_xx.BRXM + ", 您的预约已取消。祝您健康。";
                    dl_csm cc = new dl_csm(19058, 2291, "手机预约");

                    cc.uf_notify(ls_dxnr, m_xx.LXDH.Split(',')[0], "101", 1, "", ref ls_rtn);
                }
                else
                {
                    return new ReturnCode(-1, "删除失败");
                }
            }
            return new ReturnCode(1, "删除成功");
        }

        #endregion

        #region 签到小票
        /// <summary>
        /// 获取签到成功后的小票信息
        /// </summary>
        /// <param name="fzyyid"></param>
        /// <returns></returns>
        public SignInInfo GetSignInInfo(long fzyyid)
        {
            var info=new SignInInfo();
            //预约信息
            var yyxx = new dl_yyfz_yyxx().GetByFZYYID(fzyyid);
            if (yyxx == null) return info;
            info.OrderIndex = yyxx.YYXH.HasValue ? yyxx.YYXH.Value : 0;
            info.OrderTime = yyxx.YYSJ;
            info.SignTime = yyxx.QDSJ;
            info.UserName = yyxx.BRXM;
            //科室名称
            var dep=new dl_xtgl_bmdm().GetBMMCByBMID(yyxx.ZKID.HasValue?yyxx.ZKID.Value:0);
            if(dep!=null)
                info.DepName = dep.BMMC;
            //医生名称
            var doc=new dl_r_ryk().GetDoctorByYHID(yyxx.YSYHID.HasValue ? yyxx.YSYHID.Value : 0);
            if(doc!=null)
                info.DoctorName = doc.XM;
            //院区
            var pb = new dl_yyfz_pbxx().GetPBByID(yyxx.PBID.HasValue ? yyxx.PBID.Value : 0);
            if (pb == null) return info;
            info.Area = pb.YQDM=="01"?"老院":"新院";
            //房间信息
            var room = new dl_yyfz_fjxx().GetInfo((int)(pb.FJID.HasValue ? pb.FJID.Value : 0));
            if (room == null) return info;
            info.RoomName = room.FJMC;
            info.RoomSite = room.FJWZ;
            return info;
        }
        #endregion

        #region 私有的
        /// <summary>
        /// 根据排班id和理想预约时间获取预约时间点,没有时间点则返回null
        /// </summary>
        /// <param name="pbid"></param>
        /// <returns></returns>
        private YYSJD GetYYSJAndYYXH(long pbid)
        {
            YYSJD yysjd = new YYSJD();
            var list = GetDoctorAvailable(pbid);
            return list.FirstOrDefault();

            //if (list.Count <= 0)
            //{
            //    return null;
            //} 
            //if (yysj.HasValue)
            //{
            //    //正向查找最近的未预约的时间点
            //    foreach (var item in list)
            //    {
            //        if (yysj.Value <= item.YYSJ)
            //            return item;
            //    }
            //    //正向未查到未预约的时间点，则反向查找最近的未预约的时间点
            //    for (int i = list.Count - 1; i >= 0; i--)
            //    {
            //        if (yysj.Value >= list[i].YYSJ)
            //            return list[i];
            //    }
            //}
            //else  //未选择理想时间，则选择最近未预约的时间点
            //{
            //    return list[0];
            //}
            //return null;
        }

     
        /// <summary>
        /// 计算时间点
        /// </summary>
        /// <param name="sbsj"></param>
        /// <param name="xbsj"></param>
        /// <param name="ksh"></param>
        /// <param name="jsh"></param>
        /// <param name="zkxh"></param>
        /// <param name="list"></param>
        /// <param name="pbid"></param>
        private void CreateTimePoint(DateTime sbsj, DateTime xbsj, int ksh, int jsh, int zkxh, ref List<YYSJD> list, Int64 pbid)
        {

            DateTime ldt_yysj = new DateTime();
            xbsj = xbsj.AddMinutes(-10);
            long jzsc = Convert.ToInt64(Math.Floor((Convert.ToDouble(Lenovo.Tool.Util.f_SecondsAfter(sbsj, xbsj)) / zkxh + 0.5))); //就诊时长

            #region 不显示加号算法
            for (int i = ksh; i <= jsh; i++)
            {

                if (i == ksh)
                {
                    ldt_yysj = sbsj;
                }
                else
                {
                    ldt_yysj = ldt_yysj.AddSeconds(Convert.ToDouble(jzsc));
                }


                //已存在的yysj或yyxh不再插入
                if (ldt_yysj > DateTime.Now && new dl_yyfz_yyxx().CheckSJDExist(ldt_yysj, i, pbid).YYXH == 0)
                {
                    list.Add(new YYSJD(i, ldt_yysj));
                }

            }
            #endregion

        }
        
        #endregion
    }
    public class ReturnCode 
    {
        public int RET_CODE { get; set; }
        public string RET_INFO { get; set; }
        /*
        private string _ret_code;
        private string _ret_info;
        */
        public ReturnCode() { }
        public ReturnCode(int ls_ret_code, string ls_ret_info)
        {
            this.RET_CODE = ls_ret_code;
            this.RET_INFO = ls_ret_info;

        }
    }

    public class YYSJD
    {
        public int YYXH { get; set; }
        public DateTime YYSJ { get; set; }
        public YYSJD() { }
        public YYSJD(int ls_yyxh, DateTime ls_yysj)
        {
            this.YYXH = ls_yyxh;
            this.YYSJ = ls_yysj;
        }
    }
    public class SignInInfo {
        public string DepName { get; set; }
        public string DoctorName { get; set; }
        public string UserName { get; set; }
        public string RoomName { get; set; }
        public string RoomSite { get; set; }
        public string Area { get; set; }
        public int OrderIndex { get; set; }
        public DateTime? SignTime { get; set; }
        public DateTime? OrderTime { get; set; }
    }
}
