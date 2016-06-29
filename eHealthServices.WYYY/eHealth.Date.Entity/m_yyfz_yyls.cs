using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_yyls", "预约流水记录")]
    [Add]
    [Delete("DelYYLS", "fzyyid = :FZYYID", "根据预约id删除预约流水记录")]
    public class m_yyfz_yyls
    {
        public string BRBH { get; set; }
        public long FZYYID { get; set; }
        public DateTime YYFSSJ { get; set; }
        public DateTime? YYJZSJ { get; set; }
        public string YSXM { get; set; }
        public string ZTBZ { get; set; }
    }
}
