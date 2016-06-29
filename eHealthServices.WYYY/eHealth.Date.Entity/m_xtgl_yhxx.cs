using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("xtgl_yhxx", "用户信息")]
    [Select("GetYHXMByYHID", "select yhxm from xtgl_yhxx where yhid =:YHID", "根据用户id获取用户姓名", false)]
    public class m_xtgl_yhxx
    {
        public long YHID { get; set; }
        public string YHXM { get; set; }
    }
}
