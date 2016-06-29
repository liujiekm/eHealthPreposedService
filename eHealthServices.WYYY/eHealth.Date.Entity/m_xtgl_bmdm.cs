using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("xtgl_bmdm","部门代码")]
    [Select("GetBMMCByBMID", "select bmmc from xtgl_bmdm where bmid=:BMID", "根据部门id获取部门名称", false)]
    [Select("GetBMMCByBMDM", "select bmmc from xtgl_bmdm where sjbm=1 and bmdm=:BMDM", "根据部门dm获取sjbm=1的部门名称", false)]
    public class m_xtgl_bmdm
    {
        public long BMID { get; set; }
        public string BMMC { get; set; }
        public string BMDM { get; set; }
    }
}
