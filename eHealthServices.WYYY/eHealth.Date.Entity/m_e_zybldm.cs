using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("e_zybldm",  "病例代码表，用于根据代码获取病例内容表")]
    [Select("GetNrb", "select dm,nrb from e_zybldm where dm=:DM", "根据代码获取病例内容表", false)]
    public class m_e_zybldm
    {
        public string DM { get; set; }
        public string NRB { get; set; }
    }
}
