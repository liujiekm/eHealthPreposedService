using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("xtgl_ism_send", "mapp1", "发送短信提醒")]
    [Add("AddSend", "添加短信发送信息")]
    public class m_xtgl_ism_send
    {
        //[FieldSeq("seq_xtgl_ism_send_fsid")]
        public long FSID { get; set; }
        public int MBS { get; set; }
        public string XXLX { get; set; }
        public string XXLB { get; set; }
        public string XXNR { get; set; }
        public DateTime DSFSSJ { get; set; }
        public int ZTBZ { get; set; }
        public DateTime BJSJ { get; set; }
        public long BJYHID { get; set; }
        public DateTime SHSJ { get; set; }
        public long SHBMID { get; set; }
        public long SHYHID { get; set; }
        public string BZ { get; set; }
    }
}
