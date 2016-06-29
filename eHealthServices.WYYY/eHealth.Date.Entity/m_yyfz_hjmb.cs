using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_hjmb", "短信模版内容")]
    [Select("GetZBMBNR", "select mbnr from yyfz_hjmb ,yyfz_gxdy where yyfz_hjmb.fzzbh = '000' and yyfz_hjmb.dm = yyfz_gxdy.zd2 and yyfz_gxdy.lb = 'DXTX'and fjz='预约' and zd1=:ZBYY", "获取专病预约模版内容", false)]
    [Select("GetYYMBNR", "select mbnr from yyfz_hjmb where fzzbh = '000' and dm = 'ZK_YY'", "获取短信预约模版", false)]
    public class m_yyfz_hjmb
    {
        public string MBNR { get; set; }
        public string ZBYY { get; set; }
    }
}
