//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 诊疗服务，温附一实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/5 10:32:18
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;

using Dapper;
using Oracle.ManagedDataAccess.Client;
using eHPS.Common;

namespace eHPS.WYServiceImplement
{
    public class DiagnosisService : IDiagnosis
    {
        /// <summary>
        /// 获取诊疗记录
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public List<DiagnosisRecord> GetDiagnosisHistory(string patientId)
        {
            var result = new List<DiagnosisRecord>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select y.zlhdid,y.sczlhdid,y.zllx,b.bmmc,h.yhxm,y.kssj from yl_zlhd y,xtgl_bmdm b,xtgl_yhxx h  
                                            where  zlzt = '1' and  b.bmid=y.jzzkid and y.jzysyhid=h.yhid and y.brbh = :PatientId and zlzt = '1' order by kssj desc";

                var condition = new { PatientId =patientId};

                var records = con.Query(command, condition).ToList();

                foreach (var record in records)
                {
                   
                    var diagnosisActivityId = ((long)record.ZLHDID).ToString();
                    var diagnosisTime = (DateTime)record.KSSJ;

                    var indicateId = GetIndicate(diagnosisActivityId,con);

                    var diagnosisRecord = new DiagnosisRecord {
                        DiagnosisTime = diagnosisTime,
                        DeptName = (string)record.BMMC,
                        DiagnosisInfo = GetDiagnostics(diagnosisActivityId,con),
                        DoctorName = (string)record.YHXM,
                        Id = diagnosisActivityId,
                        Examination = GetMedicalRecord(indicateId.ToString(), "010002",con),
                        Laboratory= GetMedicalRecord(indicateId.ToString(), "010003",con),
                        MedicalHistory= GetMedicalRecord(indicateId.ToString(), "010001",con)
                    };
                    result.Add(diagnosisRecord);

                }

            }


            return result;
        }


        /// <summary>
        /// 获得病历内容
        /// </summary>
        /// <param name="medicalRecordId">病历标识</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        private string GetMedicalRecord(string medicalRecordId,string code,OracleConnection con)
        {
            
            var command = @"select nrb from e_zybldm where dm=:Code";
            var condition = new { Code = code };
            var result = con.Query(command, condition).FirstOrDefault();

            command = @"select JG from " + (string)result.NRB + " where dm='" + code + "' and id=" + medicalRecordId;

            var medicalContent = con.Query(command).FirstOrDefault();

            

            if(null!=medicalContent)
            {
                return (string)medicalContent.JG;
            }
            else
            {
                return "";
            }

            
        }



        /// <summary>
        /// 根据病历代码获取病历内容表
        /// </summary>
        /// <param name="code">病历代码</param>
        /// <returns></returns>
        //private string GetContentTable(string code)
        //{
        //    using (var con = DapperFactory.CrateOracleConnection())
        //    {
        //        var command = @"select nrb from e_zybldm where dm=:Code";
        //        var condition = new { Code=code};
        //        var result = con.Query(command, condition).FirstOrDefault();

        //        return (string)result;
        //    }
        //}


        /// <summary>
        /// 获得病历标识信息，病历标识信息可以查询病史、体检、化验数据
        /// </summary>
        /// <param name="diagnosisActivityId"></param>
        /// <returns></returns>
        private Int64 GetIndicate(string diagnosisActivityId, OracleConnection con)
        {

           
            var command = @"select jlid from yl_zljl a,xtgl_zjsbg b,yl_zlhd c where a.gsdm = b.gsdm and a.zlhdid = c.zlhdid and (b.zkid = c.jzzkid or b.zkid=0) and a.zlhdid = :DiagnosisActivityId";
            var condition = new { DiagnosisActivityId = diagnosisActivityId };

            var result = con.Query(command, condition).FirstOrDefault();

            

            if(null!=result)
            {
                return (long)result.JLID;
            }
            else
            {
                return 0;
            }

           
        }



        /// <summary>
        /// 获取诊断
        /// </summary>
        /// <param name="diagnosisActivityId">诊疗活动标识</param>
        /// <returns>
        /// (前置)诊断(后置),(前置)诊断(后置) 格式
        /// </returns>
        private string GetDiagnostics(string diagnosisActivityId, OracleConnection con)
        {
            
                var command = @"select qz,lczd,hz  from yl_zlzd where zlhdid=:DiagnosisActivityId order by zdxh";
                var condition =new  { DiagnosisActivityId = diagnosisActivityId };

                var result = con.Query(command, condition).ToList();

                var diagnostics =new StringBuilder();
                foreach (var item in result)
                {
                    var before = string.IsNullOrEmpty((string)item.qz) ? "" : "(" + (string)item.qz + ")";
                    var after = string.IsNullOrEmpty((string)item.hz) ? "" : "(" + (string)item.qz + ")";
                    diagnostics.Append(before + (string)item.lczd + after+",");
                }

                return diagnostics.ToString();
            
        }




        /// <summary>
        /// 获取诊疗标题
        /// </summary>
        /// <param name="diagnosisType">诊疗类型</param>
        /// <param name="firstDiagnosisActivityId">首次诊疗活动标识</param>
        /// <returns></returns>
        private string GetDiagnosisTitle(string diagnosisType,string firstDiagnosisActivityId)
        {
            
             var multiValue=   CommonService.GetValue("0051", diagnosisType);

            if (string.IsNullOrEmpty(multiValue.MC))
            {
                if (string.IsNullOrEmpty(firstDiagnosisActivityId))
                {
                    return "首次诊疗";
                }
                else
                {
                    return "诊疗复查";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(firstDiagnosisActivityId))
                {
                    return "首次" + multiValue.MC;
                }
                else
                {
                    return multiValue.MC + "复查";
                }
            }
        }




        /// <summary>
        /// 在线诊疗
        /// </summary>
        /// <param name="patientId">患者医院标识</param>
        /// <param name="pId">患者在互联网医院平台的标识</param>
        /// <param name="doctorId">医生标识</param>
        /// <param name="deptId">科室标识</param>
        /// <param name="complaint">患者主诉</param>
        /// <returns></returns>
        public ResponseMessage<string> MakeADiagnosis(string patientId,string pId,string doctorId,string deptId, string complaint)
        {
            var result = new ResponseMessage<string> { HasError = 0, ErrorMessage = "", Body = "" };
            using (HISService.n_webserviceSoapClient client = new HISService.n_webserviceSoapClient())
            {
                var requestMessage = String.Join("$$", patientId, pId, doctorId, deptId,complaint);
                var returnCode = "";

                var resultCode = client.f_get_data("zxzl", ref requestMessage, ref returnCode);
                if(resultCode==0)
                {
                    result.HasError = 0;

                }
                else
                {
                    result.HasError = 1;
                    result.ErrorMessage = returnCode;
                }

            }

            return result;
        }
    }
}
