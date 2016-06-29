using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_zkyysz", "获取是否自动签到")]
    [Select("GetSFQD", "select sfqd from yyfz_zkyysz where zkid=:ZKID", "", false)]
    public class m_yyfz_zkyysz
    {
        public string SFQD { get; set; }
        public long ZKID { get; set; }
    }
}
