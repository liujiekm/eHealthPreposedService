using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yjk_ddlbn", "mapp1", "用药时间")]
    [Select("GetMCByDM", "select mc from yjk_ddlbn where dm=:dm", "根据代码获取名称", false)]
    public class m_yjk_ddlbn
    {
        public string DM { get; set; }
        public string MC { get; set; }
    }
}
