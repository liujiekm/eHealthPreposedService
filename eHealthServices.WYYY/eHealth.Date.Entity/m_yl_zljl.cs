using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yl_zljl", "诊疗记录")]
    [Select("GetJLID", "select jlid from yl_zljl a,xtgl_zjsbg b,yl_zlhd c where a.gsdm = b.gsdm and a.zlhdid = c.zlhdid and (b.zkid = c.jzzkid or b.zkid=0) and a.zlhdid = :zlhdid", "根据zlhdid获取jlid", false)]
    [Select("GetZLHDByBRBH", "select zlhdid,sczlhdid,zllx,jzzkid,jzysyhid,kssj from yl_zlhd where brbh = :brbh and zlzt = '1' order by kssj desc", "根据病人编号获取诊疗活动")]
    [Select("GetZLHDByID", "select zlhdid,sczlhdid,zllx,jzzkid,jzysyhid,kssj from yl_zlhd where zlhdid = :zlhdid and zlzt = '1' order by kssj desc", "根据诊疗活动id获取诊疗活动",false)]
    public class m_yl_zljl
    {
        public Int64 JZZKID { get; set; }
        public Int64 JZYSYHID { get; set; }
        public DateTime KSSJ { get; set; }
        public string ZLLX { get; set; }
        public Int64? SCZLHDID { get; set; }
        public long JLID { get; set; }
        public long ZLHDID { get; set; }
        public string BRBH { get; set; }
    }
}
