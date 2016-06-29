using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("br_jctx",  "病人检查报告单")]
    [Select("GetBGList", "select distinct t1.bgbs, t1.brbh,t1.brxm,t1.bgsj,nvl(t2.mc,'检验') as jclx,t1.jclx as jclx1 from br_jctx t1,yyfz_jclx t2 where t1.jclx=t2.dm(+) and t1.brbh=:brbh and t1.ztbz='0' and t1.bgsj>sysdate-365", "通过病人编号返回所有的数据数据")]
    public class m_br_jctx
    {
        public string BGBS { get; set; }
        public string BRBH { get; set; }
        public string BRXM { get; set; }
        public DateTime BGSJ { get; set; }
        public string JCLX { get; set; }
        public string JCLX1 { get; set; }
        public string JYMC { get; set; }
    }
}
