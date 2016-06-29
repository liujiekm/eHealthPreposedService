using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yl_mzypyz", "mapp1", "门诊药品医嘱")]
    [Select("GetByZLHDID", "select zlhdid,mc,dw,jx,jl,bzl,dj,sl,cs,ycyl,jldw,pl,ff,gysj from yl_mzypyz where zlhdid = :zlhdid and (ztbz is null or ztbz<>'0')", "根据zlhdid获取药品医嘱信息")]
    [Select("GetByZLHDID_CG", "select zlhdid,mc,dw,jx,jl,bzl,dj,sl,cs,ycyl,jldw,pl,ff,gysj from yl_mzypyzcg where zlhdid = :zlhdid and ztbz='1'", "根据zlhdid获取药品医嘱信息存根")]
    public class m_yl_mzypyz
    {
        public Int64 ZLHDID { get; set; }
        public string MC { get; set; }
        public string DW { get; set; }
        public string JX { get; set; }
        public string JL { get; set; }
        public Int64 BZL { get; set; }
        public decimal DJ { get; set; }
        public decimal SL { get; set; }
        public Int64 CS { get; set; }
        public decimal YCYL { get; set; }
        public string PL { get; set; }
        public string FF { get; set; }
        public string GYSJ { get; set; }
        public string JLDW { get; set; }
        public string YFYL { get; set; }

    }
}
