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

                #region 挂号
                //首先如果 担保挂号，查询担保挂号记录，查找挂号收费项目
                var registeredCommand =  @"Select ZLHDID, BRBH,GZDM, ZLLX, JZSJ FROM CW_YGHJL 
                                                            WHERE GHID IS NULL AND BRBH IN (" + patientIdsCondition + @") 
                                                            GROUP BY ZLHDID, BRBH,GZDM, ZLLX, JZSJ
                                                            ORDER BY JZSJ";
                //根据担保挂号 查询挂号收费项目
                var registeredItemCommand = @"SELECT B.MC, A.XMJE,A.XMID,A.ZMBS,A.JJRBS FROM CW_ZLLXGHXM A,CW_SFXM B WHERE A.XMID=B.XMID 
                                                                AND  ZLLX = :DiagnosisType AND GZDM = :JobTitleId 
                                                                AND ((KSSJ IS NULL OR KSSJ <= :ChargeTime) AND (JSSJ IS NULL OR JSSJ >= :ChargeTime))";
                var registeredResult = con.Query(registeredCommand).ToList();
                foreach (var register in registeredResult)
                {
                    var treatment = new Treatment {
                        PatientId = (String)register.BRBH,
                        TreatmentId = (Int64)register.ZLHDID + "",
                        Orders = new List<Order>()
                    };
                    var condition = new { DiagnosisType = (String)register.ZLLX, JobTitleId = (String)register.GZDM,ChargeTime=(DateTime)register.JZSJ};
                    var order = new Order {
                         BookingTime= (DateTime)register.JZSJ,
                         HospitalOrderId= (Int64)register.ZLHDID + "",
                         OrderDescribe="挂号",
                         OrderType=OrderType.Registration,
                         OrderItems= new List<OrderItem>(),
                         OrderState=OrderState.Nonpayment,
                         Remark= "担保挂号",
                         ExpireTime=default(DateTime)

                    };
                    var registeredItemResult = con.Query(registeredItemCommand, condition).ToList();
                    foreach (var item in registeredItemResult)
                    {
                        var orderItem = new OrderItem {
                             ItemId= (Int64)item.XMID+"",
                             ItemCount=1,
                             ItemName=(String)item.MC,
                             ItemUnitPrice=(decimal)item.XMJE,
                             ItemGroupNO="",
                             ItemSpecification=""
                        };
                        order.OrderItems.Add(orderItem);
                    }
                    treatment.Orders.Add(order);

                    treatments.Add(treatment);

                }
                #endregion

                #region 药品医嘱
                var medicineCommand = @"SELECT YZID,ZLHDID,BRBH,YZLB,ZH,YPID,MC,DW,DJ,SL ,YZSJ FROM YL_MZYPYZ 
                                                         WHERE BRBH IN (" + patientIdsCondition + ")  AND ZTBZ IS NULL AND YZLB > '0' ORDER BY YZLB";




                var medicineResult = con.Query(medicineCommand).ToList();
                foreach (var item in medicineResult)
                {
                    //查找treatments中是否已存在同样的诊疗活动
                    if (treatments.Any(p=>p.TreatmentId==(Int64)item.ZLHDID+""))
                    {
                        var treatment = treatments.FirstOrDefault(p => p.TreatmentId == (Int64)item.ZLHDID + "");
                        //判断是否已经存在药品项目
                        if(treatment.Orders.Any(o=>o.OrderType==OrderType.Medicine))
                        {
                            var order = treatment.Orders.FirstOrDefault(o => o.OrderType == OrderType.Medicine);
                            var orderItem = new OrderItem
                            {
                                ItemId = (Int64)item.YZID + "",
                                ItemGroupNO = (Int32)item.ZH + "",
                                ItemName = (String)item.MC,
                                ItemCount = (double)item.SL,
                                ItemUnitPrice = (decimal)item.DJ,
                                ItemSpecification = (string)item.DW
                            };
                            order.OrderItems.Add(orderItem);
                            AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
                            
                        }
                        else
                        {
                            var order = new Order
                            {
                                BookingTime = (DateTime)item.YZSJ,
                                ExpireTime = default(DateTime),
                                HospitalOrderId = (Int64)item.ZLHDID + "",
                                OrderState = OrderState.Nonpayment,
                                OrderType = OrderType.Medicine,
                                OrderItems = new List<OrderItem> {
                                     new OrderItem {
                                         ItemId= (Int64)item.YZID+"",
                                         ItemGroupNO=(Int32)item.ZH+"",
                                         ItemName=(String)item.MC,
                                         ItemCount=(double)item.SL,
                                         ItemUnitPrice=(decimal)item.DJ,
                                         ItemSpecification = (string)item.DW
                                     }
                                 },
                                OrderDescribe = "",
                                Remark = ""
                            };
                            AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
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
                            OrderType = OrderType.Medicine,
                            OrderItems = new List<OrderItem> {
                                         new OrderItem {
                                             ItemId= (Int64)item.YZID+"",
                                             ItemGroupNO=(Int32)item.ZH+"",
                                             ItemName=(String)item.MC,
                                             ItemCount=(double)item.SL,
                                             ItemUnitPrice=(decimal)item.DJ,
                                             ItemSpecification = (string)item.DW
                                         }
                                     },
                            OrderDescribe = "",
                            Remark = ""
                        };
                        AdditionMedicine(order, (Int64)item.YZID, (Int32)item.ZH, con);
                        var treatment = new Treatment {
                            PatientId = (String)item.BRBH,
                            TreatmentId = (Int64)item.ZLHDID + "",
                            Orders = new List<Order> {
                                order
                            }
                        };

                        //添加到treatments
                        treatments.Add(treatment);
                    }
                }

                #endregion

                #region 检查 治疗
                CureAndInspection(treatments, patientIds, con);

                #endregion

                #region 检验

                #endregion



            }






            TreatmentAuxiliaryData(treatments);
            return treatments;
        }


  

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

        /// <summary>
        /// 支付患者的医嘱项目费用
        /// 支付成功之后，往消息队列发送成功与否的消息
        /// </summary>
        /// <param name="hospitalOrderId">医院订单标识
        /// 传递医嘱标识
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
