using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{

    #region request
    public class PhoneParam
    {
        public string PhoneNumber { get; set; }
    }
    public class Param
    {
        public string ID { get; set; }
    }

    public class ReportsParam
    {
        /// <summary>
        /// 就诊卡id
        /// </summary>
        public string CardID { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
    }
    public class ReportInfoParam : Param
    {

        public string TypeID { get; set; }
    }

    #endregion

    #region response

    #region 就诊卡
    public class Card
    {
        /// <summary>
        /// 就诊卡id
        /// </summary>
        public string HospitalCard { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        public string Address { get; set; }
    }

    public class ResponseCard : Models.BaseModel
    {
        /// <summary>
        /// 就诊卡列表
        /// </summary>
        public List<Card> Data { get; set; }
    }
    #endregion

    #region 消费记录
    public class Consumer
    {
        /// <summary>
        /// 消费项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 货币单位，元、美元等
        /// </summary>
        public string Monetary { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 消费时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    public class ConsumerRecords
    {
        /// <summary>
        /// 消费记录
        /// </summary>
        public List<Consumer> Consumers { get; set; }
        /// <summary>
        /// 其他备注信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 预存余额
        /// </summary>
        public decimal Balance { get; set; }
    }

    public class ResponseConsumerRecords : Models.BaseModel
    {
        /// <summary>
        /// 消费记录数据
        /// </summary>
        public ConsumerRecords Data { get; set; }
    }
    #endregion

    #region 就诊记录
    /// <summary>
    /// 简单的报告单
    /// </summary>
    public class DiagnosisRecordSimple
    {
        /// <summary>
        /// 诊疗记录id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string Dep { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 诊疗时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 诊断结果
        /// </summary>
        public string DiagnosisInfo { get; set; }
        /// <summary>
        /// 病史
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 体检
        /// </summary>
        public string Examination { get; set; }
        /// <summary>
        /// 化验
        /// </summary>
        public string Laboratory { get; set; }

    }

    public class ResponseDiagnosisRecordSimple : Models.BaseModel
    {
        /// <summary>
        /// 诊疗记录信息
        /// </summary>
        public List<DiagnosisRecordSimple> Data { get; set; }
    }
    public class Medicine
    {
        /// <summary>
        /// 药物医嘱id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 药名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用法
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 用量
        /// </summary>
        public string Dosage { get; set; }
        /// <summary>
        /// 剂量
        /// </summary>
        public string Dose { get; set; }
    }
    public class DiagnosisRecord : DiagnosisRecordSimple
    {
        /// <summary>
        /// 用药医嘱
        /// </summary>
        public List<Medicine> Medicines { get; set; }

        /// <summary>
        /// 报告单，化验单
        /// </summary>
        public List<Report> Reports { get; set; }

    }

    public class ResponseDiagnosisRecordInfo : Models.BaseModel
    {
        /// <summary>
        /// 诊疗记录详情
        /// </summary>
        public DiagnosisRecord Data { get; set; }


    }

    #endregion

    #region 报告单
    public class Report
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime ReportTime { get; set; }
        /// <summary>
        /// 报告者
        /// </summary>
        public string Reporter { get; set; }
        /// <summary>
        /// 报告类型id
        /// </summary>
        public string TypeID { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 检查标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 特检检查结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 特检结果诊断
        /// </summary>
        public string Diagnostic { get; set; }
        /// <summary>
        /// 化验结果
        /// </summary>
        public List<LaboratoryDetail> Details { get; set; }

    }
    public class LaboratoryDetail {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 项目结果值
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 结果值与正常值比较
        /// </summary>
        public string IsError { get; set; }
        /// <summary>
        /// 正常值
        /// </summary>
        public string Normal { get;set;}
    }
    public class ResponseReportInfo : Models.BaseModel
    {
        /// <summary>
        /// 报告单详情
        /// </summary>
        public List<Report> Data { get; set; }
    }
    #endregion

    #endregion

}