using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("xtgl_ism_sendmx", "mapp1", "发送短信提醒")]
    [Add("AddMX", "添加短信明细信息")]
    public class m_xtgl_ism_sendmx
    {
        // [FieldSeq("seq_xtgl_ism_send_fsid")]
        public long FSID { get; set; }
        public DateTime FSSJ { get; set; }
        public long FSYHID { get; set; }
        public string SJHM { get; set; }
        public decimal TXFY { get; set; }
        public DateTime CJSJ { get; set; }
        public string BZ { get; set; }
    }
}
