using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_fjxx", "mapp1", "房间信息")]
    [Select("GetAll", "select FJID,FJWZ,FJMC,ZTBZ from yyfz_fjxx where ZTBZ=1", "获取所有房间信息，状态标志可用")]
    public class m_yyfz_fjxx
    {

        public int FJID { get; set; }

        public string FJWZ { get; set; }
        public string FJMC { get; set; }
        public string ZTBZ { get; set; }
    }
}
