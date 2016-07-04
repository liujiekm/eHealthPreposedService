//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 类库说明
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/4 15:37:36
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.WYServiceImplement.Model
{
    public class m_getreport
    {
        private string _JCH;//检查号
        private string _BRBH;//病人编号
        private string _BRXM;//病人姓名
        private string _BRXB;//病人性别
        private DateTime? _CSRQ;//出生日期
        private string _TJBH;//体验编号
        private string _ZYH;//住院号
        private string _BQID;//病区ID
        private string _CWH;//床位号
        private string _LXDZ;//联系地址
        private DateTime? _SQRQ;//申请日期
        private string _JCFF;//检查方法
        private Int64? _SQZKID;//申请专科ID
        private string _SQZK;//申请专科
        private Int64? _SQYSID;//申请医生ID
        private string _SQYS;//申请医生
        private string _JCBW;//检查部位
        private string _SMCS;//扫描参数
        private string _JGSJ;//结果所见
        private string _JGZD;//结果诊断
        private DateTime? _BGSJ;//报告时间
        private Int64? _BGRYID;//报告人员ID
        private string _BGRY;//报告人员
        private DateTime? _SHSJ;//审核时间
        private Int64? _SHRYID;//审核人员ID
        private string _SHRY;//审核人员
        private Int64? _DYCS;//打印次数


        /// <summary>
        /// 检查号
        /// </summary>
        public string JCH
        {
            get { return _JCH; }
            set { _JCH = value; }
        }

        /// <summary>
        /// 病人编号
        /// </summary>
        public string BRBH
        {
            get { return _BRBH; }
            set { _BRBH = value; }
        }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string BRXM
        {
            get { return _BRXM; }
            set { _BRXM = value; }
        }


        /// <summary>
        /// 病人性别
        /// </summary>
        public string BRXB
        {
            get { return _BRXB; }
            set { _BRXB = value; }
        }


        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? CSRQ
        {
            get { return _CSRQ; }
            set { _CSRQ = value; }
        }


        /// <summary>
        /// 体验编号
        /// </summary>
        public string TJBH
        {
            get { return _TJBH; }
            set { _TJBH = value; }
        }


        /// <summary>
        /// 住院号
        /// </summary>
        public string ZYH
        {
            get { return _ZYH; }
            set { _ZYH = value; }
        }


        /// <summary>
        /// 病区ID
        /// </summary>
        public string BQID
        {
            get { return _BQID; }
            set { _BQID = value; }
        }


        /// <summary>
        /// 床位号
        /// </summary>
        public string CWH
        {
            get { return _CWH; }
            set { _CWH = value; }
        }


        /// <summary>
        /// 联系地址
        /// </summary>
        public string LXDZ
        {
            get { return _LXDZ; }
            set { _LXDZ = value; }
        }


        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime? SQRQ
        {
            get { return _SQRQ; }
            set { _SQRQ = value; }
        }



        /// <summary>
        /// 检查方法
        /// </summary>
        public string JCFF
        {
            get { return _JCFF; }
            set { _JCFF = value; }
        }


        /// <summary>
        /// 申请专科ID
        /// </summary>
        public Int64? SQZKID
        {
            get { return _SQZKID; }
            set { _SQZKID = value; }
        }


        /// <summary>
        /// 申请专科
        /// </summary>
        public string SQZK
        {
            get { return _SQZK; }
            set { _SQZK = value; }
        }


        /// <summary>
        /// 申请医生ID
        /// </summary>
        public Int64? SQYSID
        {
            get { return _SQYSID; }
            set { _SQYSID = value; }
        }


        /// <summary>
        /// 申请医生
        /// </summary>
        public string SQYS
        {
            get { return _SQYS; }
            set { _SQYS = value; }
        }


        /// <summary>
        /// 检查部位
        /// </summary>
        public string JCBW
        {
            get { return _JCBW; }
            set { _JCBW = value; }
        }


        /// <summary>
        /// 扫描参数
        /// </summary>
        public string SMCS
        {
            get { return _SMCS; }
            set { _SMCS = value; }
        }



        /// <summary>
        /// 结果所见
        /// </summary>
        public string JGSJ
        {
            get { return _JGSJ; }
            set { _JGSJ = value; }
        }



        /// <summary>
        /// 结果诊断
        /// </summary>
        public string JGZD
        {
            get { return _JGZD; }
            set { _JGZD = value; }
        }



        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime? BGSJ
        {
            get { return _BGSJ; }
            set { _BGSJ = value; }
        }


        /// <summary>
        /// 报告人员ID
        /// </summary>
        public Int64? BGRYID
        {
            get { return _BGRYID; }
            set { _BGRYID = value; }
        }


        /// <summary>
        /// 报告人员
        /// </summary>
        public string BGRY
        {
            get { return _BGRY; }
            set { _BGRY = value; }
        }


        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? SHSJ
        {
            get { return _SHSJ; }
            set { _SHSJ = value; }
        }


        /// <summary>
        /// 审核时间ID
        /// </summary>
        public Int64? SHRYID
        {
            get { return _SHRYID; }
            set { _SHRYID = value; }
        }


        /// <summary>
        /// 审核人员
        /// </summary>
        public string SHRY
        {
            get { return _SHRY; }
            set { _SHRY = value; }
        }


        /// <summary>
        /// 打印次数
        /// </summary>
        public Int64? DYCS
        {
            get { return _DYCS; }
            set { _DYCS = value; }
        }
    }
}
