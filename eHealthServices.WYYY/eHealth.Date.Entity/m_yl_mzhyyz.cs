using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    /// <summary>
    /// 化验医嘱
    /// </summary>
    [Table("yl_mzhyyz","化验医嘱")]
    [Select("GetHYByZLHDID", "select z.YZID,z.CJSJ,z.HYXMMC,z.YZYHID,z.TXM,(select brxm from cw_khxx where brbh=z.brbh) xm from yl_mzhyyz z where z.zlhdid=:zlhdid order by z.hyxmid", "根据诊疗活动id获取化验医嘱")]
    [Select("GetHYByTXM", "select  sampleno TXM,receivetime CJSJ,examinaim HYXMMC,patientname XM,notes Result from L_PATIENTINFO,XTGL_BMDM,L_SAMPLETYPE where bqid=bmid(+) and L_PATIENTINFO.sampletype=l_sampletype.sampletype  and resultstatus>=4 and doctadviseno=:TXM ","根据条形码找到化验单",false)]
    public class m_yl_mzhyyz
    {
        public long ZLHDID { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public long YZID { get; set; }

        public DateTime CJSJ { get; set; }
        /// <summary>
        /// title
        /// </summary>
        public string HYXMMC { get; set; }

        public string XM { get; set; }

        public string YZYHID { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string TXM { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string Result { get; set; }
    }
}
