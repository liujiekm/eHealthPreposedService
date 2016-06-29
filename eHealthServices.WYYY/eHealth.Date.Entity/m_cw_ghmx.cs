using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("cw_ghmx", "挂号明细")]
    [Select("GetByYYID", "select GHID from cw_ghmx where YYID=:YYID", "查看预约是否存在挂号记录", false)]
    [Update("UpdateYYIDByGHID", "YYID", "GHID=:GHID", "更新挂号信息为还未使用")]
    public class m_cw_ghmx
    {
        public long GHID { get; set; }
        public long YYID { get; set; }
    }
}
