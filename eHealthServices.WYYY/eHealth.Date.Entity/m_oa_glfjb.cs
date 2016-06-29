using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("OA_GLFJB", "关联附件表")]
    [Select("GetGLByGLID", "select FJID,WJGS from OA_GLFJB where glid=:GLID and gllb ='6' order by SXH ", "通过关联id获取附件信息")]
    [Select("GetGLByFJID", "select FJNR from OA_GLFJB where FJID=:FJID", "根据附件id获取附件", false)]
    public class m_oa_glfjb
    {
        public m_oa_glfjb()
        {
            this.FJID = string.Empty;
            this.FJNR = string.Empty;
            this.Url = string.Empty;
            this.WJGS = string.Empty;
        }
        public string FJID { get; set; }
        public string WJGS { get; set; }
        public long GLID { get; set; }
        public string FJNR { get; set; }

        /// <summary>
        /// 实际数据库不存在该字段
        /// </summary>
        public string Url { get; set; }
    }
}
