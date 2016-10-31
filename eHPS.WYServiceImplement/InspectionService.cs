//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 温附一检查检验报告实现逻辑
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/4 9:58:33
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;

using Dapper;
using eHPS.WYServiceImplement.Model;
using eHPS.Common;

namespace eHPS.WYServiceImplement
{
    public class InspectionService : IInspection
    {
        private List<InspectionReport> GetInspectionReportByPatientId(string patientId)
        {
            var reports = new List<InspectionReport>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var getReports = @"select distinct t1.bgbs as ReportId, t1.brbh as PatientId,t1.brxm as PatientName,t1.bgsj as ReportTime,nvl(t2.mc,'检验') as InspectionTypeName,t1.jclx as InspectionTypeCode from 
                                            br_jctx t1,yyfz_jclx t2 where t1.jclx=t2.dm(+)  and t1.ztbz='0' and t1.bgsj>sysdate-365 and t1.brbh=:PatientId";
                var conditon = new { PatientId = patientId };
                reports = con.Query<InspectionReport>(getReports, conditon).ToList();
                foreach (var item in reports)
                {
                    if(item.InspectionTypeCode=="JY")
                    {
                        item.InspectionTypeName = "化验";
                        var examCommand = "select examinaim from L_PATIENTINFO where sampleno='" + item.ReportId + "'";
                        var result = con.Query(examCommand).FirstOrDefault();
                        item.InspectionName = result==null? "检验" : (string)result.EXAMINAIM;
                    }
                    else
                    {
                        item.InspectionName = item.InspectionTypeName;
                        item.InspectionTypeName = "检查";
                    }
                }
                return reports.OrderByDescending(p=>p.ReportTime).ToList();
            }
           
        }



        /// <summary>
        /// 生成检验、检查详情查询语句
        /// </summary>
        /// <returns></returns>
        private string GenerateCommand(string inspectionType,string condition,string verifierAndResult)
        {
            string ls_sql = "";
            string sjks = "", jgsj = "", jgzd = "";
            if (inspectionType == "BLSZ")
            {
                sjks = verifierAndResult.Split('^')[0];
                jgsj = verifierAndResult.Split('^')[1];
                jgzd = verifierAndResult.Split('^')[2];
            }
            switch (inspectionType)
            {
                case "CTBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                                nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk,'' as sqysid,' ' as sqys,t1.jcbw,
                                '层厚<T>' || t1.ch || '<T>' || 'mm<BR/>' || '层间隔<T>' || t1.cjg || '<T>' || 'mm<BR/>' as smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                                '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'" + verifierAndResult + "' as shry ,t1.dycs from f_ctbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + "  order by t1.bgsj";
                    break;
                case "PFBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,'' as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_pfbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + " order by t1.bgsj";
                    break;
                case "MRBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              smcs,t1.jgsj,t1.jgzd1 || '<BR/>' || t1.jgzd2 || '<BR/>' || t1.jgzd3 as jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_mribg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + " order by t1.bgsj";
                    break;
                case "BCBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,'' as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,t1.jgsj,t1.jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_bcbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + " order by t1.bgsj";
                    break;
                case "NJBG":
                    ls_sql = @"select t1.ACCENUM as jch,to_char(t1.bh) as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,'' as tjbh,
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '' as smcs,'' as jgsj,t1.jgzd as jgzd,'' as bgsj,
                              '' as bgryid,t1.czz as bgry,'' as shsj,'' as zhshysid,'' as shry,'' as dycs
                              from f_njbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + " order by bgsj";
                    break;
                case "XCBG":
                    ls_sql = @"select t1.ACCENUM as jch,t1.bh as brbh,t1.xm as brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),t1.bq as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jcnr as jclx,t2.sqzkid,nvl(t1.sqks,' ') as sqzk ,'' as sqysid,' ' as sqys,'' as jcbw,
                              '主动脉根部内径<T>' || t1.aord || '<T>' || 'mm<BR/>' || '左室舒张末期内径<T>' || t1.lvdd || '<T>' || 'mm<BR/>' || '左室收缩末期内径<T>' || t1.lvds || '<T>' || 'mm<BR/>' ||
                               '左房内径<T>' || t1.lad || '<T>' || 'mm<BR/>' || '室间隔厚度<T>' || t1.ivst || '<T>' || 'mm<BR/>' || '左室后壁厚度<T>' || t1.pwt || '<T>' || 'mm<BR/>' || '主肺动脉<T>'
                                || t1.mpad || '<T>' || 'mm<BR/>' || '左室舒张末期容量<T>' || t1.edv || '<T>' || 'mL<BR/>' || '左室收缩末期容量<T>' || t1.esv || '<T>' || 'mL<BR/>' || '每搏量<T>' ||
                                 t1.sv || '<T>' || 'mL/B<BR/>' || '心输出量<T>' || t1.co || '<T>' || 'L/min<BR/>' || '心脏指数<T>' || t1.ci || '<T>' || 'L/min/m<BR/>' || '室内径缩短分数<T>' || t1.fs || '<T>' || '%<BR/>' ||
                                  '左室射血分数<T>' || t1.ef || '<T>' || '%<BR/>'  as smcs,t1.jgsj,t1.jgzd,t1.bgsj,
                              '' as bgryid,t1.czz as bgry,t1.shsj,'' as zhshysid,'' as shry,t1.dycs
                              from f_xcbg t1,yyfz_tjyy t2 where t1.accenum=t2.jch(+) " + condition + " order by t1.bgsj";
                    break;
                case "BLSZ":
                    ls_sql = @"select t1.jch,t1.brbh,t1.brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,' ' as tjbh,
                              nvl(t1.zyh,' ') as zyh,' ' as bqid,nvl(t1.ch,' ') as cwh,t1.lxdz,t1.jsrq as sqrq,'BL' as jclx,t1.sjks as sqzkid,'" + sjks + "' as sqks ,'' as sqysid,' ' as sqys,' ' as jcbw,' ' as smcs,'" + jgsj + "'  as jgsj,'" + jgzd + "' as jgzd,t1.bgrq as bgsj,'' as bgryid,' ' as bgry,t1.shrq as shsj,'' as zhshysid,' ' as shry,'' as dycs from bl_sjb t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) and t1.bgzt='7' " + condition + " order by t1.shrq";
                    break;
                case "PET":
                    ls_sql = @"select t1.jch ,t1.brbh,t1.brxm,decode(t1.xb,'1','男','女') as brxb,t1.csrq,' ' as tjbh,
                              nvl(t1.zyh,' '),' ' as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,' ' as jclx,t2.sqzkid,' ' as sqks ,'' as sqysid,' ' as sqys,t1.jcbw,
                              ' ' as smcs,' ' as jgsj,' ' as jgzd,t1.bgsj,
                              '' as bgryid,' ' as bgry,t1.shsj,'' as zhshysid,' ' as shry,'' as dycs
                              from fs_petbg t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) " + condition + "  order by t1.bgsj";
                    break;
                case "JYBG":
                    ls_sql = @"SELECT b.sampleno,c.patientid,c.patientname brxm,decode(c.sex,'1','男','女') as brxb,c.birthday,'',
                            c.patientid,'','','',null sqrq,null jclx,null sqzkid,' ' as sqks,null as sqysid,' ' as sqys,examinaim as jcbw,
                   '' smcs,CHINESENAME||'&'|| b.TESTRESULT||'&'|| HINT||'&'||lpad(reflo,7,' ')||decode(nvl(length(refhi),0),0,'','~')||rpad(nvl(refhi,' '),decode(nvl(length(refhi),0),0,5 - length(reflo),7),' ') jgsj,' ' as jgzd,executetime bgsj,
                            null as bgryid,' ' as bgry,checktime,'','','' as dycs from L_TESTDESCRIBE a,L_TESTRESULT b,l_patientinfo c
                    WHERE a.TESTID = b.TESTID   AND    b.ISPRINT = 1 and b.Teststatus >=4 
                    and b.sampleno=c.sampleno  " + condition;
                    break;
                case "WZDG":
                    ls_sql = @"select t1.jch,t1.brbh,t1.brxm,decode(t1.brxb,'1','男','女') as brxb,t1.csrq,nvl(t1.tjbh,' '),
                              nvl(t1.zyh,' '),' ' as bqid,nvl(t1.cwh,' '),t1.lxdz,t1.sqrq,t1.jclx,t2.sqzkid,' ' as sqks ,'' as sqysid,' ' as sqys,t1.jcbw,
                              '层厚<T>' || t1.ch || '<T>' || 'mm<BR/>' || '层间隔<T>' || t1.cjg || '<T>' || 'mm<BR/>' as smcs,t1.jgsj,' ' as jgzd,t1.bgsj,
                              '' as bgryid,' ' as bgry,t1.shsj,'' as zhshysid,' ' as shry,'' as dycs
                              from fs_jrbg t1,yyfz_tjyy t2 where t1.jch=t2.jch(+) " + condition + " order by t1.shsj";
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
        /// 转换检验检查类型代码
        /// </summary>
        /// <param name="inpsectionTypeCode"></param>
        /// <returns></returns>
        private string TransferInpsectionTypeCode(string inpsectionTypeCode)
        {
            string inspetionType = "";
            if (inpsectionTypeCode == "CT")
                inspetionType = "CTBG";
            else if (inpsectionTypeCode == "RF" || inpsectionTypeCode == "CR" || inpsectionTypeCode == "DR")
                inspetionType = "PFBG";
            else if (inpsectionTypeCode == "MR")
                inspetionType = "MRBG";
            else if (inpsectionTypeCode == "BC")
                inspetionType = "BCBG";
            else if (inpsectionTypeCode == "XC" || inpsectionTypeCode == "GQ" || inpsectionTypeCode == "ER" || inpsectionTypeCode == "DJ" || inpsectionTypeCode == "CS" || inpsectionTypeCode == "FD" || inpsectionTypeCode == "WJ" || inpsectionTypeCode == "ZJ" || inpsectionTypeCode == "PG" || inpsectionTypeCode == "CJ" || inpsectionTypeCode == "NJ" || inpsectionTypeCode == "BJ" || inpsectionTypeCode == "XJ")
                inspetionType = "NJBG";
            else if (inpsectionTypeCode == "CU")
                inspetionType = "XCBG";
            else if (inpsectionTypeCode == "BL")
                inspetionType = "BLSZ";
            else if (inpsectionTypeCode == "PT")
                inspetionType = "PET";
            else if (inpsectionTypeCode == "XA")
                inspetionType = "WZDG";
            else if (inpsectionTypeCode == "JY")
                inspetionType = "JYBG";
            return inspetionType;
        }



        /// <summary>
        /// 获取相关检查检验的审核人
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="inspectionType"></param>
        /// <returns></returns>
        private string GetInspectionVerifier(string inspectionId,string inspectionType)
        {
            var verifier = String.Empty;
            var baseCommand = @"select x.yhxm from {0} f,xtgl_yhxx x where x.yhid=f.zhshysid and f.accenum=:InspectionId";

            String command = String.Empty;
            if (inspectionType == "CTBG" || inspectionType == "PFBG" || inspectionType == "MRBG")
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    switch (inspectionType)
                    {
                        case "CTBG":
                            command = String.Format(baseCommand, "f_ctbg");
                            break;
                        case "PFBG":
                            command = String.Format(baseCommand, "f_pfbg");
                            break;
                        case "MRBG":
                            command = String.Format(baseCommand, "f_mribg");
                            break;
                    }

                    var condition = new { InspectionId = inspectionId };
                    var result = con.Query(command, condition).FirstOrDefault();
                    verifier = (string)result.yhxm;
                }
            }
            

            return verifier;
        }



        /// <summary>
        /// 获取相关检查检验的结果信息
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns>
        ///  ^ ^ 格式
        /// </returns>
        private string GetInspectionResult(string inspectionId)
        {
            var inspectionResult = new StringBuilder();
            var blk = string.Empty;
            var blh = string.Empty;
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var baseCommand = @"select x.bmmc,b.blk,b.blh from bl_sjb b,xtgl_bmdm x where b.sjks=x.bmid and b.jch='" + inspectionId + "'";
                var temp = con.Query(baseCommand).FirstOrDefault();
                if(temp!=null)
                {
                    inspectionResult.Append(temp.bmmc == null ? " " : (string)temp.bmmc);
                    inspectionResult.Append("^");
                    blk = (string)temp.blk;
                    blh = (string)temp.blh;
                }
                var switchCommand = String.Empty;
                switch (blk)
                {
                    case "020301":
                        switchCommand = "select jxsj,blzd from bl_blbg where blh='" + blh + "'";
                        var temp_blbg = con.Query(switchCommand).FirstOrDefault();
                        if (null!=temp_blbg.jxsj)
                        {
                            inspectionResult.Append((string)temp_blbg.jxsj);
                        }
                        else
                        {
                            inspectionResult.Append(" ");
                        }
                        inspectionResult.Append("^");
                        inspectionResult.Append((string)temp_blbg.blzd);
                        break;
                    case "020302":
                        string myd = "", xbl = "", hsxb = "", jgxb = "", gnmxb = "", yzfy = "", dcgr = "", mjgr = "", hpvgr = "", pzgr = "", qtgr = "";
                        switchCommand = "select ' ' as jxsj,nvl(xbxzd,' ') as xbxzd,nvl(myd,' ') as myd,nvl(xbl,' ') as xbl,nvl(hsxb,' ') as hsxb,nvl(jgxb,' ') as jgxb,nvl(gnmxb,' ') as gnmxb,nvl(yzfy,' ') as yzfy,nvl(dcgr,' ') as dcgr,nvl(mjgr,' ') as mjgr,nvl(hpvgr,' ') as hpvgr,nvl(pzgr,' ') as pzgr,nvl(qtgr,' ') as qtgr from bl_xbxbg where blh='" + blh + "'";
                        temp_blbg = con.Query(switchCommand).FirstOrDefault();
                        if (temp_blbg != null)
                        {
                            if ((string)temp_blbg.myd == "0")
                                myd = "不满意";
                            else if ((string)temp_blbg.myd == "1")
                                myd = "基本满意";
                            else if ((string)temp_blbg.myd == "2")
                                myd = "满意";
                            if ((string)temp_blbg.xbl == "1")
                                xbl = ">40%";
                            else if ((string)temp_blbg.xbl == "0")
                                xbl = "<=40%";
                            if ((string)temp_blbg.hsxb == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.hsxb == "1")
                                xbl = "有";
                            if ((string)temp_blbg.jgxb == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.jgxb == "1")
                                xbl = "有";
                            if ((string)temp_blbg.gnmxb == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.gnmxb == "1")
                                xbl = "有";
                            if ((string)temp_blbg.dcgr == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.dcgr == "1")
                                xbl = "有";
                            if ((string)temp_blbg.mjgr == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.mjgr == "1")
                                xbl = "有";
                            if ((string)temp_blbg.yzfy == "0")
                                xbl = "无";
                            else if ((string)temp_blbg.yzfy == "1")
                                xbl = "轻";
                            else if ((string)temp_blbg.yzfy == "2")
                                xbl = "中";
                            else if ((string)temp_blbg.yzfy == "3")
                                xbl = "重";

                            inspectionResult.Append("标本满意度<T>" + myd + "<BR/>");
                            inspectionResult.Append("细胞量<T>" + xbl + "<BR/>");
                            inspectionResult.Append("化生细胞<T>" + hsxb + "<BR/>");
                            inspectionResult.Append("颈管细胞<T>" + jgxb + "<BR/>");
                            inspectionResult.Append("宫内膜细胞<T>" + gnmxb + "<BR/>");
                            inspectionResult.Append("炎症反应<T>" + yzfy + "<BR/>");
                            inspectionResult.Append("滴虫感染<T>" + dcgr + "<BR/>");
                            inspectionResult.Append("霉菌感染<T>" + mjgr + "<BR/>");
                            inspectionResult.Append("提示HPV感染可能<T>" + hpvgr + "<BR/>");
                            inspectionResult.Append("提示疱疹病毒感染可能<T>" + pzgr + "<BR/>");
                            inspectionResult.Append("其他感染<T>" + qtgr + "<BR/>");

                            inspectionResult.Append("^");
                            inspectionResult.Append((string)temp_blbg.xbxzd);
                        }
                        else
                            inspectionResult.Append(" ^ ");


                        break;
                    case "020303":
                        switchCommand = "select ' '  as jxsj,blzd from bl_bdbg where blh='" + blh + "'";
                        temp_blbg = con.Query(switchCommand).FirstOrDefault();
                        if (null!=temp_blbg.jxsj)
                            inspectionResult.Append((string)temp_blbg.jxsj);
                        else
                            inspectionResult.Append(" ");
                        inspectionResult.Append("^");
                        inspectionResult.Append((string)temp_blbg.blzd);
                        break;
                    case "020304":
                        switchCommand = "select ' '  as jxsj,blzd from bl_hzbg where blh='" + blh + "'";
                        temp_blbg = con.Query(switchCommand).FirstOrDefault();
                        if (null!=temp_blbg.jxsj)
                            inspectionResult.Append((string)temp_blbg.jxsj);
                        else
                            inspectionResult.Append(" ");
                        inspectionResult.Append("^");
                        inspectionResult.Append((string)temp_blbg.blzd);
                        break;
                    case "020305":
                        string myzhjg = "", fishjg = "", myd1 = "", jsxb = "", her2 = "", cep17 = "", bzh = "";
                        switchCommand = "select ' ' as jxsj,nvl(blzd,' ') as blzd,nvl(myzhjg,' ') as myzhjg,nvl(fishjg,' ') as fishjg,nvl(myd,' ') as myd,nvl(jsxb,' ') as jsxb,nvl(her2,' ') as her2,nvl(cep17,' ') as cep17,nvl(bzh,' ') as bzh from bl_fishbg where blh='" + blh + "'";
                        temp_blbg = con.Query(switchCommand).FirstOrDefault();
                        if (temp_blbg != null)
                        {
                            inspectionResult.Append("HER2 免疫组化结果<T>" + myzhjg + "<BR/>");
                            inspectionResult.Append("HER2 FISH结果<T>" + fishjg + "<BR/>");
                            inspectionResult.Append("标本满意度<T>" + myd1 + "<BR/>");
                            inspectionResult.Append("计数细胞<T>" + jsxb + "<BR/>");
                            inspectionResult.Append("HER2 平均信号个数/细胞核<T>" + her2 + "<BR/>");
                            inspectionResult.Append("CEP17 平均信号个数/细胞核<T>" + cep17 + "<BR/>");
                            inspectionResult.Append("HER2/CEP17 平均比值<T>" + bzh + "<BR/>");

                            inspectionResult.Append("^");
                            inspectionResult.Append((string)temp_blbg.blzd);
                        }
                        else
                            inspectionResult.Append(" ^ ");
                        break;
                    default:
                        break;
                }

            }

            return inspectionResult.ToString();
        }





        /// <summary>
        /// 根据报告标识和检查类型获取指定报告
        /// </summary>
        /// <returns></returns>
        public List<m_getreport> GetReport(string reportId, string inspectionTypeCode)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var condition = new { jch = reportId };
                string bg = "";
                string sql_where = " and t1.accenum=:jch";
                string sql_where1 = " and t1.jch=:jch";
                string sql_where2 = " and b.sampleno=:jch";
                string sql_select;
                List<m_getreport> lm_getreports = new List<m_getreport>();
                bg = TransferInpsectionTypeCode(inspectionTypeCode);
                if (bg == "PET" || bg == "WZDG")
                    sql_select = GenerateCommand(bg, sql_where1, GetInspectionVerifier(reportId, bg));
                else if (bg == "BLSZ")
                    sql_select = GenerateCommand(bg, sql_where1, GetInspectionResult(reportId));
                else if (bg == "JYBG")
                    sql_select = GenerateCommand(bg, sql_where2, GetInspectionVerifier(reportId, bg));
                else
                    sql_select = GenerateCommand(bg, sql_where, GetInspectionVerifier(reportId, bg));
                if (!string.IsNullOrEmpty(sql_select))
                {
                    var command = sql_select;
                    lm_getreports = con.Query<m_getreport>(command,condition).ToList();
                }
                return lm_getreports;
            }
        }



        /// <summary>
        /// 接口实现
        /// </summary>
        /// <param name="patientId">
        /// 用户标识
        /// </param>
        /// <returns></returns>
        public List<InspectionReportDetail> GetInspectionReportDetailByPatientId(string patientId)
        {
            var data = new List<InspectionReportDetail>();
            var list = GetInspectionReportByPatientId(patientId);
            foreach (var item in list)
            {
                //数据查询
                var report = GetReport(item.ReportId, item.InspectionTypeCode);
                //实体转换
                if (report != null && report.Count > 0)
                    //添加化验单详情
                    if (item.InspectionTypeCode.ToUpper() == "JY")
                    {
                        var assayList = new List<LaboratoryItemDetail>();
                        foreach (var ass in report)
                        {
                            var project = ass.JGSJ.Split('&');
                            var assay = new LaboratoryItemDetail() { ItemName = project[0], Result = project[1], IsError = project[2], Normal = project[3].Trim() };
                            assayList.Add(assay);
                        }
                        var dataReport = new InspectionReportDetail()
                        {
                            ReportId = item.ReportId,
                            ReportTime = report[0].BGSJ.HasValue ? report[0].BGSJ.Value : DateTime.Now,
                            Title = report[0].JCBW,
                             InspectionTypeName = "化验",
                            InspectionTypeCode = item.InspectionTypeCode,
                            PatientName = report[0].BRXM,
                            Details = assayList,
                            Reporter = report[0].BGRY
                        };
                        data.Add(dataReport);
                    }
                    else  //添加特检单详情
                    {
                        var temp = report[0];
                        temp.JGSJ = FormatHtml.NoHTML(temp.JGSJ);
                        temp.JGZD = FormatHtml.NoHTML(temp.JGZD);

                        var dataReport = new InspectionReportDetail()
                        {
                            Result = temp.JGSJ,
                            Diagnostic = temp.JGZD,
                            ReportId = item.ReportId,
                            ReportTime = temp.BGSJ.HasValue ? temp.BGSJ.Value : DateTime.Now,
                            Title = temp.JCBW,
                            InspectionTypeName = "特检",
                            InspectionTypeCode = item.InspectionTypeCode,
                            PatientName = temp.BRXM,
                            Reporter = report[0].BGRY
                        };
                        data.Add(dataReport);
                    }
            }

            return data;
        }
    }
}
