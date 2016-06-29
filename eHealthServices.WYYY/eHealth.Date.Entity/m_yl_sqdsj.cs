using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    /// <summary>
    /// 特检
    /// </summary>
     [Table("yl_sqdsj", "特检")]
     [Select("GetTJByZLHDID", "select l.tj,xq.mc,l.sclrsj,l.xm,l.yzyhid from yl_sqdsj l,yl_yzml xq where l.mlid=xq.yzmlid(+) and l.zlhdid=:zlhdid AND L.ZTBZ=1 order by l.yzsj desc","根据诊疗活动id获取特检信息")]
    public class m_yl_sqdsj
    {
        /// <summary>
        /// id
        /// </summary>
        public long SQDSJID { get; set; }
        /// <summary>
        /// 报告名称
        /// </summary>
        public string MC { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string XM { get; set; }
        public long YZYHID { get; set; }
        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime SCLRSJ { get; set; }

        /// <summary>
        /// 特检结果
        /// </summary>
        public string TJ { get; set; }
        /// <summary>
        /// 诊疗活动id
        /// </summary>
        public long ZLHDID { get; set; }
    }
}
