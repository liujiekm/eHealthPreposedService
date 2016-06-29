using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("xtgl_ddlbn", "部门代码与部门名称对应表")]
    [Select("GetYQMC", "select mc from xtgl_ddlbn where lb='0014' and dm=:DM", "根据代码获取lb='0005'的院区名称", false)]
    [Select("GetZBYYByDM", "select mc from yyfz_ddlbn where lb='FZ15' and dm=:DM", "获取专病预约名称", false)]
    [Select("GetByLBDM", "select mc from xtgl_ddlbn where lb = :lb and dm = :dm and ztbz='1'", "获取指定类别和代码的名称", false)]
    public class m_xtgl_ddlbn
    {
        public string MC { get; set; }
        public string LB { get; set; }
        public string DM { get; set; }
    }
}
