using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_yspb","医生排班信息，包括医生排班信息和剩余号")]
    [Select("GetTodayHalfPB", "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.ztbz='1' and pb.zkid=:ZKID and  pb.sbsj >=:SBSJ and pb.xbsj>=:XBSJ and pb.xbsj<=:XBSJ2 and pb.zllx in('02','04','07') order by pb.sbsj, pb.yqdm desc, pb.gzdm", "检索当天下午班和全天班和剩余号", true)]
    [Select("GetAllPB", "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.ztbz='1'  and pb.zkid=:ZKID and  pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') order by pb.sbsj, pb.yqdm desc, pb.gzdm", "检索给定日期内所有班和剩余号", true)]
    [Select("GetPBByID", "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,pb.zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.pbid=:pbid", "根据排班ID获取排班信息", false)]
    [Select("GetAllDoctor", "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb,r_ryk r where pb.rykid=r.id(+) and pb.ztbz='1'  and (trim(replace(pb.ysxm,' ','')) like :ysxm or r.py like :yspy) and  pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') ", "根据医生姓名检索给定日期内所有班和剩余号", true)]
    [Select("GetByRYKID", "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.rykid=:RYKID and pb.ztbz='1' and pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') ", "根据医生人员库id检索给定日期内所有班和剩余号", true)]
    [Select("GetZLLXByPBID", "select zllx from yyfz_yspb where pbid=:PBID", "获取诊疗类型", false)]
    [Select("GetSXBSJ", "select zd2 ZTBZ from yyfz_gxdy where lb='YFSJ' and zd1=:ZTBZ", "获取排版上下班时间(结果作为ZTBZ传出)", false)]
    [Select("CheckPB", "select count(*) pbid from yyfz_yspb where ztbz<>'9' and  pbid=:PBID", "判断排班是否已被取消", false)]
    public class m_yyfz_pbxx
    {
        public Int64 PBID { get; set; }
        public string YSXM { get; set; }
        public string YSPY { get; set; }
        public string YSXB { get; set; }
        public Int64? YSYHID { get; set; }
        public Int64? RYKID { get; set; }
        public Int64? ZKID { get; set; }
        public string GZDM { get; set; }
        public string ZLLX { get; set; }
        public Int64? FJID { get; set; }
        public string ZTBZ { get; set; }
        public DateTime? SBSJ { get; set; }
        public DateTime? XBSJ { get; set; }
        public DateTime? XBSJ2 { get; set; }
        public string FZZBH { get; set; }
        public Int64? ZKXH { get; set; }
        public Int64? JH { get; set; }
        public Int64? KXYZ { get; set; }
        public Int64? FMYZ { get; set; }
        public string YQDM { get; set; }
        public Int64? CZZID { get; set; }
        public DateTime? XGSJ { get; set; }
        public string BCLB { get; set; }
        public Int64? BMID { get; set; }
        public string ZBYY { get; set; }
        public string KSMC { get; set; }
        public int SYH { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal XMJE { get; set; }
    }
}
