using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yl_zlhd","诊疗诊断")]
    [Select("GetZKZDByZLHDID", "select qz,lczd,hz  from yl_zlzd where zlhdid=:zlhdid order by zdxh", "根据诊疗活动id获取诊疗诊断")]
    public class m_yl_zlzd
    {
        public string QZ { get; set; }
        public string LCZD { get; set; }
        public string HZ { get; set; }
        public long ZLHDID { get; set; }
    }
}
