//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 支付服务温附一实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/13 16:37:12
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

namespace eHPS.WYServiceImplement
{
    /// <summary>
    /// 支付服务温附一实现
    /// </summary>
    public class PaymentService : IPayment
    {


        private IBasicInfo basicService;

        public PaymentService(IBasicInfo basicService)
        {
            this.basicService = basicService;
        }


        /// <summary>
        /// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        /// 则本方法实现需主动轮询，未支付的收费项目
        /// </summary>
        /// <param name="patientIds"></param>
        /// <returns></returns>
        public List<Treatment> AwareOrderBooked(List<string> patientIds)
        {
            var treatments = new List<Treatment>();
            
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var patientIdsCondition = String.Join(",", patientIds.Select(p => "'" + p + "'"));
                //首先如果 担保挂号，查询担保挂号记录，查找挂号收费项目
                var registeredCommand = @"Select ZLHDID, BRBH,GZDM, ZLLX, JZSJ 	FROM CW_YGHJL 
                                                            WHERE GHID IS NULL AND BRBH IN (" + patientIdsCondition + @") 
                                                            GROUP BY ZLHDID, BRBH,GZDM, ZLLX, JZSJ
                                                            ORDER BY JZSJ";
                var registeredResult = con.Query(registeredCommand).ToList();

                foreach (var register in registeredResult)
                {

                }
                //根据担保挂号 查询挂号收费项目
                var registeredItemCommand = @"SELECT B.MC, A.XMJE,A.XMID,A.ZMBS,A.JJRBS FROM CW_ZLLXGHXM A,CW_SFXM B WHERE A.XMID=B.XMID AND  ZLLX = :AS_ZLLX AND GZDM = :AS_GZDM 
                                                                AND ((KSSJ IS NULL OR KSSJ <= :ADT_SFSJ) AND (JSSJ IS NULL OR JSSJ >= :ADT_SFSJ))";






            }







            return treatments;
        }



        /// <summary>
        /// 根据诊疗活动标识以及患者标识补充以下数据
        /// 科室信息、病人基本信息、医生基本信息、诊断信息
        /// </summary>
        /// <param name="treatments"></param>
        private void TreatmentAuxiliaryData(List<Treatment> treatments)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var activityCommand = @"SELECT BRXM,JZYSYHID FROM YL_ZLHD WHERE ZLHDID=:ActiveId";
                //获取患者诊断信息
                var diagnoseCommand = @"SELECT ICD,LCZD  FROM YL_ZLZD WHERE ZLHDID=:ActiveId";
                foreach (var item in treatments)
                {
                    var condition = new { ActiveId =Int64.Parse(item.TreatmentId)};
                    var activityResult = con.Query(activityCommand, condition).FirstOrDefault();
                    if (activityResult!=null)
                    {
                        item.PatientName = (String)activityResult.BRXM;
                        var doctor = basicService.GetDoctorById((Int64)activityResult.JZYSYHID + "");
                        item.DoctorId = doctor.DoctorId;
                        item.DoctorName = doctor.DoctorName;
                        item.DeptdId = doctor.DeptId;
                        item.DeptName = doctor.DeptName;
                    }
                    var diagnoseResult = con.Query(diagnoseCommand, condition).ToList();
                    item.Diagnostics = new List<Diagnostics>();
                    foreach (var diagnose in diagnoseResult)
                    {
                        item.Diagnostics.Add(new Diagnostics
                        {
                            ICD = (String)diagnose.ICD,
                            DiagnosisName=(String)diagnose.LCZD
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalOrderId">医院订单标识
        /// 如果是药品，取药品组号
        /// 如果是检查、检验、治疗 去申请单标识
        /// </param>
        /// <param name="hospitalId">医院标识</param>
        /// <returns></returns>
        public ResponseMessage<string> Pay(List<string> hospitalOrderId, string hospitalId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 挂号收费
        /// 收费成功后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalId">医院标识</param>
        /// <param name="appointId">预约标识</param>
        /// <returns></returns>
        public ResponseMessage<string> PayRegistration(string hospitalId, string appointId)
        {
            throw new NotImplementedException();
        }


    }
}
