using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yjk_yyff", "mapp1", "用药方法")]
    [Select("GetMCByDM", "select mc from yjk_yyff where dm=:dm", "根据代码获取名称", false)]
    public class m_yjk_yyff
    {
        public string DM { get; set; }
        public string MC { get; set; }
    }
}
