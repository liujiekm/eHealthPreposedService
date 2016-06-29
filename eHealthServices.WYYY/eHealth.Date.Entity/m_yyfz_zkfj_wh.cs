using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("yyfz_zkfj_wh", "部门信息，限制查询层次，只对外显示cxbz='1'的部门")]
    [Select("GetByDM", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where DM=:BMID and cxbz='1' and ztbz='1'", "根据代码获取信息", true)]
    [Select("GetAllByFDM", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM=:FDM  and ztbz='1'", "根据父代码获取信息", true)]
    [Select("GetByMC", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where MC=:MC  and ztbz='1'", "根据名称获取信息", true)]
    [Select("GetByFDM", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where DM=:FDM and cxbz='1'  and ztbz='1'", "根据父代码获取信息", true)]
    [Select("GetFirstFDM", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM='1' and cxbz='1' and ztbz='1'", "根据获取一级分类信息", true)]
    [Select("GetSecendBM", "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM>2 and cxbz='1' and ztbz='1'", "根据获取所有二级级部门信息", true)]
    [Select("GetAllBMID", "select bmid from yyfz_zkfj_wh where bmid is not null", "获取所有的部门id")]
    [Select("GetMCByID", "select mc,FDM,cxbz from yyfz_zkfj_wh where bmid=:BMID", "获取科室名称根据部门id", false)]
    [Select("GetDMByMC", "select  dm from yyfz_zkfj_wh where cxbz='1' and ztbz='1' and mc=:MC", "根据名称获取代码", false)]
    public class m_yyfz_zkfj_wh
    {
        public int? BMID { get; set; }
        public string FDM { get; set; }
        public string MC { get; set; }
        public string DM { get; set; }
        public string CXBZ { get; set; }
    }
}
