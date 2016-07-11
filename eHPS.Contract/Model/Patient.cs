//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者基本信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/7 18:21:01
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{
    public class Patient
    {

        //public string BRBH { get; set; }
        /// <summary>
        /// 患者标识
        /// </summary>
        public string PatientId { get; set; }


        //public string BRXM { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }


        //public string SFZH { get; set; }
        /// <summary>
        /// 患者身份证号
        /// </summary>
        public string IdCode { get; set; }


        //public string LXDH { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }




        //public string YDDH { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        //public string LXDZ { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }


        /// <summary>
        /// 首次诊疗时间
        /// </summary>
        public DateTime? FirstTimeDiagnosis { get; set; }





        //public DateTime? CSRQ { get; set; }



        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BornDate { get; set; }


        //public string BRXB { get; set; }


        /// <summary>
        /// 患者性别 1 男 2 女
        /// </summary>
        public string Sex { get; set; }
    }
}
