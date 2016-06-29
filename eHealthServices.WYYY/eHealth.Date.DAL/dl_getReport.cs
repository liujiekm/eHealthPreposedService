using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public class dl_getreport
    {

        #region

        ///// <summary>
        ///// 根据诊疗活动id返回特检信息
        ///// </summary>
        ///// <param name="zlhldi"></param>
        ///// <returns></returns>
        //public List<> GetTJByZLHDID(long zlhldi)
        //{ 

        //}







        #endregion

        /// <summary>
        /// 获取所有的检查、报告列表
        /// </summary>
        /// <param name="BRBH"></param>
        /// <returns></returns>
        public List<m_br_jctx> GetBGList(string BRBH)
        {
            List<m_br_jctx> models = new dl_br_jctx().GetBGList(BRBH).ToList();
            models.ForEach(a =>
            {
                if (a.JCLX1 == "JY")
                {
                    a.JCLX = "化验";

                    using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                    {
                        conn.Open();
                        var command = conn.CreateCommand();
                        command.CommandText = "select examinaim from L_PATIENTINFO where sampleno='" + a.BGBS + "'";
                        var temp = OracleHelper.GetDataItems<m_examinaim>(command).FirstOrDefault();
                        a.JYMC = temp == null ? "检验" : temp.EXAMINAIM;
                    }
                }
                else
                {
                    a.JYMC = a.JCLX;
                    a.JCLX = "特检";
                }
            });
            return models.OrderByDescending(a => a.BGSJ).ToList();
        }



        ///<summary>
        ///根据传值获取查询语句
        ///</summary>
        /// <returns></returns>
        private string Get_sqlselect(string bglx, string wh_bgbs, string xgxx)
        {
            string ls_sql = "";
            string sjks = "", jgsj = "", jgzd = "";
            if (bglx == "BLSZ")
            {
                sjks = xgxx.Split('^')[0];
                jgsj = xgxx.Split('^')[1];
                jgzd = xgxx.Split('^')[2];
            }
            switch (bglx)
            {
                case "CTBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                                nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk,'' as sqysid,' ' as sqys,t1.jcbw,
                                '层厚<T>' || t1.ch || '<T>' || 'mm<BR/>' || '层间隔<T>' || t1.cjg || '<T>' || 'mm<BR/>' as smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                                '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'" + xgxx + "' as shry ,t1.dycs from f_ctbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + "  order by t1.bgsj";
                    break;
                case "PFBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,'' as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_pfbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " order by t1.bgsj";
                    break;
                case "MRBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_mribg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " order by t1.bgsj";
                    break;
                case "BCBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,'' as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,t1.jgsj,t1.jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_bcbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " order by t1.bgsj";
                    break;
                case "NJBG":
                    ls_sql = @"select t1.ACCENUM as jch,to_char(t1.bh) as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,'' as tjbh,
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,'' as jgsj,t1.jgzd as jgzd,'' as bgsj,
                              '' as bgryid,t1.czz as bgry,'' as shsj,'' as zhshysid,'' as shry,'' as dycs
                              from f_njbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " order by bgsj";
                    break;
                case "XCBG":
                    //                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                    //                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jcnr as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,'' as jcbw,
                    //                              '主动脉根部内径(mm):' || t1.aord || ',左室舒张末期内径(mm):' || t1.lvdd || ',左室收缩末期内径(mm):' || t1.lvds ||
                    //                               ',左房内径(mm):' || t1.lad || ',室间隔厚度(mm):' || t1.ivst || ',左室后壁厚度(mm):' || t1.pwt || ',主肺动脉(mm):'
                    //                                || t1.mpad || ',左室舒张末期容量(mL):' || t1.edv || ',左室收缩末期容量(mL):' || t1.esv || ',每搏量(mL/B):' ||
                    //                                 t1.sv || ',心输出量(L/min):' || t1.co || ',心脏指数(L/min/m):' || t1.ci || ',室内径缩短分数(%):' || t1.fs ||
                    //                                  ',左室射血分数(%):' || t1.ef  as smcs,t1.jgsj,jgzd,t1.bgsj,
                    //                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                    //                              from f_xcbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " ";

                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jcnr as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,'' as jcbw,
                              '主动脉根部内径<T>' || t1.aord || '<T>' || 'mm<BR/>' || '左室舒张末期内径<T>' || t1.lvdd || '<T>' || 'mm<BR/>' || '左室收缩末期内径<T>' || t1.lvds || '<T>' || 'mm<BR/>' ||
                               '左房内径<T>' || t1.lad || '<T>' || 'mm<BR/>' || '室间隔厚度<T>' || t1.ivst || '<T>' || 'mm<BR/>' || '左室后壁厚度<T>' || t1.pwt || '<T>' || 'mm<BR/>' || '主肺动脉<T>'
                                || t1.mpad || '<T>' || 'mm<BR/>' || '左室舒张末期容量<T>' || t1.edv || '<T>' || 'mL<BR/>' || '左室收缩末期容量<T>' || t1.esv || '<T>' || 'mL<BR/>' || '每搏量<T>' ||
                                 t1.sv || '<T>' || 'mL/B<BR/>' || '心输出量<T>' || t1.co || '<T>' || 'L/min<BR/>' || '心脏指数<T>' || t1.ci || '<T>' || 'L/min/m<BR/>' || '室内径缩短分数<T>' || t1.fs || '<T>' || '%<BR/>' ||
                                  '左室射血分数<T>' || t1.ef || '<T>' || '%<BR/>'  as smcs,t1.jgsj,t1.jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_xcbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + wh_bgbs + " order by t1.bgsj";
                    break;
                case "BLSZ":
                    //                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,'' as tjbh,
                    //                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,'' as sqrq,'' as jclx,t2.sqzkid,'' as sqks ,'' as sqysid,' ' as sqys,'' as jcbw,
                    //                              '' as smcs,t1.jgsj,jgzd,t1.bgsj,
                    //                              '' as bgryid,t1.czz as bgry,'' as shsj,'' as zhshysid,'' as shry,'' as dycs
                    //                              from f_blbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) "+wh_bgbs+" ";
                    //                    ls_sql = @"select t1.jch,t1.brbh,t1.brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,' ' as tjbh,
                    //                              nvl(t1.zyh,' ') as zyh,' ' as bqid,nvl(t1.ch,' ') as cwh,t1.lxdz,t1.jsrq as sqrq,'BL' as jclx,t1.sjks as sqzkid,' ' as sqks ,'' as sqysid,' ' as sqys,' ' as jcbw,
                    //                              ' ' as smcs,' '  as jgsj,t1.lczd as jgzd,t1.bgrq as bgsj,
                    //                              '' as bgryid,' ' as bgry,t1.shrq as shsj,'' as zhshysid,' ' as shry,'' as dycs
                    //                              from bl_sjb t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) and t1.bgzt='7' " + wh_bgbs + " ";

                    ls_sql = @"select t1.jch,t1.brbh,t1.brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,' ' as tjbh,
                              nvl(t1.zyh,' ') as zyh,' ' as bqid,nvl(t1.ch,' ') as cwh,t1.lxdz,t1.jsrq as sqrq,'BL' as jclx,t1.sjks as sqzkid,'" + sjks + "' as sqks ,'' as sqysid,' ' as sqys,' ' as jcbw,' ' as smcs,'" + jgsj + "'  as jgsj,'" + jgzd + "' as jgzd,t1.bgrq as bgsj,'' as bgryid,' ' as bgry,t1.shrq as shsj,'' as zhshysid,' ' as shry,'' as dycs from bl_sjb t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) and t1.bgzt='7' " + wh_bgbs + " order by t1.shrq";
                    break;
                case "PET":
                    ls_sql = @"select t1.jch ,t1.brbh,t1.brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,' ' as tjbh,
                              nvl(t1.zyh,' '),' ' as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,' ' as jclx,t2.sqzkid,' ' as sqks ,'' as sqysid,' ' as sqys,t1.jcbw,
                              ' ' as smcs,' ' as jgsj,' ' as jgzd,t1.bgsj,
                              '' as bgryid,' ' as bgry,t1.shsj,'' as zhshysid,' ' as shry,'' as dycs
                              from fs_petbg t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) " + wh_bgbs + "  order by t1.bgsj";
                    break;
                case "JY":
                    ls_sql = @"SELECT b.sampleno,c.patientid,c.patientname brxm,decode(c.sex,'1','男','女') as brxb,c.birthday,'',
                            c.patientid,'','','',null sqrq,null jclx,null sqzkid,' ' as sqks,null as sqysid,' ' as sqys,examinaim as jcbw,
                   '' smcs,CHINESENAME||'&'|| b.TESTRESULT||'&'|| HINT||'&'||lpad(reflo,7,' ')||decode(nvl(length(refhi),0),0,'','~')||rpad(nvl(refhi,' '),decode(nvl(length(refhi),0),0,5 - length(reflo),7),' ') jgsj,' ' as jgzd,executetime bgsj,
                            null as bgryid,' ' as bgry,checktime,'','','' as dycs from L_TESTDESCRIBE a,L_TESTRESULT b,l_patientinfo c
                    WHERE a.TESTID = b.TESTID   AND    b.ISPRINT = 1 and b.Teststatus >=4 
                    and b.sampleno=c.sampleno  " + wh_bgbs;
                    break;
                case "WZDG":
                    ls_sql = @"select t1.jch,t1.brbh,t1.brxm,decode(t1.brxb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),' ' as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,' ' as sqks ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '层厚<T>' || t1.ch || '<T>' || 'mm<BR/>' || '层间隔<T>' || t1.cjg || '<T>' || 'mm<BR/>' as smcs,t1.jgsj,' ' as jgzd,t1.bgsj,
                              '' as bgryid,' ' as bgry,t1.shsj,'' as zhshysid,' ' as shry,'' as dycs
                              from fs_jrbg t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) " + wh_bgbs + " order by t1.shsj";
                    break;
                case "GMD":
                    break;
                case "JYTW":
                    break;
                default:
                    break;
            }
            return ls_sql;
        }


        /// <summary>
        /// 根据报告类型获取case下的类型(CTBG^PFBG^MRBG^BCBG^NJBG^XCBG^BLSZ^PET^JYBG^WZDG)
        /// </summary>
        /// <returns></returns>
        private string Get_bg(string ll_bglx)
        {
            string bg = "";
            if (ll_bglx == "CT")
                bg = "CTBG";
            else if (ll_bglx == "RF" || ll_bglx == "CR" || ll_bglx == "DR")
                bg = "PFBG";
            else if (ll_bglx == "MR")
                bg = "MRBG";
            else if (ll_bglx == "BC")
                bg = "BCBG";
            else if (ll_bglx == "XC" || ll_bglx == "GQ" || ll_bglx == "ER" || ll_bglx == "DJ" || ll_bglx == "CS" || ll_bglx == "FD" || ll_bglx == "WJ" || ll_bglx == "ZJ" || ll_bglx == "PG" || ll_bglx == "CJ" || ll_bglx == "NJ" || ll_bglx == "BJ" || ll_bglx == "XJ")
                bg = "NJBG";
            else if (ll_bglx == "CU")
                bg = "XCBG";
            else if (ll_bglx == "BL")
                bg = "BLSZ";
            else if (ll_bglx == "PT")
                bg = "PET";
            else if (ll_bglx == "XA")
                bg = "WZDG";
            else if (ll_bglx == "JY")
                bg = "JYBG";
            return bg;
        }


        ///<summary>
        ///根据检查号获取相关信息
        ///</summary>
        ///<pattern a>
        ///审核人员
        ///</pattern a>
        ///<pattern b>
        ///sqzk^shry--申请专科^审核人员
        ///</pattern>
        private string get_xgxx(string ll_jch, string bglx)
        {
            string str_xx = "";
            long zhshysid;
            string shry = " ";
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                dl_xtgl_yhxx lb_xtgl_yhxx = new dl_xtgl_yhxx();//this nr_xtgl_yhxx
                switch (bglx)
                {
                    case "CTBG":
                        command.CommandText = "select zhshysid from f_ctbg where accenum='" + ll_jch + "'";
                        var temp = OracleHelper.GetDataItems<m_zhshysid>(command).FirstOrDefault();
                        zhshysid = temp == null ? 0 : temp.zhshysid;
                        if (zhshysid != 0 && zhshysid != null)
                            shry = lb_xtgl_yhxx.GetYHXMByYHID(zhshysid).YHXM;
                        str_xx = shry;
                        break;
                    case "PFBG":
                        command.CommandText = "select zhshysid from f_pfbg where accenum='" + ll_jch + "'";
                        temp = OracleHelper.GetDataItems<m_zhshysid>(command).FirstOrDefault();
                        zhshysid = temp == null ? 0 : temp.zhshysid;
                        if (zhshysid != 0 && zhshysid != null)
                            shry = lb_xtgl_yhxx.GetYHXMByYHID(zhshysid).YHXM;
                        str_xx = shry;
                        break;
                    case "MRBG":
                        command.CommandText = "select zhshysid from f_mribg where accenum='" + ll_jch + "'";
                        temp = OracleHelper.GetDataItems<m_zhshysid>(command).FirstOrDefault();
                        zhshysid = temp == null ? 0 : temp.zhshysid;
                        if (zhshysid != 0 && zhshysid != null)
                            shry = lb_xtgl_yhxx.GetYHXMByYHID(zhshysid).YHXM;
                        str_xx = shry;
                        break;
                    case "BCBG":
                        //dt = CDatabase.SetDataTable("select zhshysid from f_bcbg where accenum='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "NJBG":
                        //dt =CDatabase.SetDataTable("select zhshysid from f_njbg where accenum='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "XCBG":
                        //dt =CDatabase.SetDataTable("select zhshysid from f_xcbg where accenum='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "BLSZ":
                        //dt =CDatabase.SetDataTable("select zhshysid from f_blbg where accenum='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)    
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "PET":
                        //dt =CDatabase.SetDataTable("select zhshysid from fs_petbg where zyh='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "JYBG":
                        break;
                    case "WZDG":
                        //dt =CDatabase.SetDataTable("select zhshysid from fs_jrbg where zyh='" + ll_jch + "'");
                        //if (dt.Rows.Count > 0)
                        //    zhshysid = dt.Rows[0][0].ToString();
                        //if (!string.IsNullOrEmpty(zhshysid))
                        //    shry = lb_xtgl_yhxx.GetXtgl_yhxx(Convert.ToInt64(zhshysid)).YHXM;
                        str_xx = shry;
                        break;
                    case "GMD":
                        break;
                    case "JYTW":
                        break;
                    default:
                        break;
                }
            }
            return str_xx;
        }


        ///<summary>
        ///根据检查号获取相关信息  sjks^jgsj^jgzd
        ///</summary>
        ///<tip>
        ///bl_xbxbg/bl_fishbg:有结果所见
        ///</tip>
        private string get_xgxx_BLSZ(string ll_jch)
        {
            StringBuilder xgxx = new StringBuilder();
            string blk = "", blh = "";
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                var lb_xtgl_bmdm = new dl_xtgl_bmdm();//this nr_xtgl_bmdm
                command.CommandText = "select sjks,blk,blh from bl_sjb where jch='" + ll_jch + "'";
                var temp = OracleHelper.GetDataItems<m_bl_sjb>(command).FirstOrDefault();
                if (temp != null)
                {
                    if (temp.sjks != null && temp.sjks != 0)
                    {
                        xgxx.Append(lb_xtgl_bmdm.GetBMMCByBMID(temp.sjks).BMMC);
                    }
                    else
                        xgxx.Append(" ");
                    xgxx.Append("^");
                    blk = temp.blk;
                    blh = temp.blh;
                }
                switch (blk)
                {
                    case "020301":
                        command.CommandText = "select jxsj,blzd from bl_blbg where blh='" + blh + "'";
                        var temp_blbg = OracleHelper.GetDataItems<m_bl_blbg>(command).FirstOrDefault();
                        if (!string.IsNullOrEmpty(temp_blbg.jxsj))
                            xgxx.Append(temp_blbg.jxsj);
                        else
                            xgxx.Append(" ");
                        xgxx.Append("^");
                        xgxx.Append(temp_blbg.blzd);
                        break;
                    case "020302":
                        string myd = "", xbl = "", hsxb = "", jgxb = "", gnmxb = "", yzfy = "", dcgr = "", mjgr = "", hpvgr = "", pzgr = "", qtgr = "";
                        command.CommandText = "select ' ' as jxsj,nvl(xbxzd,' ') as xbxzd,nvl(myd,' ') as myd,nvl(xbl,' ') as xbl,nvl(hsxb,' ') as hsxb,nvl(jgxb,' ') as jgxb,nvl(gnmxb,' ') as gnmxb,nvl(yzfy,' ') as yzfy,nvl(dcgr,' ') as dcgr,nvl(mjgr,' ') as mjgr,nvl(hpvgr,' ') as hpvgr,nvl(pzgr,' ') as pzgr,nvl(qtgr,' ') as qtgr from bl_xbxbg where blh='" + blh + "'";
                        temp_blbg = OracleHelper.GetDataItems<m_bl_blbg>(command).FirstOrDefault();
                        if (temp_blbg != null)
                        {
                            if (temp_blbg.myd == "0")
                                myd = "不满意";
                            else if (temp_blbg.myd == "1")
                                myd = "基本满意";
                            else if (temp_blbg.myd == "2")
                                myd = "满意";
                            if (temp_blbg.xbl == "1")
                                xbl = ">40%";
                            else if (temp_blbg.xbl == "0")
                                xbl = "<=40%";
                            if (temp_blbg.hsxb == "0")
                                xbl = "无";
                            else if (temp_blbg.hsxb == "1")
                                xbl = "有";
                            if (temp_blbg.jgxb == "0")
                                xbl = "无";
                            else if (temp_blbg.jgxb == "1")
                                xbl = "有";
                            if (temp_blbg.gnmxb == "0")
                                xbl = "无";
                            else if (temp_blbg.gnmxb == "1")
                                xbl = "有";
                            if (temp_blbg.dcgr == "0")
                                xbl = "无";
                            else if (temp_blbg.dcgr == "1")
                                xbl = "有";
                            if (temp_blbg.mjgr == "0")
                                xbl = "无";
                            else if (temp_blbg.mjgr == "1")
                                xbl = "有";
                            if (temp_blbg.yzfy == "0")
                                xbl = "无";
                            else if (temp_blbg.yzfy == "1")
                                xbl = "轻";
                            else if (temp_blbg.yzfy == "2")
                                xbl = "中";
                            else if (temp_blbg.yzfy == "3")
                                xbl = "重";

                            xgxx.Append("标本满意度<T>" + myd + "<BR/>");
                            xgxx.Append("细胞量<T>" + xbl + "<BR/>");
                            xgxx.Append("化生细胞<T>" + hsxb + "<BR/>");
                            xgxx.Append("颈管细胞<T>" + jgxb + "<BR/>");
                            xgxx.Append("宫内膜细胞<T>" + gnmxb + "<BR/>");
                            xgxx.Append("炎症反应<T>" + yzfy + "<BR/>");
                            xgxx.Append("滴虫感染<T>" + dcgr + "<BR/>");
                            xgxx.Append("霉菌感染<T>" + mjgr + "<BR/>");
                            xgxx.Append("提示HPV感染可能<T>" + hpvgr + "<BR/>");
                            xgxx.Append("提示疱疹病毒感染可能<T>" + pzgr + "<BR/>");
                            xgxx.Append("其他感染<T>" + qtgr + "<BR/>");

                            xgxx.Append("^");
                            xgxx.Append(temp_blbg.xbxzd);
                        }
                        else
                            xgxx.Append(" ^ ");


                        break;
                    case "020303":
                        command.CommandText = "select ' '  as jxsj,blzd from bl_bdbg where blh='" + blh + "'";
                        temp_blbg = OracleHelper.GetDataItems<m_bl_blbg>(command).FirstOrDefault();
                        if (!string.IsNullOrEmpty(temp_blbg.jxsj))
                            xgxx.Append(temp_blbg.jxsj);
                        else
                            xgxx.Append(" ");
                        xgxx.Append("^");
                        xgxx.Append(temp_blbg.blzd);
                        break;
                    case "020304":
                        command.CommandText = "select ' '  as jxsj,blzd from bl_hzbg where blh='" + blh + "'";
                        temp_blbg = OracleHelper.GetDataItems<m_bl_blbg>(command).FirstOrDefault();
                        if (!string.IsNullOrEmpty(temp_blbg.jxsj))
                            xgxx.Append(temp_blbg.jxsj);
                        else
                            xgxx.Append(" ");
                        xgxx.Append("^");
                        xgxx.Append(temp_blbg.blzd);
                        break;
                    case "020305":
                        string myzhjg = "", fishjg = "", myd1 = "", jsxb = "", her2 = "", cep17 = "", bzh = "";
                        command.CommandText = "select ' ' as jxsj,nvl(blzd,' ') as blzd,nvl(myzhjg,' ') as myzhjg,nvl(fishjg,' ') as fishjg,nvl(myd,' ') as myd,nvl(jsxb,' ') as jsxb,nvl(her2,' ') as her2,nvl(cep17,' ') as cep17,nvl(bzh,' ') as bzh from bl_fishbg where blh='" + blh + "'";
                        temp_blbg = OracleHelper.GetDataItems<m_bl_blbg>(command).FirstOrDefault();
                        if (temp_blbg != null)
                        {
                            xgxx.Append("HER2 免疫组化结果<T>" + myzhjg + "<BR/>");
                            xgxx.Append("HER2 FISH结果<T>" + fishjg + "<BR/>");
                            xgxx.Append("标本满意度<T>" + myd1 + "<BR/>");
                            xgxx.Append("计数细胞<T>" + jsxb + "<BR/>");
                            xgxx.Append("HER2 平均信号个数/细胞核<T>" + her2 + "<BR/>");
                            xgxx.Append("CEP17 平均信号个数/细胞核<T>" + cep17 + "<BR/>");
                            xgxx.Append("HER2/CEP17 平均比值<T>" + bzh + "<BR/>");

                            xgxx.Append("^");
                            xgxx.Append(temp_blbg.blzd);
                        }
                        else
                            xgxx.Append(" ^ ");
                        break;
                    default:
                        break;
                }
            }
            return xgxx.ToString();
        }



        /// <summary>
        /// 根据报告标识和检查类型获取指定报告
        /// </summary>
        /// <returns></returns>
        public List<m_getreport> Get_Report(string ll_bgbs, string ll_jclx)
        {
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.Parameters.Add(new OracleParameter(":jch", ll_bgbs));
                string bg = "";
                string sql_where = " and t1.accenum=:jch";
                string sql_where1 = " and t1.jch=:jch";
                string sql_where2 = " and b.sampleno=:jch";
                string sql_select;
                List<m_getreport> lm_getreports = new List<m_getreport>();
                bg = Get_bg(ll_jclx);
                if (bg == "PET" || bg == "WZDG")
                    sql_select = Get_sqlselect(bg, sql_where1, get_xgxx(ll_bgbs, bg));
                else if (bg == "BLSZ")
                    sql_select = Get_sqlselect(bg, sql_where1, get_xgxx_BLSZ(ll_bgbs));
                else if (bg == "JYBG")
                    sql_select = Get_sqlselect(ll_jclx, sql_where2, get_xgxx(ll_bgbs, bg));
                else
                    sql_select = Get_sqlselect(bg, sql_where, get_xgxx(ll_bgbs, bg));
                if (!string.IsNullOrEmpty(sql_select))
                {
                    command.CommandText = sql_select;
                    lm_getreports = OracleHelper.GetDataItems<m_getreport>(command).ToList();
                }
                return lm_getreports;
            }
        }
    }

    public class m_examinaim
    {
        public string EXAMINAIM { get; set; }
    }
    public class m_zhshysid
    {
        public long zhshysid { get; set; }
    }
    public class m_bl_sjb
    {
        public long sjks { get; set; }
        public string blk { get; set; }
        public string blh { get; set; }
    }
    public class m_bl_blbg
    {
        public string jxsj { get; set; }
        public string blzd { get; set; }
        public string myd { get; set; }
        public string xbl { get; set; }
        public string hsxb { get; set; }
        public string jgxb { get; set; }
        public string gnmxb { get; set; }
        public string dcgr { get; set; }
        public string mjgr { get; set; }
        public string yzfy { get; set; }
        public string xbxzd { get; set; }
    }

    public class m_getreport
    {
        private string _JCH;//检查号
        private string _BRBH;//病人编号
        private string _BRXM;//病人姓名
        private string _BRXB;//病人性别
        private DateTime? _CSRQ;//出生日期
        private string _TJBH;//体验编号
        private string _ZYH;//住院号
        private string _BQID;//病区ID
        private string _CWH;//床位号
        private string _LXDZ;//联系地址
        private DateTime? _SQRQ;//申请日期
        private string _JCFF;//检查方法
        private Int64? _SQZKID;//申请专科ID
        private string _SQZK;//申请专科
        private Int64? _SQYSID;//申请医生ID
        private string _SQYS;//申请医生
        private string _JCBW;//检查部位
        private string _SMCS;//扫描参数
        private string _JGSJ;//结果所见
        private string _JGZD;//结果诊断
        private DateTime? _BGSJ;//报告时间
        private Int64? _BGRYID;//报告人员ID
        private string _BGRY;//报告人员
        private DateTime? _SHSJ;//审核时间
        private Int64? _SHRYID;//审核人员ID
        private string _SHRY;//审核人员
        private Int64? _DYCS;//打印次数

        public string JCH
        {
            get { return _JCH; }
            set { _JCH = value; }
        }

        public string BRBH
        {
            get { return _BRBH; }
            set { _BRBH = value; }
        }


        public string BRXM
        {
            get { return _BRXM; }
            set { _BRXM = value; }
        }

        public string BRXB
        {
            get { return _BRXB; }
            set { _BRXB = value; }
        }

        public DateTime? CSRQ
        {
            get { return _CSRQ; }
            set { _CSRQ = value; }
        }

        public string TJBH
        {
            get { return _TJBH; }
            set { _TJBH = value; }
        }

        public string ZYH
        {
            get { return _ZYH; }
            set { _ZYH = value; }
        }

        public string BQID
        {
            get { return _BQID; }
            set { _BQID = value; }
        }

        public string CWH
        {
            get { return _CWH; }
            set { _CWH = value; }
        }

        public string LXDZ
        {
            get { return _LXDZ; }
            set { _LXDZ = value; }
        }

        public DateTime? SQRQ
        {
            get { return _SQRQ; }
            set { _SQRQ = value; }
        }

        public string JCFF
        {
            get { return _JCFF; }
            set { _JCFF = value; }
        }

        public Int64? SQZKID
        {
            get { return _SQZKID; }
            set { _SQZKID = value; }
        }

        public string SQZK
        {
            get { return _SQZK; }
            set { _SQZK = value; }
        }

        public Int64? SQYSID
        {
            get { return _SQYSID; }
            set { _SQYSID = value; }
        }

        public string SQYS
        {
            get { return _SQYS; }
            set { _SQYS = value; }
        }

        public string JCBW
        {
            get { return _JCBW; }
            set { _JCBW = value; }
        }

        public string SMCS
        {
            get { return _SMCS; }
            set { _SMCS = value; }
        }

        public string JGSJ
        {
            get { return _JGSJ; }
            set { _JGSJ = value; }
        }

        public string JGZD
        {
            get { return _JGZD; }
            set { _JGZD = value; }
        }

        public DateTime? BGSJ
        {
            get { return _BGSJ; }
            set { _BGSJ = value; }
        }

        public Int64? BGRYID
        {
            get { return _BGRYID; }
            set { _BGRYID = value; }
        }

        public string BGRY
        {
            get { return _BGRY; }
            set { _BGRY = value; }
        }

        public DateTime? SHSJ
        {
            get { return _SHSJ; }
            set { _SHSJ = value; }
        }

        public Int64? SHRYID
        {
            get { return _SHRYID; }
            set { _SHRYID = value; }
        }

        public string SHRY
        {
            get { return _SHRY; }
            set { _SHRY = value; }
        }

        public Int64? DYCS
        {
            get { return _DYCS; }
            set { _DYCS = value; }
        }
    }
}
