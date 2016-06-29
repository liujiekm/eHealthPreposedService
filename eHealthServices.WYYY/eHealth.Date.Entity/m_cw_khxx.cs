using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lenovo.CodeBuild.Attribute;

namespace eHealth.Date.Entity
{
    [Table("cw_khxx", "客户信息")]
    [Select("GetKHXXBySJH", "select brbh,brxm,brxb,sfzh,csrq,lxdz,lxdh,yddh,szsj,xgsj from cw_khxx where ztbz='1' and (lxdh=:LXDH or yddh=:YDDH)", "根据手机号获取客户信息")]
    [Select("GetKHXXByBRBH", "select brbh,dwdm,knsj,brxm,brxb,gjdm,hyzk,zydm,jgdm,mzdm,sfzh,csrq,py,wb,xzzdm,lxdz,lxdh,yddh,szsj,ztbz,czzid,xgsj from cw_khxx where brbh = :brbh", "根据病人编号获取病人卡号信息",false)]
    public class m_cw_khxx
    {
        public string BRBH { get; set; }
        public string BRXM { get; set; }
        public string SFZH { get; set; }
        public string LXDH { get; set; }
        public string YDDH { get; set; }
        public string LXDZ { get; set; }
        public DateTime? SZSJ { get; set; }
        public DateTime? XGSJ { get; set; }
        public DateTime? CSRQ { get; set; }
        public string BRXB { get; set; }
    }
}
