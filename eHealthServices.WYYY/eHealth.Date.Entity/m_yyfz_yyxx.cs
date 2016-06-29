using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_yyxx",  "预约信息")]
    [Select("CheckSJDExist", "select count(yysj) yyxh from yyfz_yyxx where (yysj=:YYSJ or yyxh=:YYXH) and ztbz<>'9' and pbid=:PBID", "获取指定排班指定预约号是否已被预约，数量作为YYXH返回", false)]
    [Select("GetYYXXForSJD", "select fzyyid,yysj,yyxh,pbid from yyfz_yyxx where ztbz<>'9' and pbid=:pbid", "获取算时间点的预约信息")]
    [Select("GetByFZYYID", "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where fzyyid = :fzyyid", "获取预约信息通过预约id", false)]
    [Update("UpdateQDBZ", "ZTBZ,QDSJ", "FZYYID=:FZYYID", "修改签到标志和时间")]
    [Update("UpdateZTBZ", "ZTBZ", "FZYYID=:FZYYID", "修改状态标志")]
    [Update("UpdateZTBZCZZ", "ZTBZ,CZZID,XGSJ", "FZYYID=:FZYYID", "修改状态标志和操作者修改时间")]
    [Add]
    [Select("CheckYYCountByBRBH", "select count(a.lxdh) yyxh from yyfz_yyxx a,yyfz_yspb b,cw_khxx c where c.brbh=:BRBH and b.pbid=:PBID and  ((c.lxdh is not null and a.lxdh like '%'||c.lxdh||'%')or(c.yddh is not null and  a.lxdh like '%'||c.yddh||'%')) and a.yysj>=b.sbsj  and a.yysj<=b.xbsj and (a.ztbz='1' or a.ztbz='2') and (a.zllx='02' or a.zllx='04' or a.zllx='07')", "(已登录)检查一个半天限约两个号源,数量作为YYXH返回", false)]
    [Select("CheckExist1", "select count(brbh) yyxh from yyfz_yyxx where ztbz<>'9' and brbh=:BRBH and pbid=:PBID", "(已登录)检查用户在此排班是否已经有预约,数量作为YYXH返回", false)]
    [Select("CheckYYCountBySJH2", "select count(lxdh) YYXH from yyfz_yyxx,(select sbsj, xbsj from yyfz_yspb where pbid=:PBID) where lxdh like :LXDH and yysj>=sbsj and yysj<=xbsj and (ztbz='1' or ztbz='2') and (zllx='02' or zllx='04' or zllx='07')", "(未登录)检查一个半天限约两个号源,数量作为YYXH返回", false)]
    [Select("CheckExist2", "select count(fzyyid) YYXH from yyfz_yyxx where ztbz<>'9' and brxm=:BRXM and lxdh like :LXDH and pbid=:PBID;", "(未登录)检查用户在此排班是否已经有预约,数量作为YYXH返回", false)]
    [Select("CheckYYSJ", "select count(pbid) YYXH from yyfz_yyxx where pbid=:PBID and yysj=:YYSJ and ztbz<>'9'", "检查该时间点是否已被其他人预约,数量作为YYXH返回", false)]
    [Select("GetByBRBH", "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where djsj>sysdate-31 and yyfs<>'F' and brbh=:brbh order by ztbz,yysj desc", "根据病人编号获取用户历史31天内的预约记录")]
    [Select("GetByBRBHTody", "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where trunc(sysdate)=trunc(yysj) and yyfs<>'F' and brbh=:brbh order by yysj desc,YSYHID", "根据病人编号获取用户当天的预约记录")]
    [Select("GetByXMDH", "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where djsj>sysdate-31 and yyfs<>'F' and brxm=:BRXM and lxdh like :LXDH order by ztbz,yysj desc", "根据病人姓名和手机号获取病人历史预约")]
    [Select("GetByXMDHTody", "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where trunc(sysdate)=trunc(yysj) and yyfs<>'F' and brxm=:BRXM and lxdh like :LXDH order by yysj desc,YSYHID", "根据病人姓名和手机号获取预约信息")]
    [Select("GetYYWZByPBID", "select a.yqdm,b.fjmc,b.fjwz from yyfz_yspb a,yyfz_fjxx b where a.fjid=b.fjid and a.pbid=:PBID", "根据排版id获取预约科室位置信息", false)]
    [Select("GetYYXHByPBID", "select yyxh from yyfz_yyxx where pbid=:PBID and ztbz<>'9' order by yyxh", "获取已预约的预约序号")]
    public class m_yyfz_yyxx
    {
        [FieldSeq("seq_yyfz_yyxx_id")]
        public long FZYYID { get; set; }
        public DateTime? YYSJ { get; set; }
        public int? YYXH { get; set; }
        public long? PBID { get; set; }
        public string BRBH { get; set; }
        public string BRXM { get; set; }
        public string BRXB { get; set; }
        public DateTime? CSRQ { get; set; }
        public string LXDH { get; set; }
        public string SFZ { get; set; }
        public string LXDZ { get; set; }
        public long? PDH { get; set; }
        public string FZZBH { get; set; }
        public long? ZKID { get; set; }
        public long? YSYHID { get; set; }
        public string YYFS { get; set; }
        public string ZTBZ { get; set; }
        public string ZLLX { get; set; }
        public DateTime? QDSJ { get; set; }
        public DateTime? JZSJ { get; set; }
        public long? GHID { get; set; }
        public string BZ { get; set; }
        public long? DJRYID { get; set; }
        public long? CZZID { get; set; }
        public DateTime? XGSJ { get; set; }
        public string HJLX { get; set; }
        public string YJNR { get; set; }
        public DateTime? DJSJ { get; set; }
        public long? QDRYID { get; set; }
        public string GZDM { get; set; }
        public string QHPZDM { get; set; }
        public string ZMTX { get; set; }
        public string ZBYY { get; set; }
    }
}
