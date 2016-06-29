using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("l_testresult","化验结果表")]
    [Select("GetListByTXM", "SELECT L_TESTDESCRIBE.IMPORTANT,L_TESTDESCRIBE.CHINESENAME,L_TESTRESULT.UNIT, L_TESTRESULT.HINT,lpad(reflo,7,' ')||decode(nvl(length(refhi),0),0,'','～')||rpad(nvl(refhi,' '),decode(nvl(length(refhi),0),0,5 - length(reflo),7),' ') ckfw,L_TESTRESULT.TESTRESULT,L_TESTRESULT.TESTID FROM L_TESTDESCRIBE,L_TESTRESULT WHERE L_TESTDESCRIBE.TESTID = L_TESTRESULT.TESTID and L_TESTRESULT.SAMPLENO  in (SELECT sampleno from L_PATIENTINFO where  doctadviseno =:TXM)  AND    L_TESTRESULT.ISPRINT = 1 and L_TESTRESULT.Teststatus >=4 ORDER BY L_TESTDESCRIBE.PRINTORD ASC ", "根据条形码获取数据")]
    public class m_l_testresult
    {
        /// <summary>
        /// 重要
        /// </summary>
        public string IMPORTANT { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string CHINESENAME { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UNIT { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string HINT { get; set; }
        /// <summary>
        /// 正常值
        /// </summary>
        public string CKFW { get; set; }
        /// <summary>
        /// 测试结果
        /// </summary>
        public string TESTRESULT { get; set; }
        /// <summary>
        /// 测试id
        /// </summary>
        public string TESTID { get; set; }

        public string TXM { get; set; }
    }
}
