using eHealth.Date.DAL;
using eHealth.Service;
using eHealthServices.WYYY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace eHealthServices.WYYY.Controllers
{
    public partial class HospitalController : ApiController
    {
        /// <summary>
        ///
        /// 获取绑定的就诊卡信息，可根据手机号码，或其他用户信息
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseCard GetCards(PatientInfo param)
        { //根据手机号码获取该手机号绑定的就诊卡信息
             //param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseCard() { HasError = 1, ErrorMessage = "参数不正确！" };
            if (param.PhoneNumber.Contains(",")) param.PhoneNumber=param.PhoneNumber.Split(',')[0];
            try
            {
                //数据查询
                var list = new dl_cw_khxx().GetKHXXBySJH(param.PhoneNumber, param.PhoneNumber);
                //数据实体转换
                var result = new ResponseCard();
                var data = new List<Card>();
                foreach (var card in list)
                {
                    data.Add(new Card()
                    {
                        HospitalCard = card.BRBH,
                        CreateTime = card.XGSJ.HasValue ? card.XGSJ.Value : card.SZSJ.Value,
                        IDCard = card.SFZH,
                        PhoneNumber = param.PhoneNumber,
                        Name = card.BRXM,
                        Address = card.LXDZ
                    });
                }
                result.Data = data.FindAll(a=>a.IDCard==param.IDCard&&a.PhoneNumber==param.PhoneNumber&&a.Name==param.Name);
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseCard() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 模块：消费记录
        /// 获取病人的消费记录，可根据就诊卡号，或其他用户信息
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseConsumerRecords ConsumerRecords(PatientInfo param)
        { //根据就诊卡号获取病人消费记录
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseConsumerRecords() { HasError = 1, ErrorMessage = "参数有误" };
            try
            {
                //数据查询
                //消费记录
                var temp = new dl_cw_ycxfmx().GetXFJL(param.HospitalCard);
                //余额查询
                var yhye = new dl_cw_ycxfmx().GetYHYE(param.HospitalCard);
                //数据实体转换
                var list = temp.Select(a => new
                {
                    BRBH = a.BRBH,
                    SFSJ = a.SFSJ,
                    XFJE = a.XFJE,
                    SFLX = ((a.SFLX == "1") ? "挂号" : ((a.SFLX == "2") ? "门诊收费" : "体检收费"))
                });
                var consumers = new List<Consumer>();
                foreach (var consumer in list)
                {
                    consumers.Add(
                        new Consumer()
                        {
                            CreateTime = consumer.SFSJ,
                            Monetary = "元",
                            Money = consumer.XFJE,
                            Name = consumer.SFLX
                        }
                    );
                }
                var result = new ResponseConsumerRecords()
                {
                    Data = new ConsumerRecords()
                    {
                        Msg = "三个月内的消费记录",
                        UserName = param.Name,
                        Consumers = consumers,
                        Balance = yhye.XFJE
                    }
                };
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseConsumerRecords() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        #region 6.诊疗记录
        /// <summary>
        /// 模块：诊疗记录
        /// 获取就诊记录列表，可根据就诊卡号，或其他用户信息
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseDiagnosisRecordSimple DiagnosisRecords(PatientInfo param)
        {//根据就诊卡id获取就诊记录
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseDiagnosisRecordSimple() { HasError = 1, ErrorMessage = "参数有误！" };
            if(string.IsNullOrWhiteSpace(param.HospitalCard))
                return new ResponseDiagnosisRecordSimple() { HasError = 1, ErrorMessage = "就诊卡号不能为空！" };
            try
            {
               //数据获取
                var list = new dl_zljl().GetZLJLByBRBH(param.HospitalCard);
                //实体转换
                var result = new ResponseDiagnosisRecordSimple();
                var data = new List<DiagnosisRecordSimple>();
                foreach (var zljl in list)
                {
                    data.Add(new DiagnosisRecordSimple()
                    {
                        CreateTime = zljl.ZDSJ,
                        Dep = zljl.KSMC,
                        DiagnosisInfo = zljl.ZDJG,
                        DoctorName = zljl.YSXM,
                        Examination = zljl.TJ,
                        ID = zljl.HDID,
                        Laboratory = zljl.HYTJ,
                        MedicalHistory = zljl.BS
                    });
                }
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseDiagnosisRecordSimple() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 模块：诊疗记录
        /// 获取诊疗详情，包括各项医嘱，可根据诊疗id，或其他用户信息，传递的id：KeyID 为诊疗id
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="KeyID">诊疗id</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseDiagnosisRecordInfo DiagnosisRecordInfo(PatientInfo param)
        {//根据就诊卡获取诊疗记录详情
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseDiagnosisRecordInfo() { HasError = 1, ErrorMessage = "参数有误！" };
            try
            {
                long id = Convert.ToInt64(param.KeyID);//诊疗活动id
                var list = new MedicinesYZ().GetYPYZByZLHDID(id);//药物医嘱
                var zljl = new dl_zljl().GetZLJLByID(id);
                var Medicines = new List<Medicine>();
                foreach (var item in list)
                {
                    Medicines.Add(new Medicine() { Dosage = "每次"+item.YCYL+item.JLDW, Dose=item.JL+"*"+item.BZL+"/"+item.DW, Name=item.MC, Usage=item.YFYL });
                }

                #region 报告单
                List<Report> reports = new List<Report>();//报告单
                var tj = new dl_yl_sqdsj().GetTJByZLHDID(id);
                if (tj != null&&tj.Count>0)
                    reports = tj.Select(a => new Report
                    {
                        Diagnostic = a.TJ,
                        ID = a.SQDSJID.ToString(),
                        Reporter = a.YZYHID.ToString(),
                        ReportTime = a.SCLRSJ,
                        Result = a.TJ,
                        Type = "特检",
                        TypeID = "JC",
                        Title = a.MC,
                        UserName = a.XM
                    }).ToList();
                var hys = new dl_yl_mzhyyz().GetHYByZLHDID(id);
                var dl_test = new dl_l_testresult();  
                if (hys != null && hys.Count > 0)
                    reports.AddRange(hys.Select(a => new Report()
                    {
                        ID = a.YZID.ToString(),
                        ReportTime = a.CJSJ,
                        Reporter = a.YZYHID,
                        Title = a.HYXMMC,
                        UserName = a.XM,
                        Type = "化验",
                        TypeID = "JY",
                        Details = dl_test.GetListByTXM(a.TXM).Select(b => new LaboratoryDetail()
                        {
                            Result = b.TESTRESULT + b.UNIT,
                            ItemName = b.CHINESENAME,
                            Normal = b.CKFW
                        }).ToList()
                    }));
                #endregion

               
                var result = new ResponseDiagnosisRecordInfo()
                    {
                        Data = new DiagnosisRecord()
                        {
                            CreateTime = zljl.ZDSJ,
                            Dep = zljl.KSMC,
                            DiagnosisInfo = zljl.ZDJG,
                            DoctorName = zljl.YSXM,
                            Examination = zljl.TJ,
                            ID = zljl.HDID,
                            Laboratory = zljl.HYTJ,
                            MedicalHistory = zljl.BS,
                            Medicines = Medicines,
                            Reports=reports.OrderByDescending(a=>a.ReportTime).ToList()
                        }
                    };
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseDiagnosisRecordInfo() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        #endregion

        #region 4、报告单
        ///// <summary>
        ///// 4、1、【消息方法】
        ///// 获取患者的报告单
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ResponseSimpleReport Reports( )
        //{ //根据用户就诊卡id，身份证，手机号，姓名 获取报告单
        //    var param = ConverToT<ReportsParam>();
        //    if (param == null)
        //        return new ResponseSimpleReport() { HasError = 1, ErrorMessage = "参数不正确！" };
        //    try
        //    {
        //        //数据查询
        //        var list = new dl_getreport().GetBGList(param.CardID);
        //        //实体转换
        //        var result = new ResponseSimpleReport();
        //        var data = new List<SimpleReport>();
        //        foreach (var report in list)
        //        {
        //            data.Add(new SimpleReport()
        //            {
        //                ID = report.BGBS,
        //                ReportTime = report.BGSJ,
        //                Title = report.JYMC,
        //                Type = report.JCLX,
        //                TypeID = report.JCLX1,
        //                UserName = report.BRXM
        //            });
        //        }
        //        result.Data = data;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Lenovo.Tool.Log4NetHelper.Error(ex);
        //        return new ResponseSimpleReport() { HasError = 1, ErrorMessage = ex.Message };
        //    }
        //}
        /// <summary>
        /// 模块：取报告单
        /// 扫描二维码获取报告单
        /// </summary>
        /// <param name="param">id为条形码值</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseReportInfo ScanQRCode(Param param)
        {
            // param = ConverToT<Param>();
            if (param == null || string.IsNullOrWhiteSpace(param.ID))
                return new ResponseReportInfo() { HasError = 1, ErrorMessage = "参数不正确！" };
            try
            {
                var qr = new dl_yl_mzhyyz().GetHYByTXM(param.ID);
                if (qr == null)
                    return new ResponseReportInfo() { HasError = 1, ErrorMessage = "该条形码下没有报告单！" };
                var result = new ResponseReportInfo()
                {
                    Data = new List<Report>() { 
                        new Report(){
                          Diagnostic=qr.Result,
                           ID=param.ID,
                            ReportTime=qr.CJSJ,
                             Result=qr.Result,
                              Title=qr.HYXMMC,
                               Type="化验",
                                TypeID="JY",
                                 UserName=qr.XM,
                                  Details=new dl_l_testresult().GetListByTXM(param.ID).Select(b => new LaboratoryDetail()
                            {
                                Result = b.TESTRESULT + b.UNIT,
                                ItemName = b.CHINESENAME,
                                Normal = b.CKFW
                            }).ToList()
                        }
                    }
                };
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseReportInfo() { HasError = 1, ErrorMessage = "温医一院查询出错！" };
            }
        }


        /// <summary>
        /// 模块：我的检查
        /// 获取用户在医院的所有报告单详情，可根据就诊卡号，或其他用户信息
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseReportInfo ReportInfo(PatientInfo param)
        { 
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseReportInfo() { HasError = 1, ErrorMessage = "参数不正确！" };
            var result = new ResponseReportInfo();
            var data = new List<Report>();
            try
            {
                var list = new dl_getreport().GetBGList(param.HospitalCard);
                foreach (var item in list)
                {
                    //数据查询
                    var report = new dl_getreport().Get_Report(item.BGBS, item.JCLX1);
                    //实体转换
                    if (report != null && report.Count > 0)
                        //添加化验单详情
                        if (item.JCLX1.ToUpper() == "JY")
                        {
                            var assayList = new List<LaboratoryDetail>();
                            foreach (var ass in report)
                            {
                                var project = ass.JGSJ.Split('&');
                                var assay = new LaboratoryDetail() { ItemName = project[0], Result = project[1], IsError = project[2], Normal = project[3].Trim()};
                                assayList.Add(assay);
                            }
                            var dataReport = new Report()
                            {
                                ID = item.BGBS,
                                ReportTime = report[0].BGSJ.HasValue ? report[0].BGSJ.Value : DateTime.Now,
                                Title = report[0].JCBW,
                                Type = "化验",
                                TypeID = item.JCLX1,
                                UserName = report[0].BRXM,
                                Details = assayList,
                                Reporter = report[0].BGRY
                            };
                            data.Add(dataReport);
                        }
                        else  //添加特检单详情
                        {
                            var temp = report[0];
                            temp.JGSJ = Lenovo.Tool.Html.FormatHtml.NoHTML(temp.JGSJ);
                            temp.JGZD = Lenovo.Tool.Html.FormatHtml.NoHTML(temp.JGZD);

                            var dataReport = new Report()
                            {
                                Result = temp.JGSJ,
                                Diagnostic = temp.JGZD,
                                ID =item.BGBS,
                                ReportTime = temp.BGSJ.HasValue ? temp.BGSJ.Value : DateTime.Now,
                                Title = temp.JCBW,
                                Type = "特检",
                                TypeID = item.JCLX1,
                                UserName = temp.BRXM,
                                Reporter = report[0].BGRY
                            };
                            data.Add(dataReport);
                        }
                }
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseReportInfo() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        #endregion
    }
}

