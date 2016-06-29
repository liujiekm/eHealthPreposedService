using Lenovo.CodeBuild.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.Entity
{
    [Table("r_ryk", "人员库信息")]
    [Select("GetXBByID", "select xb from r_ryk where id=:id", "根据人员库id获取用户性别", false)]
    [Select("GetDoctorByBMID", "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid=r.id and r.id in (select distinct rykid from yyfz_yspb where ztbz='1' and zllx<>'04' and zllx<>'15' and sbsj>sysdate-100 and zkid=:zkid and zbyy is null )", "根据部门id获取科室下的所有医生的信息")]
    [Select("GetDoctorByHZ", "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid=r.id and r.id in (select distinct rykid from yyfz_yspb where ztbz='1' and zllx<>'04' and zllx<>'15' and sbsj>sysdate-100 and zbyy is null ) and (r.py like :XM  or r.xm like :XB)", "根据搜索内容获取所有医生的信息")]
    [Select("GetDoctorByRykid", "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid(+)=r.id and r.id =:ID", "根据rykid获取医生的信息",false)]
    [Select("GetZKByRykid", "select xzkid zkid,mc from yl_ryk r,yyfz_zkfj_wh d where r.xzkid=d.bmid and r.rykid=:ID", "根据rykid获取医生专科信息",false)]
    [Select("GetDoctorByYHID", "select xm,rykid id,xzkid zkid from yl_ryk where yhid=:YSYHID", "获取医生姓名，专科，人员库id，通过医生用户id", false)]
    public class m_r_ryk
    {
        /// <summary>
        /// 人员库id
        /// </summary>
        public long ID { get; set; }
        public long YSYHID { get; set; }
        public string XB { get; set; }
        public string XM { get; set; }

        public byte[] RYPIC { get; set; }

        public int ZKID { get; set; }
        public string JSNR { get; set; }
        public string ZW { get; set; }
        public string URL { get; set; }
        public string GZDM { get; set; }
        public string GZDM2 { get; set; }
    }
}
