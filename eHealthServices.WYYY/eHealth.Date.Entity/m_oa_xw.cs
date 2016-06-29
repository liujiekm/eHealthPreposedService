using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("oa_xw", "新闻")]
    [Select("GetOAXWByID", "select BT,ZZ,TJSJ,WZNR from oa_xw where ID=:id", "根据新闻id获取新闻", false)]
    //[SelectPaging("GetXWPage", "ID,BT,ZZ,TJSJ", "TJSJ", " yxbz ='Y'  and ( xwlbid like  '1蠀%' or xwlbid like'蠁1蠀' ) and TGZT in ('5','7','8') and NWXS like '%1'", " 获取新闻 分页")]
    public class m_oa_xw
    {
        public m_oa_xw()
        {
            this.BT = string.Empty;
            this.ZZ = string.Empty;
            this.WZNR = string.Empty;
        }
        public long ID { get; set; }
        public string BT { get; set; }
        public string ZZ { get; set; }
        public DateTime TJSJ { get; set; }
        public string WZNR { get; set; }
        public long XWCNT { get; set; }
    }
}
