using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("i_ysxx", "医生信息")]
    [Select("GetDoctorsByKS", "select ksmc,id,xm,jj,zc,yszp,ksmc1,xh,xh1 from i_ysxx where  shzt='已审核' and flag=1 and (ksmc in (:ksmc) or ksmc1 in (:ksmc1))", "根据科室获取医生")]
    [Select("GetDoctorByID", "select id,yszp,xm,ksmc,ksmc1 from i_ysxx where id=:id", "根据ID获取医生信息", false)]
    [Select("GetDoctorPicByName", "select id,yszp from i_ysxx where xm=:xm and shzt='已审核' and xm not in ('卢才教','何金彩','褚茂平','周颖') ", "根据医生姓名获取医生照片", false)]
    [Select("GetDocInfoByID", "select zc,mzsj,xm,jj from i_ysxx where id=:id", "根据ID获取医生简介", false)]
    public class m_i_ysxx
    {
        public string KSMC { get; set; }
        public int ID { get; set; }
        public string XM { get; set; }
        public string JJ { get; set; }
        public string ZC { get; set; }
        public string YSZP { get; set; }
        public string KSMC1 { get; set; }
        public int XH { get; set; }
        public int XH1 { get; set; }
        public string MZSJ { get; set; }
    }
}
