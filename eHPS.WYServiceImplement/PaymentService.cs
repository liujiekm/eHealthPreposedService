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
using Oracle.ManagedDataAccess.Client;
using eHPS.Common;

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




        #region 第一版实现


        /// <summary>
        /// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        /// 则本方法实现需主动轮询，未支付的收费项目
        /// </summary>
        /// <param name="patientIds"></param>
        /// <returns></returns>
        //public List<Treatment> AwareOrderBooked(List<string> patientIds)
        //{
        //    var treatments = new List<Treatment>();
            
        //    using (var con = DapperFactory.CrateOracleConnection())
        //    {
        //        var patientIdsCondition = String.Join(",", patientIds.Select(p => "'" + p + "'"));

        //        #region 挂号
        //        //首先如果 担保挂号，查询担保挂号记录，查找挂号收费项目
        //        var registeredCommand =  @"Select ZLHDID, BRBH,GZDM, ZLLX, JZSJ FROM CW_YGHJL 
        //                                                    WHERE GHID IS NULL AND BRBH IN (" + patientIdsCondition + @") 
        //                                                    GROUP BY ZLHDID, BRBH,GZDM, ZLLX, JZSJ
        //                                                    ORDER BY JZSJ";
        //        //根据担保挂号 查询挂号收费项目
        //        var registeredItemCommand = @"SELECT B.MC, A.XMJE,A.XMID,A.ZMBS,A.JJRBS FROM CW_ZLLXGHXM A,CW_SFXM B WHERE A.XMID=B.XMID 
        //                                                        AND  ZLLX = :DiagnosisType AND GZDM = :JobTitleId 
        //                                                        AND ((KSSJ IS NULL OR KSSJ <= :ChargeTime) AND (JSSJ IS NULL OR JSSJ >= :ChargeTime))";
        //        var registeredResult = con.Query(registeredCommand).ToList();
        //        foreach (var register in registeredResult)
        //        {
        //            var treatment = new Treatment {
        //                PatientId = (String)register.BRBH,
        //                TreatmentId = (Int64)register.ZLHDID + "",
        //                Orders = new List<Order>()
        //            };
        //            var condition = new { DiagnosisType = (String)register.ZLLX, JobTitleId = (String)register.GZDM,ChargeTime=(DateTime)register.JZSJ};
        //            var order = new Order {
        //                 BookingTime= (DateTime)register.JZSJ,
        //                 HospitalOrderId= (Int64)register.ZLHDID + "",
        //                 OrderDescribe="挂号",
        //                 OrderType=OrderType.Registration,
        //                 OrderItems= new List<OrderItem>(),
        //                 OrderState=OrderState.Nonpayment,
        //                 Remark= "担保挂号",
        //                 ExpireTime=default(DateTime)

        //            };
        //            var registeredItemResult = con.Query(registeredItemCommand, condition).ToList();
        //            foreach (var item in registeredItemResult)
        //            {
        //                var orderItem = new OrderItem {
        //                     ItemId= (Int64)item.XMID+"",
        //                     ItemCount=1,
        //                     ItemName=(String)item.MC,
        //                     ItemUnitPrice=(decimal)item.XMJE,
        //                     ItemGroupNO="",
        //                     ItemSpecification=""
        //                };
        //                order.OrderItems.Add(orderItem);
        //            }
        //            treatment.Orders.Add(order);

        //            treatments.Add(treatment);

        //        }
        //        #endregion

        //        #region 药品医嘱
        //        var medicineCommand = @"SELECT YZID,ZLHDID,BRBH,YZLB,ZH,YPID,MC,DW,DJ,SL ,YZSJ FROM YL_MZYPYZ 
        //                                                 WHERE BRBH IN (" + patientIdsCondition + ")  AND ZTBZ IS NULL AND YZLB > '0' ORDER BY YZLB";




        //        var medicineResult = con.Query(medicineCommand).ToList();
        //        foreach (var item in medicineResult)
        //        {
        //            //查找treatments中是否已存在同样的诊疗活动
        //            if (treatments.Any(p=>p.TreatmentId==(Int64)item.ZLHDID+""))
        //            {
        //                var treatment = treatments.FirstOrDefault(p => p.TreatmentId == (Int64)item.ZLHDID + "");
        //                //判断是否已经存在药品项目
        //                if(treatment.Orders.Any(o=>o.OrderType==OrderType.Medicine))
        //                {
        //                    var order = treatment.Orders.FirstOrDefault(o => o.OrderType == OrderType.Medicine);
        //                    var orderItem = new OrderItem
        //                    {
        //                        ItemId = (Int64)item.YZID + "",
        //                        ItemGroupNO = (Int32)item.ZH + "",
        //                        ItemName = (String)item.MC,
        //                        ItemCount = (double)item.SL,
        //                        ItemUnitPrice = (decimal)item.DJ,
        //                        ItemSpecification = (string)item.DW
        //                    };
        //                    order.OrderItems.Add(orderItem);
        //                    AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
                            
        //                }
        //                else
        //                {
        //                    var order = new Order
        //                    {
        //                        BookingTime = (DateTime)item.YZSJ,
        //                        ExpireTime = default(DateTime),
        //                        HospitalOrderId = (Int64)item.ZLHDID + "",
        //                        OrderState = OrderState.Nonpayment,
        //                        OrderType = OrderType.Medicine,
        //                        OrderItems = new List<OrderItem> {
        //                             new OrderItem {
        //                                 ItemId= (Int64)item.YZID+"",
        //                                 ItemGroupNO=(Int32)item.ZH+"",
        //                                 ItemName=(String)item.MC,
        //                                 ItemCount=(double)item.SL,
        //                                 ItemUnitPrice=(decimal)item.DJ,
        //                                 ItemSpecification = (string)item.DW
        //                             }
        //                         },
        //                        OrderDescribe = "",
        //                        Remark = ""
        //                    };
        //                    AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
        //                    treatment.Orders.Add(order);
                            
        //                }
                        
        //            }
        //            else //不存在则要添加新的treatment
        //            {
        //                var order = new Order
        //                {
        //                    BookingTime = (DateTime)item.YZSJ,
        //                    ExpireTime = default(DateTime),
        //                    HospitalOrderId = (Int64)item.ZLHDID + "",
        //                    OrderState = OrderState.Nonpayment,
        //                    OrderType = OrderType.Medicine,
        //                    OrderItems = new List<OrderItem> {
        //                                 new OrderItem {
        //                                     ItemId= (Int64)item.YZID+"",
        //                                     ItemGroupNO=(Int32)item.ZH+"",
        //                                     ItemName=(String)item.MC,
        //                                     ItemCount=(double)item.SL,
        //                                     ItemUnitPrice=(decimal)item.DJ,
        //                                     ItemSpecification = (string)item.DW
        //                                 }
        //                             },
        //                    OrderDescribe = "",
        //                    Remark = ""
        //                };
        //                AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
        //                var treatment = new Treatment {
        //                    PatientId = (String)item.BRBH,
        //                    TreatmentId = (Int64)item.ZLHDID + "",
        //                    Orders = new List<Order> {
        //                        order
        //                    }
        //                };

        //                //添加到treatments
        //                treatments.Add(treatment);
        //            }
        //        }

        //        #endregion

        //        #region 检查 治疗
        //        CureAndInspection(treatments, patientIds, con);

        //        #endregion

        //        //#region 检验

        //        //#endregion



        //    }






        //    TreatmentAuxiliaryData(treatments);
        //    return treatments;
        //}


  

        /// <summary>
        /// 添加对应药品医嘱的附加医嘱到同一组内
        /// </summary>
        /// <param name="order"></param>
        /// <param name="adviceId"></param>
        /// <param name="groupNO"></param>
        /// <param name="con"></param>
        private void AdditionMedicine(Order order,Int64 adviceId,Int32 groupNO,OracleConnection con)
        {
            //药品附加医嘱
            var medicineAdditionCommand = @"SELECT '0' ZF,BRBH,JLID YZID,SFXMID,SFZHID,SL,ZLHDID,YHID,YZSJ,BMID,'7' FJYZLB,YZLB,-1 YZID,'' SPJG,'2' SFFS,'' KZJB,MC,DJ,YZMLID ZH,JYID,ZKID FROM (
                                                                     SELECT C.BRBH,C.JLID JLID,B.SFXMID SFXMID,B.SFZHID,C.SL,C.ZLHDID,C.YHID,C.YZSJ,C.BMID,D.MC,D.DJ,C.YZMLID,C.JYID,C.YZLB,E.JZZKID ZKID 
                                                                     FROM YL_YZMLMX A,YL_YZXM B,YL_YPFJYZ C,CW_SFXM D,YL_ZLHD E 
                                                                     WHERE E.ZLHDID = C.ZLHDID AND A.YZXMID = B.YZXMID AND C.YZMLID = A.MLID 
                                                                     AND B.SFXMID = D.XMID AND C.ZTBZ IS NULL AND B.ZTBZ = '1' 
                                                                     AND A.ZTBZ = '1' AND B.SFXMID IS NOT NULL AND C.SL > 0 AND C.JLID=:AdviceId）";
            //当前药品医嘱的附加医嘱,前提肯定是已经存在药品医嘱
            var medicineAdditionCondition = new { AdviceId = adviceId };
            var medicineAdditionResult = con.Query(medicineAdditionCommand, medicineAdditionCondition).ToList();
            foreach (var medicineAddition in medicineAdditionResult)
            {
                order.OrderItems.Add(new OrderItem {
                     ItemGroupNO=groupNO+"",
                     ItemId="",
                     ItemName= (String)medicineAddition.MC,
                     ItemCount=1,
                     ItemSpecification="",
                     ItemUnitPrice= (decimal)medicineAddition.DJ
                });
            }
        }


        /// <summary>
        /// 根据查询的各类医嘱类型，添加到对应的treatments
        /// </summary>
        /// <param name="treatments"></param>
        /// <param name="result"></param>
        /// <param name="orderType"></param>
        private void FulfilTreatment(List<Treatment> treatments,List<dynamic> result,OrderType orderType)
        {
            foreach (var item in result)
            {
                if (treatments.Any(p => p.TreatmentId == (Int64)item.ZLHDID + ""))
                {
                    var treatment = treatments.FirstOrDefault(p => p.TreatmentId == (Int64)item.ZLHDID + "");
                    //判断是否已经存在指定的医嘱项目
                    if (treatment.Orders.Any(o => o.OrderType == orderType))
                    {
                        var order = treatment.Orders.FirstOrDefault(o => o.OrderType == orderType);
                        var orderItem = new OrderItem
                        {
                            ItemId = (Int64)item.YZID + "",
                            ItemGroupNO = item.ZH==null?"":(Int32)item.ZH+"",
                            ItemName = (String)item.MC,
                            ItemCount = (double)item.SL,
                            ItemUnitPrice = (decimal)item.DJ,
                            ItemSpecification = item.DW==null?"":(String)item.DW
                        };
                        order.OrderItems.Add(orderItem);


                    }
                    else
                    {
                        var order = new Order
                        {
                            BookingTime = (DateTime)item.YZSJ,
                            ExpireTime = default(DateTime),
                            HospitalOrderId = (Int64)item.ZLHDID + "",
                            OrderState = OrderState.Nonpayment,
                            OrderType = orderType,
                            OrderItems = new List<OrderItem> {
                                     new OrderItem {
                                         ItemId= (Int64)item.YZID+"",
                                         ItemGroupNO=item.ZH==null?"":(Int32)item.ZH+"",
                                         ItemName=(String)item.MC,
                                         ItemCount=(double)item.SL,
                                         ItemUnitPrice=(decimal)item.DJ,
                                         ItemSpecification = item.DW==null?"":(String)item.DW
                                     }
                                 },
                            OrderDescribe = "",
                            Remark = ""
                        };

                        treatment.Orders.Add(order);

                    }

                }
                else //不存在则要添加新的treatment
                {
                    var order = new Order
                    {
                        BookingTime = (DateTime)item.YZSJ,
                        ExpireTime = default(DateTime),
                        HospitalOrderId = (Int64)item.ZLHDID + "",
                        OrderState = OrderState.Nonpayment,
                        OrderType = orderType,
                        OrderItems = new List<OrderItem> {
                                         new OrderItem {
                                             ItemId= (Int64)item.YZID+"",
                                             ItemGroupNO=item.ZH==null?"":(Int32)item.ZH+"",
                                             ItemName=(String)item.MC,
                                             ItemCount=(double)item.SL,
                                             ItemUnitPrice=(decimal)item.DJ,
                                             ItemSpecification =item.DW==null?"":(String)item.DW
                                         }
                                     },
                        OrderDescribe = "",
                        Remark = ""
                    };

                    var treatment = new Treatment
                    {
                        PatientId = (String)item.BRBH,
                        TreatmentId = (Int64)item.ZLHDID + "",
                        Orders = new List<Order> {
                                order
                            }
                    };
                    treatments.Add(treatment);
                }
            }
        }






        /// <summary>
        /// 增加治疗以及检查医嘱收费信息
        /// </summary>
        /// <param name="treatments"></param>
        /// <param name="patientIds"></param>
        private void CureAndInspection(List<Treatment> treatments,List<String> patientIds,OracleConnection con)
        {
            var command = @"SELECT  BRBH,ZLHDID,YZYHID YSID,SQDSJID,SFXMID,SFZHID,SL,1 CS,DJ,YZSJ,YZID,MC,ZF,BMID,ZKID,
                                        DECODE(YZLBDM,'03','6','5') YZLY,'1' SFFS,'' KZJB,JYID FROM YL_MZSQDYZ T1 
                                        WHERE BRBH =:PatientId AND
                                        (YZLBDM LIKE '03%' OR YZLBDM LIKE '02%') AND EXISTS
                                        (SELECT * FROM YL_SQDSJ T2 WHERE BRBH =:PatientId AND
                                        (T2.YZLBDM LIKE '03%' OR T2.YZLBDM LIKE '02%')
                                        AND T2.ZTBZ IS NULL AND T1.SQDSJID = T2.SQDSJID) 
                                        AND ZTBZ IS NULL";

            foreach (var patientId in patientIds)
            {
                var condition = new { PatientId = patientId };
                var result = con.Query(command, condition).ToList();
                var cureResult = result.Where(p=>(String)p.YZLY=="6").ToList();
                var inspectionResult = result.Where(p => (String)p.YZLY == "5").ToList();
                FulfilTreatment(treatments, cureResult, OrderType.Cure);
                FulfilTreatment(treatments, inspectionResult, OrderType.Inspection);
            }
            
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


        #endregion




        /// <summary>
        /// 主动推送：如果医生在HIS系统内部给平台用户开具了医嘱并等待收费，
        /// 则本方法实现需主动轮询，未支付的收费项目
        /// </summary>
        /// <param name="patientIds"></param>
        /// <returns></returns>
        public List<PatientConsumption> AwareOrderBooked(List<string> patientIds)
        {

            var patientConsumptions = new List<PatientConsumption>();

            if(patientIds==null)
            {
                return patientConsumptions;
            }
            var paramter = String.Join("$$", patientIds);

            //从webservice中返回 包含格式化 具体项目的字符串

            HISService.n_webserviceSoapClient client = new HISService.n_webserviceSoapClient();

            String code = "getmzyz";
            String content = paramter;
            String serviceReturn = "";
            
            var serviceReturnCode = client.f_get_data(code,ref content,ref serviceReturn);

            if (serviceReturnCode == 0)
            {
                var items = CommonService.RetriveFromString(content);
                //按患者标识分组归类
                foreach (var item in items)
                {
                    double itemCount = 0.0;
                    decimal itemUintPrice = 0.0M;
                    DateTime orderTime = default(DateTime);

                    Double.TryParse((String)item.ItemCount, out itemCount);
                    Decimal.TryParse((String)item.ItemUnitPrice, out itemUintPrice);
                    DateTime.TryParse((String)item.OrderTime, out orderTime);

                    if (patientConsumptions.Any(p => p.PatientId == (String)item.PatientId)) //已经存在当前患者的消费项目信息
                    {
                        var patientConsumption = patientConsumptions.FirstOrDefault(p => p.PatientId == (String)item.PatientId);
                        //检查是否存相同的诊疗活动标识
                        if (patientConsumption.TreatmentActivityInfos.Any(t => t.TreatmentId == (String)item.TreatmentId))
                        {
                            //在同样的诊疗活动下面则把项目添加到相关的诊疗活动下面
                            var treatmentActivityInfo = patientConsumption.TreatmentActivityInfos.FirstOrDefault(t => t.TreatmentId == (String)item.TreatmentId);

                            treatmentActivityInfo.Orders.Add(
                                   new OrderItem
                                   {
                                       ItemId = (String)item.ItemId,
                                       ItemCount = itemCount,
                                       ItemGroupNO = item.ItemGroupNO == null ? "" : (String)item.ItemGroupNO,
                                       ItemName = (String)item.ItemName,
                                       ItemSpecification = (String)item.ItemSpecification,
                                       ItemType = (String)item.ItemType,
                                       ItemUnitPrice = itemUintPrice,
                                       OrderTime = orderTime
                                   }
                                );

                        }
                        else //不存在相同的诊疗活动标识
                        {
                            patientConsumption.TreatmentActivityInfos.Add(new TreatmentActivityInfo {
                                TreatmentId = (String)item.TreatmentId,
                                Orders = new List<OrderItem> {
                                  new OrderItem {
                                            ItemId=(String)item.ItemId,
                                            ItemCount=itemCount,
                                            ItemGroupNO=item.ItemGroupNO==null?"":(String)item.ItemGroupNO,
                                            ItemName=(String)item.ItemName,
                                            ItemSpecification=(String)item.ItemSpecification,
                                            ItemType=(String)item.ItemType,
                                            ItemUnitPrice=itemUintPrice,
                                            OrderTime=orderTime
                                       }
                             }
                            });
                        }
                    }
                    else
                    {
                        var patientConsumption = new PatientConsumption {
                            PatientId = (String)item.PatientId,
                            TreatmentActivityInfos = new List<TreatmentActivityInfo> {
                            new TreatmentActivityInfo
                            {
                                 TreatmentId=(String)item.TreatmentId,
                                  Orders= new List<OrderItem> {
                                       new OrderItem {
                                            ItemId=(String)item.ItemId,
                                            ItemCount=itemCount,
                                            ItemGroupNO=item.ItemGroupNO==null?"":(String)item.ItemGroupNO,
                                            ItemName =(String)item.ItemName,
                                            ItemSpecification=(String)item.ItemSpecification,
                                            ItemType=(String)item.ItemType,
                                            ItemUnitPrice=itemUintPrice,
                                            OrderTime=orderTime
                                       }
                                  }
                            }
                        }
                        };
                        patientConsumptions.Add(patientConsumption);
                    }
                }

                TreatmentAuxiliaryData(patientConsumptions);
            }
            return patientConsumptions;

        }




        /// <summary>
        /// 根据诊疗活动标识以及患者标识补充以下数据
        /// 科室信息、病人基本信息、医生基本信息、诊断信息
        /// </summary>
        /// <param name="treatments"></param>
        private void TreatmentAuxiliaryData(List<PatientConsumption> patientConsumptions)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                //var treatmentIds = patientConsumptions.Select(p => p.TreatmentActivityInfos.Select(t => t.TreatmentId)).ToList();
                var activityCommand = @"SELECT BRXM,JZZKID,JZYSYHID FROM YL_ZLHD WHERE ZLHDID=:ActiveId";
                //获取患者诊断信息
                var diagnoseCommand = @"SELECT ICD,LCZD  FROM YL_ZLZD WHERE ZLHDID=:ActiveId";

                foreach (var patientConsumption in patientConsumptions)
                {
                    //patientConsumption.AppId = "";
                    patientConsumption.PatientName = basicService.GetPatientInfo(patientConsumption.PatientId).PatientName;
                    foreach (var treatment in patientConsumption.TreatmentActivityInfos)
                    {
                        var condition = new { ActiveId = treatment.TreatmentId };

                        var treatmentResult = con.Query(activityCommand, condition).FirstOrDefault();
                        treatment.DeptdId = (Int32)treatmentResult.JZZKID + "";
                        treatment.DeptName = CommonService.GetDeptName((Int32)treatmentResult.JZZKID);
                        treatment.DoctorId = (Int64)treatmentResult.JZYSYHID + "";
                        treatment.DoctorName = basicService.GetDoctorById((Int64)treatmentResult.JZYSYHID + "").DoctorName;

                        treatment.Diagnostics = new List<Diagnostics>();
                        var diagnoseResult = con.Query(diagnoseCommand, condition);
                        foreach (var diagnose in diagnoseResult)
                        {
                            treatment.Diagnostics.Add(new Diagnostics {
                                 ICD= (String)diagnose.ICD,
                                 DiagnosisName=(String)diagnose.LCZD

                            });
                        }


                    }
                }
            }
        }




        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，直接返回成功与否信息
        /// </summary>
        /// <param name="trading">本次交易标识</param>
        /// <param name="activityId">
        /// 当前支付的诊疗活动标识
        /// </param>
        /// <param name="amount">总金额</param>
        /// <param name="amount">实际交易金额</param>
        /// <returns>
        /// HasError：0 支付成功
        /// HasError：1 交易标识、诊疗活动标识、总金额、实际交易金额不可为空
        /// HasError：2 预存充值失败
        /// HasError：3 充值成功，结算失败，余额存入医院预存账户
        /// </returns>
        public ResponseMessage<string> Pay(String tradingId ,String activityId, String amount,String actualAmount)
        {
            var result = new ResponseMessage<string> { HasError = 0, ErrorMessage = "", Body = "" };
            if (String.IsNullOrEmpty(tradingId) || String.IsNullOrEmpty(activityId) || String.IsNullOrEmpty(amount)||String.IsNullOrEmpty(actualAmount))
            {
                result.HasError = 1;
                result.ErrorMessage = "交易标识、诊疗活动标识、总金额、实际交易金额不可为空";
                return result;
            }

            //var requestMessage = String.Join("$$", trading, activityId, amount)
            var requestMessage = String.Join("$$", tradingId,activityId, amount, actualAmount);
            var returnCode = "";
            using (HISService.n_webserviceSoapClient client = new HISService.n_webserviceSoapClient())
            {
                 var resultCode = client.f_get_data("zljs", ref requestMessage, ref returnCode);

                if(resultCode==0)
                {
                    result.HasError = 0;
                    result.ErrorMessage = requestMessage;
                    result.Body = requestMessage;
                }
                else if (resultCode==-100)
                {
                    //充值失败
                    result.HasError = 2;
                    result.ErrorMessage = "预存充值失败";
                    result.Body = requestMessage;
                }
                else
                {
                    //充值成功，结算失败，余额存入医院预存账户
                    result.HasError = 3;
                    result.ErrorMessage = "充值成功，结算失败，余额存入医院预存账户  错误信息：" + returnCode;
                    result.Body = requestMessage;
                }
            }
            //MessageQueueHelper.PushMessage<ResponseMessage<String>>("", result);
            return result;

        }


        /// <summary>
        /// 挂号收费
        /// 收费成功后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalId">院区标识</param>
        /// <param name="appointId">预约标识</param>
        /// <returns></returns>
        public ResponseMessage<string> PayRegistration(string tradingId,string hospitalId, string appointId,string amount,string actualAmount)
        {
            var result = new ResponseMessage<string> { HasError = 0, ErrorMessage = "", Body = "" };
            //MessageQueueHelper.PushMessage<ResponseMessage<String>>("", result);
            return result;
        }






        /// <summary>
        /// 获取指定患者的医院账户可用金额（预存for温附一）
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <returns></returns>
        public TradingAccount GetPatientAvaliableAmount(string patientId)
        {
            var accountInfo = new TradingAccount();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var accountAvaliableCommand = @"SELECT ZTBZ  FROM CW_YCZH WHERE BRBH =:PatientId";
                var condition = new { PatientId=patientId };

                var accountAvaliable = con.Query(accountAvaliableCommand, condition).FirstOrDefault();
                if(accountAvaliable!=null&&(String)accountAvaliable.ZTBZ=="1")//预存账户可用
                {
                    var accountAmountCommand = @"SELECT A.YCDM,B.MZKY,B.TJKY,A.KYJE,A.KYJE - A.XFJE - A.MZDJJE  AS YE
                                                                        FROM CW_YCJE A,CW_YCDM B  WHERE A.BRBH = :PatientId  AND  A.YCDM = B.YCDM 
                                                                        AND A.YXSJ >= SYSDATE AND (A.KSSJ IS NULL OR A.KSSJ <= SYSDATE) 
                                                                        AND B.ZTBZ = '1' AND A.ZTBZ = '1' AND A.KYJE - A.XFJE >= 0 AND B.MZKY='1' ORDER BY A.YXJ";

                    var accountAmountResult = con.Query(accountAmountCommand,condition).ToList();
                    decimal amount = 0.0M;
                    foreach (var item in accountAmountResult)
                    {
                        amount += (decimal)item.YE;
                    }

                    accountInfo.Avaliable = true;
                    accountInfo.PatientId = patientId;
                    accountInfo.Amount = amount;
                    
                }
                else
                {
                    accountInfo.Avaliable = false;
                    accountInfo.PatientId = patientId;
                    
                }

                accountInfo.PatientName = basicService.GetPatientInfo(accountInfo.PatientId).PatientName;
                return accountInfo;


            }
        }



        /// <summary>
        /// 预存充值（温付一预存挂号其实就是充值呢）
        /// </summary>
        /// <param name="tradingId">交易标识</param>
        /// <param name="appointId">预约标识</param>
        /// <param name="amount">实际充值金额</param>
        /// <returns>
        /// HasError :0 充值成功
        /// HasError :1 交易标识、预约不能为空/患者未用就诊卡预约，无法挂号/不存在预约记录
        /// HasError :2 挂号充值失败(ErrorMessage 包含错误信息)
        /// </returns>
        public ResponseMessage<string> Recharge(string tradingId, string appointId,string amount)
        {
            var result = new ResponseMessage<string> { HasError = 0, ErrorMessage = "", Body = "" };
            if (String.IsNullOrEmpty(tradingId) || String.IsNullOrEmpty(appointId))
            {
                result.HasError = 1;
                result.ErrorMessage = "交易标识、预约不能为空";
                return result;
            }

            //根据预约标识获取患者标识以及挂号金额
            var command = @"SELECT BRBH ,ZLLX, GZDM FROM YYFZ_YYXX  WHERE FZYYID=" + Int64.Parse(appointId);
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var queryResult = con.Query(command).FirstOrDefault();
                if(queryResult!=null)
                {
                    if (queryResult.BRBH!=null)//如果病人编号不为空
                    {

                        var patientId = (String)queryResult.BRBH;
                        var diagnosisTypeId = (String)queryResult.ZLLX;
                        var jobTitleId = (String)queryResult.GZDM;

                        //var amount = basicService.GetRegisteredAmount(diagnosisTypeId, jobTitleId);

                        var requestMessage = String.Join("$$", patientId, tradingId, amount);
                        var returnCode = "";
                        using (HISService.n_webserviceSoapClient client = new HISService.n_webserviceSoapClient())
                        {
                            var resultCode = client.f_get_data("yccz", ref requestMessage, ref returnCode);

                            if (resultCode == 0)
                            {
                                result.HasError = 0;
                                result.ErrorMessage = "充值成功";
                                result.Body = "充值成功";
                            }
                            else
                            {
                                //挂号充值失败
                                result.HasError = 2;
                                result.ErrorMessage = returnCode;

                            }
                        }
                    }
                    else
                    {
                        result.HasError = 1;
                        result.ErrorMessage = "患者未用就诊卡预约，无法挂号";
                    }
                    return result;
                }
                else
                {
                    result.HasError = 1;
                    result.ErrorMessage = "不存在预约记录";
                    return result;
                }
                 
            }

        }
    }
}
