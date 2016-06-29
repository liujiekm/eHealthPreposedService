using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("cw_ycxfmx", "用户消费记录")]
    [Select("GetXFJL", "select xfje,sflx,sfsj from cw_ycxfmx where sfsj>sysdate-90 and brbh=:brbh order by sfsj desc", "根据用户健康卡号获取用户消费记录")]
    [Select("GetYHYE", "select ckje-xfje-mzdjje XFJE  from cw_ycje where brbh=:BRBH and ycdm='02'", "获取用户余额", false)]
    public class m_cw_ycxfmx
    {
        public string BRBH { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal XFJE { get; set; }
        public string SFLX { get; set; }
        public DateTime SFSJ { get; set; }
    }
}
