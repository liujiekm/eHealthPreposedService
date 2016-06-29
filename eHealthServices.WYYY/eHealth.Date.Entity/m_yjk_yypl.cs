using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yjk_yypl", "mapp1", "用药频率")]
    [Select("GetMCByDM", "select mc from yjk_yypl where dm=:dm", "根据代码获取名称", false)]
    public class m_yjk_yypl
    {
        public string DM { get; set; }
        public string MC { get; set; }
    }
}
