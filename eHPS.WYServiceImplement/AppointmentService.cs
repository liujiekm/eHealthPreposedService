//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 预约服务，温附一HIS系统实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 11:17:07
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
using eHPS.CrossCutting.NetFramework.Caching;
using Oracle.ManagedDataAccess.Client;
using eHPS.Common;
using System.Data;

namespace eHPS.WYServiceImplement
{
    public class AppointmentService : IAppointment
    {

        private IBasicInfo basicInfoService;

        public AppointmentService(IBasicInfo basicInfoService)
        {
            this.basicInfoService = basicInfoService;
        }

        /// <summary>
        /// 取消指定预约
        /// </summary>
        /// <param name="apponintId">预约标识</param>
        public ResponseMessage<string> CancelTheAppointment(string apponintId)
        {
            var response = new ResponseMessage<string>();
            //获取预约信息
            using (var con = DapperFactory.CrateOracleConnection())
            {
                //查找是否有挂号记录
                var registerCommand = @"select GHID from cw_ghmx where YYID=:ApponintId";
                var registerCondition = new { ApponintId= Int64.Parse(apponintId) };
                var registerResult = con.Query(registerCommand, registerCondition).FirstOrDefault();

                var appointmentState = "9";
                var excutorId = 19058;
                var modifyTime = DateTime.Now;
                var cancelCommand = @"update yyfz_yyxx set ZTBZ=:AppointmentState,CZZID=:ExcutorId,XGSJ=:ModifyTime where FZYYID=:ApponintId";
                var cancelCondition = new { AppointmentState = appointmentState, ExcutorId = excutorId, ModifyTime = modifyTime, ApponintId = apponintId };
                
                using (IDbTransaction transaction = con.BeginTransaction())
                {
                        //无挂号记录
                        if (registerResult == null || (Int64)registerResult.GHID == 0)
                        {
                            //修改预约状态
                            if (con.Execute(cancelCommand, cancelCondition) > 0)
                            {
                                response.HasError = 0;
                                response.ErrorMessage = "取消成功";
                            }
                            else
                            {
                                response.HasError = 1;
                                response.ErrorMessage = "数据跟新失败";
                            }
                        }
                        else //存在挂号记录则 设置此挂号 为 未使用
                        {
                            var updateRegisterHistory = @"update cw_ghmx set YYID=:ApponintId where GHID=:RegisterId";
                            var updateRegisterHistoryCondition = new { ApponintId = 0, RegisterId = (Int64)registerResult.GHID };
                            if (con.Execute(updateRegisterHistory, updateRegisterHistoryCondition) > 0 &&
                            con.Execute(cancelCommand, cancelCondition) > 0)
                            {
                                response.HasError = 0;
                                response.ErrorMessage = "取消成功";
                            }
                            else
                            {
                                response.HasError = 1;
                                response.ErrorMessage = "数据跟新失败";
                            }

                        }

                    transaction.Commit();
                }

            }


            return response;
        }


        /// <summary>
        /// 获取患者的预约历史
        /// 温附一 默认取一个月内的
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <param name="mobile">患者手机</param>
        /// <returns></returns>
        public List<BookHistory> GetAppointmentHistory(string patientId,string mobile)
        {
            var bookHistorys = new List<BookHistory>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var baseCommand = @"SELECT FZYYID,BRBH,BRXM,BRXB,CSRQ,LXDH,SFZ,LXDZ,YYXH,PDH,YYSJ,FZZBH,ZKID,
                                                    YSYHID,YYFS,ZTBZ,ZLLX,QDSJ,JZSJ,GHID,BZ,DJRYID,CZZID,XGSJ,PBID,HJLX,YJNR,DJSJ,
                                                    QDRYID,GZDM,QHPZDM,ZMTX,ZBYY FROM YYFZ_YYXX WHERE DJSJ>SYSDATE-31 
                                                    AND YYFS<>'F' AND (BRBH=:PatientId OR LXDH=:Mobile) ORDER BY ZTBZ,YYSJ DESC";

                var condition = new { PatientId = patientId, Mobile = mobile };
                var result = con.Query(baseCommand, condition).ToList();
                foreach (var item in result)
                {
                    var doctor = basicInfoService.GetDoctorById((Int64)item.YSYHID + "");
                    var deptName = basicInfoService.GetDeptName((Int32)item.ZKID + "");
                    var bookHistory = new BookHistory {
                        AppointId = (Int64)item.FZYYID + "",
                        AppointSequence = (Int32)item.PDH,
                        AppointState= (AppointState)Int32.Parse((string)item.ZTBZ),
                        AppointTime=(DateTime)item.YYSJ,
                        DoctorId=(Int64)item.YSYHID+"",
                        DoctorJobTitle=doctor.JobTitle,
                        DoctorSex=doctor.Sex,
                        DiagnosisType= (string)item.ZLLX == "04" ? "2" : "1",
                        DeptId =(Int32)item.ZKID+"",
                        DeptName= deptName,     
                        ArrangeId=(Int64)item.PBID+"",
                        PatientId=(string)item.BRBH,
                        PatientName=(string)item.BRXM,
                        PatientIdCard=(string)item.SFZ,
                        PatientMobile=(string)item.LXDH,
                        Attention="",
                        Remark=(string)item.ZBYY=="01"?"专病":"",
                        DoctorName= doctor.DoctorName,
                        CreateTime=(DateTime)item.DJSJ,
                        RegisteredAmount=GetRegisteredAmount((string)item.ZLLX,(string)item.GZDM)          
                    };
                    bookHistorys.Add(bookHistory);

                }
            }
            return bookHistorys;
        }







        /// <summary>
        /// 获取医生可预约信息
        /// </summary>
        /// <param name="deptId">科室标识</param>
        /// <returns></returns>
        public List<BookableDoctor> GetBookableInfo(String areaId,String deptId,String doctorId, DateTime? startTime, DateTime? endTime)
        {
            var bookableDoctors = new List<BookableDoctor>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                //首先取医生排班信息
                var command = @"SELECT B.PBID,B.YSXM,B.YSYHID,B.RYKID,B.ZKID,R.GZDM2,B.ZLLX,B.SBSJ,B.XBSJ,B.ZKXH,B.YQDM,B.ZBYY 
                                            FROM YYFZ_YSPB B,R_RYK R WHERE B.ZTBZ='1'AND B.RYKID=R.ID AND R.GZDM2!='0000'  AND B.YQDM=:AreaId AND B.ZKID=:DeptId
                                            AND (B.ZLLX='02' OR B.ZLLX='04' OR B.ZLLX='07') AND B.YSYHID =:DoctorId AND B.SBSJ >=:KSSJ and B.SBSJ<=:JSSJ";

                if(null==startTime)
                {
                    startTime = DateTime.Now;
                }
                if(null==endTime)
                {
                    endTime = DateTime.Now.AddDays(14);
                }
                var condition = new { AreaId=areaId, DeptId=deptId, DoctorId = Int32.Parse(doctorId),KSSJ= startTime, JSSJ= endTime };
                var result = con.Query(command, condition).ToList();
                var userPhotos = new Dictionary<Int32, Byte[]>();
                foreach (var item in result)
                {
                    
                    //每个员工图片只取一次
                    byte[] photo = default(byte[]);
                    if(!userPhotos.TryGetValue((Int32)item.RYKID, out photo))
                    {
                        photo = GetDoctorPhoto(((Int32)item.RYKID).ToString(), con);
                        userPhotos.Add((Int32)item.RYKID, photo);
                    }
                    var bookableDoctor = new BookableDoctor {
                        ArrangeId = ((Int32)item.PBID).ToString(),
                        ArrangeStartTime = ((DateTime)item.SBSJ).ToLocalTime(),
                        ArrangeEndTime = ((DateTime)item.XBSJ).ToLocalTime(),
                        DeptId = ((Int32)item.ZKID).ToString(),
                        DeptName=CommonService.GetDeptName((Int32)item.ZKID),
                        DocotorId=((Int32)item.YSYHID).ToString(),
                        DoctorName=(String)item.YSXM,
                        JobTitleId=(String)item.GZDM2,
                        Photo = photo,
                        RegisteredAmount=GetRegisteredAmount((string)item.ZLLX, (String)item.GZDM2),
                        DiagnosisType= (string)item.ZLLX=="04"?"2":"1",
                        Remark =item.ZBYY==null?"":(string)item.ZBYY=="01"?"专病":"",
                        JobTitle =CommonService.GetJobTitle((String)item.GZDM2),
                        BookableTimePoints=GetBookableTimePoint(((Int32)item.PBID).ToString()),
                        SumBookNum=(Int32)item.ZKXH,
                        UsedBookNum=GetUesdBookNum(((Int32)item.PBID).ToString(),con),
                         
                    };
                    bookableDoctors.Add(bookableDoctor);
                }
            }

            return bookableDoctors;
        }



        public List<BookableTimePoint> GetBookableTimePoint(string arrangeId)
        {
            dynamic result;

            var bookableTimePoints = new List<BookableTimePoint>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,pb.bclb,
                                            (select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,
                                            (select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,
                                            pb.zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje 
                                            from yyfz_yspb pb where pb.pbid=:ArrangeId";
                var condition = new { ArrangeId=Int64.Parse(arrangeId) };
                result = con.Query(command, condition).FirstOrDefault();

            }

            #region 上一个实现版本拷贝
            if ((string)result.BCLB == "09" && DateHelper.GetSeconds((DateTime)result.SBSJ, (DateTime)result.XBSJ) / 3600 > 5)   //全天
            {
                DateTime[] ldt_sbsj = new DateTime[2];
                DateTime[] ldt_xbsj = new DateTime[2];

                ldt_sbsj[0] = (DateTime)result.SBSJ;  //上午上班时间
                string timeAME = "", zd1AME = "";
                if (((DateTime)result.SBSJ).Month > 9)
                {
                    zd1AME = ((DateTime)result.SBSJ).Month + "AME";//begin AMEnd
                }
                else
                {
                    zd1AME = "0" + ((DateTime)result.SBSJ).Month + "AME";//begin AMEnd
                }
                timeAME = CommonService.GetTimetable(zd1AME);

                ldt_xbsj[0] = new DateTime(ldt_sbsj[0].Year, ldt_sbsj[0].Month, ldt_sbsj[0].Day, Convert.ToInt32(timeAME.Split(':')[0]), Convert.ToInt32(timeAME.Split(':')[1]), 0);  //上午下班时间

                ldt_xbsj[1] = (DateTime)result.XBSJ;  //下午下班时间

                string timePMB = "", zd1PMB = "";
                if (Convert.ToDateTime(result.SBSJ).Month > 9)
                {
                    zd1PMB = ((DateTime)result.SBSJ).Month + "PMB";
                }
                else
                {
                    zd1PMB = "0" + ((DateTime)result.SBSJ).Month + "PMB";
                }
                timePMB = CommonService.GetTimetable(zd1PMB);
                ldt_sbsj[1] = new DateTime(ldt_sbsj[0].Year, ldt_sbsj[0].Month, ldt_sbsj[0].Day, Convert.ToInt32(timePMB.Split(':')[0]), Convert.ToInt32(timePMB.Split(':')[1]), 0);  //下午上班时间

                //计算上下午秒数
                long sjd0 = DateHelper.GetSeconds(ldt_sbsj[0], ldt_xbsj[0]);
                long sjd1 = DateHelper.GetSeconds(ldt_sbsj[1], ldt_xbsj[1]);

                //计算上下午专科限号数
                int zkxh0 = Convert.ToInt32(Math.Floor(Convert.ToDouble((Convert.ToDouble(sjd0) / (sjd0 + sjd1)) * (Int32)result.ZKXH)));
                int zkxh1 = Convert.ToInt32((Int32)result.ZKXH) - zkxh0;

                CreateTimePoint(ldt_sbsj[0], ldt_xbsj[0], 1, zkxh0, zkxh0, bookableTimePoints, Convert.ToInt64(arrangeId));
                CreateTimePoint(ldt_sbsj[1], ldt_xbsj[1], zkxh0 + 1, zkxh0 + zkxh1, zkxh1, bookableTimePoints, Convert.ToInt64(arrangeId));
            }
            else
            {
                CreateTimePoint(Convert.ToDateTime(result.SBSJ), Convert.ToDateTime(result.XBSJ), 1, Convert.ToInt32(result.ZKXH), Convert.ToInt32(result.ZKXH), bookableTimePoints, Convert.ToInt64(arrangeId));
            }
            return bookableTimePoints.FindAll(d => d.AppointTime > DateTime.Now).OrderBy(d => d.AppointSequence).ToList();

            #endregion




        }


        #region 上一个实现版本拷贝


        /// <summary>
        /// 创建可预约的时间点
        /// </summary>
        /// <param name="sbsj"></param>
        /// <param name="xbsj"></param>
        /// <param name="ksh"></param>
        /// <param name="jsh"></param>
        /// <param name="zkxh"></param>
        /// <param name="list"></param>
        /// <param name="pbid"></param>
        private void CreateTimePoint(DateTime sbsj, DateTime xbsj, int ksh, int jsh, int zkxh, List<BookableTimePoint> list, Int64 pbid)
        {

            DateTime ldt_yysj = new DateTime();
            
            xbsj = xbsj.AddMinutes(-10);
            long jzsc = Convert.ToInt64(Math.Floor((Convert.ToDouble(DateHelper.GetSeconds(sbsj, xbsj)) / zkxh + 0.5))); //就诊时长

            #region 不显示加号算法
            for (int i = ksh; i <= jsh; i++)
            {

                if (i == ksh)
                {
                    ldt_yysj = sbsj;
                }
                else
                {
                    ldt_yysj = ldt_yysj.AddSeconds(Convert.ToDouble(jzsc));
                }


                //已存在的yysj或yyxh不再插入
                if (ldt_yysj > DateTime.Now &&! IsTimePointBooked(ldt_yysj, i, pbid.ToString()))
                {
                    list.Add(new BookableTimePoint { AppointSequence= i, AppointTime= ldt_yysj.ToLocalTime() });
                }

            }
            #endregion

        }
        #endregion

        /// <summary>
        /// 检查当前时间点或者预约序号是否被占用
        /// </summary>
        /// <param name="bookTime">预约时间</param>
        /// <param name="bookSequence">预约序号</param>
        /// <param name="arrangeId">排班标识</param>
        /// <returns></returns>
        public bool IsTimePointBooked(DateTime? bookTime, int? bookSequence, string arrangeId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select count(yysj) yyxh from yyfz_yyxx where (yysj=:BookTime or yyxh=:BookSequence) and ztbz<>'9' and pbid=:ArrangeId";
                var condition = new { BookTime=bookTime, BookSequence =bookSequence,arrangeId=Int64.Parse(arrangeId) };

                var result = con.Query<Int32>(command, condition).FirstOrDefault();
                if(result==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        

        /// <summary>
        /// 检查一个半天限约两个号源(温附一规则)
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <param name="arrangeId">排班标识</param>
        /// <returns></returns>
        private bool VerifyAppointCountExceed(String patientId,String arrangeId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select count(a.lxdh) yyxh from yyfz_yyxx a,yyfz_yspb b,cw_khxx c where c.brbh=:PatientId and b.pbid=:ArrangeId and 
                                            ((c.lxdh is not null and a.lxdh like '%'||c.lxdh||'%')or(c.yddh is not null and  a.lxdh like '%'||c.yddh||'%')) 
                                            and a.yysj>=b.sbsj  and a.yysj<=b.xbsj and (a.ztbz='1' or a.ztbz='2') and (a.zllx='02' or a.zllx='04' or a.zllx='07')";

                var condition = new { PatientId =patientId, ArrangeId =arrangeId};

                var result = con.Query(command, condition).FirstOrDefault();

                if((Int32)result.YYXH>=2)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }



        /// <summary>
        /// 验证在当前排班，患者是否已经预约过了
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <param name="arrangeId">排班标识</param>
        /// <returns></returns>
        private bool VerifyAlreadyBookedInThisArrange(String patientId, String arrangeId,BookHistory bookHistory)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT FZYYID,YYXH,ZTBZ,YYSJ,XGSJ,YSYHID,SFZ,BRXM,LXDH,GZDM,ZKID,ZLLX
                                            FROM YYFZ_YYXX WHERE ZTBZ<>'9' AND BRBH=:PatientId and pbid=:ArrangeId";
                var condition = new { PatientId = patientId, ArrangeId = Int64.Parse(arrangeId) };
                var result = con.Query(command, condition).FirstOrDefault();

                if(result!=null)
                {
                    //返回预约信息

                    //获取排班信息
                    var arrangeInfo = GetArrangeInfo(arrangeId, con);
                    var doctor = basicInfoService.GetDoctorById((Int64)arrangeInfo.YSYHID + "");

                    //转换预约状态
                    var appointState = (String)result.ZTBZ;
                    var outAppointState = 0;
                    AppointState enumAppointState = AppointState.Appointing;
                    if(Int32.TryParse(appointState, out outAppointState))
                    {
                        enumAppointState = (AppointState)outAppointState;
                    }

                    bookHistory.AppointId = (Int64)result.FZYYID+"";
                    bookHistory.AppointSequence = result.YYXH == null ? 0 : (Int32)result.YYXH;
                    bookHistory.AppointState = enumAppointState;
                    bookHistory.AppointTime = result.YYSJ == null ? default(DateTime) : (DateTime)result.YYSJ;
                    bookHistory.ArrangeId = arrangeId;
                    bookHistory.Attention = "";
                    bookHistory.CreateTime = result.XGSJ == null ? default(DateTime) : (DateTime)result.XGSJ;
                    bookHistory.DoctorId = (Int64)arrangeInfo.YSYHID + "";
                    bookHistory.DoctorName = (String)arrangeInfo.YSXM;
                    bookHistory.PatientId = patientId;
                    bookHistory.PatientIdCard = result.SFZ==null?"":(String)result.SFZ;
                    bookHistory.PatientName = result.BRXM == null ? "" : (String)result.BRXM;
                    bookHistory.PatientMobile = result.LXDH == null ? "" : (String)result.LXDH;
                    bookHistory.RegisteredAmount = arrangeInfo.XMJE == null ? 0 : (decimal)arrangeInfo.XMJE;
                    bookHistory.Remark = "";
                    bookHistory.DeptId = (Int64)arrangeInfo.ZKID + "";
                    bookHistory.DeptName = basicInfoService.GetDeptName((Int64)arrangeInfo.ZKID + "");
                    bookHistory.DoctorJobTitle = doctor.JobTitle;
                    bookHistory.DiagnosisType = (string)arrangeInfo.ZLLX == "04" ? "2" : "1";
                    bookHistory.DoctorSex = doctor.Sex;


                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 只有支持预约到时间点才实现如下方法
        /// 验证此时间点是否被预约过了
        /// </summary>
        /// <param name="arrangeId">排班标识</param>
        /// <param name="appointTime">预约时间</param>
        /// <returns></returns>
        private bool VerifyAlreadyBookedInThisTime(string arrangeId,DateTime appointTime)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select count(pbid) YYXH from yyfz_yyxx where pbid=:ArrangeId and yysj=:AppointTime and ztbz<>'9'";
                var condition = new { ArrangeId=arrangeId, AppointTime =appointTime};

                var result = con.Query(command, condition).FirstOrDefault();
                if((Int32)result.YYXH>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 验证排班是否被取消
        /// </summary>
        /// <param name="arrangeId">排班标识</param>
        /// <returns></returns>
        private bool VerifyArrangeFunctional(string arrangeId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select count(*) pbid from yyfz_yspb where ztbz<>'9' and  pbid=:ArrangeId";

                var condition = new { ArrangeId =arrangeId };

                var result = con.Query(command,condition).FirstOrDefault();

                if((Int32)result.PBID<=0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }




        /// <summary>
        /// 预约行为
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>预约历史</returns>
        public ResponseMessage<BookHistory> MakeAnAppointment(MakeAnAppointment appointment)
        {
            var response = new ResponseMessage<BookHistory>();
            response.Body = new BookHistory();

            var appointTimeAndSequence = appointment.ArrangeIndicate.Split(new String[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
            appointment.AppointSequence = appointTimeAndSequence[0] == null ? 0 : Int32.Parse(appointTimeAndSequence[0]);
            appointment.AppointTime = appointTimeAndSequence[1] == null ? default(DateTime) : DateTime.Parse(appointTimeAndSequence[1]);

            if (VerifyArrangeFunctional(appointment.ArrangeId))
            {
                response.HasError = 1;
                response.ErrorMessage = "该排班已被取消";
                return response;
            }

            if (VerifyAlreadyBookedInThisTime(appointment.ArrangeId, appointment.AppointTime.Value))
            {
                response.HasError = 1;
                response.ErrorMessage = "该时间点已被其他人预约，请返回刷新";

                if (!String.IsNullOrEmpty(appointment.PatientId)&& VerifyAlreadyBookedInThisArrange(appointment.PatientId, appointment.ArrangeId, response.Body))
                {
                    response.HasError = 2;
                    response.ErrorMessage = "您在此排班已经有预约";
                }

                return response;
            }

            if (!String.IsNullOrEmpty(appointment.PatientId) && VerifyAppointCountExceed(appointment.PatientId, appointment.ArrangeId))
            {
                response.HasError = 1;
                response.ErrorMessage = "一个半天限约两个号源";
                return response;
            }

            //判断是否无卡预约
            var noCard = false;
            if(String.IsNullOrEmpty(appointment.PatientId))
            {
                noCard = true;
            }

            var patient = basicInfoService.GetPatientInfo(appointment.PatientId);

            //插入预约信息,插入预约流水信息
            using (var con = DapperFactory.CrateOracleConnection())
            {
                IDbTransaction transaction = con.BeginTransaction();
                //插入预约信息
                var appointId = CommonService.GetNextValue("seq_yyfz_yyxx_id");

                String insertAppointInfo = String.Empty;
                String insertAppointList = String.Empty;
                dynamic appointInfoCondition;
                dynamic appointListCondition;
                //获取排班信息
                var arrangeInfo = GetArrangeInfo(appointment.ArrangeId, con);

                if (!noCard)
                {
                    #region 有卡预约
                    insertAppointInfo = @"insert into yyfz_yyxx(FZYYID,FZZBH,BRBH,BRXM,BRXB,CSRQ,BZ,LXDH,SFZ,LXDZ,YYXH,PDH,YYSJ,PBID,GZDM,ZKID,YSYHID,YYFS,ZTBZ,ZLLX,GHID,DJRYID,CZZID,XGSJ,ZBYY,HJLX,DJSJ) 
                                                      values(:FZYYID,:FZZBH,:BRBH,:BRXM,:BRXB,:CSRQ,:BZ,:LXDH,:SFZ,:LXDZ,:YYXH,:PDH,:YYSJ,:PBID,:GZDM,:ZKID,:YSYHID,:YYFS,:ZTBZ,:ZLLX,:GHID,:DJRYID,:CZZID,:XGSJ,:ZBYY,:HJLX,:DJSJ)";

                    insertAppointList = @"insert into yyfz_yyls(BRBH,FZYYID,YYFSSJ,YYJZSJ,YSXM,ZTBZ) values(:BRBH,:FZYYID,:YYFSSJ,:YYJZSJ,:YSXM,:ZTBZ)";
                    appointInfoCondition = new
                    {
                        FZYYID= appointId,
                        FZZBH = arrangeInfo.FZZBH==null?"":(String)arrangeInfo.FZZBH,
                        BRBH = appointment.PatientId,
                        BRXM = patient.PatientName,
                        BRXB = patient.Sex=="男"?"1":"2",
                        CSRQ = patient.BornDate,
                        BZ = "",
                        LXDH = patient.Telephone==null?"": patient.Telephone,
                        SFZ = patient.IdCode,
                        LXDZ = patient.ContactAddress == null ? "" : patient.ContactAddress,
                        YYXH = appointment.AppointSequence,
                        PDH = appointment.AppointSequence,
                        YYSJ = appointment.AppointTime,
                        PBID = Int64.Parse(appointment.ArrangeId),
                        GZDM = arrangeInfo.GZDM==null?"": (string)arrangeInfo.GZDM,
                        ZKID = (Int64)arrangeInfo.ZKID,
                        YSYHID = (Int64)arrangeInfo.YSYHID,
                        YYFS = "8",
                        ZTBZ = "1",
                        ZLLX = (string)arrangeInfo.ZLLX,
                        GHID = 0,
                        DJRYID = 19058,
                        CZZID = 19058,
                        XGSJ = DateTime.Now,
                        ZBYY = arrangeInfo.ZBYY==null?"": (string)arrangeInfo.ZBYY,
                        HJLX = "1",
                        DJSJ = DateTime.Now
                    };
                    appointListCondition = new
                    {
                        BRBH = appointment.PatientId,
                        FZYYID = appointId,
                        YYFSSJ = DateTime.Now,
                        YYJZSJ = appointment.AppointTime,
                        YSXM = (string)arrangeInfo.YSXM, ZTBZ = "0"
                    };

                    #endregion
                }
                else
                {
                    #region 无卡预约

                    insertAppointInfo = @"insert into yyfz_yyxx(FZYYID,FZZBH,BRXM,BZ,LXDH,SFZ,YYXH,PDH,YYSJ,PBID,GZDM,ZKID,YSYHID,YYFS,ZTBZ,ZLLX,GHID,DJRYID,CZZID,XGSJ,ZBYY,HJLX,DJSJ) 
                                                      values(:FZYYID,:FZZBH,:BRXM,:BZ,:LXDH,:SFZ,:YYXH,:PDH,:YYSJ,:PBID,:GZDM,:ZKID,:YSYHID,:YYFS,:ZTBZ,:ZLLX,:GHID,:DJRYID,:CZZID,:XGSJ,:ZBYY,:HJLX,:DJSJ)";

                    insertAppointList = @"insert into yyfz_yyls(FZYYID,YYFSSJ,YYJZSJ,YSXM,ZTBZ) values(:FZYYID,:YYFSSJ,:YYJZSJ,:YSXM,:ZTBZ)";
                    appointInfoCondition = new
                    {
                        FZYYID = appointId,
                        FZZBH = arrangeInfo.FZZBH == null ? "" : (String)arrangeInfo.FZZBH,
                        //BRBH = appointment.PatientId,
                        BRXM = patient.PatientName,
                        //BRXB = patient.Sex,
                        //CSRQ = patient.BornDate,
                        BZ = "",
                        LXDH = appointment.Mobile,
                        SFZ = appointment.PatientIdCard,
                        //LXDZ = patient.ContactAddress,
                        YYXH = appointment.AppointSequence,
                        PDH = appointment.AppointSequence,
                        YYSJ = appointment.AppointTime,
                        PBID = Int64.Parse(appointment.ArrangeId),
                        GZDM = (string)arrangeInfo.GZDM,
                        ZKID = (Int64)arrangeInfo.ZKID,
                        YSYHID = (Int64)arrangeInfo.YSYHID,
                        YYFS = "8",
                        ZTBZ = "1",
                        ZLLX = (string)arrangeInfo.ZLLX,
                        GHID = 0,
                        DJRYID = 19058,
                        CZZID = 19058,
                        XGSJ = DateTime.Now,
                        ZBYY = (string)arrangeInfo.ZBYY,
                        HJLX = "1",
                        DJSJ = DateTime.Now
                    };
                    appointListCondition = new
                    {
                        //BRBH = appointment.PatientId,
                        FZYYID = appointId,
                        YYFSSJ = DateTime.Now,
                        YYJZSJ = appointment.AppointTime,
                        YSXM = (string)arrangeInfo.YSXM,
                        ZTBZ = "0"
                    };

                    #endregion
                }

                try
                {
                    con.Execute(insertAppointInfo, (object)appointInfoCondition);
                    con.Execute(insertAppointList, (object)appointListCondition);
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    response.HasError = 1;
                    response.ErrorMessage = "数据保存错误";

                    throw;
                }
                transaction.Commit();


                //返回预约历史信息
                response.HasError = 0;

                var doctor = basicInfoService.GetDoctorById((Int64)arrangeInfo.YSYHID + "");

                response.Body = new BookHistory
                {
                    AppointId = appointId.ToString(),
                    AppointSequence= appointment.AppointSequence.Value,
                    AppointState=AppointState.Appointing,
                    AppointTime=appointment.AppointTime.Value,
                    ArrangeId= appointment.ArrangeId,
                    Attention="",
                    CreateTime=DateTime.Now,
                    DoctorId= (Int64)arrangeInfo.YSYHID+"",
                    DoctorName= (string)arrangeInfo.YSXM,
                    PatientId= appointment.PatientId,
                    PatientIdCard= patient.IdCode,
                    PatientName= patient.PatientName,
                    PatientMobile= patient.Mobile,
                    RegisteredAmount=arrangeInfo.XMJE==null?0: (decimal)arrangeInfo.XMJE,
                    Remark="",
                    DeptId= (Int64)arrangeInfo.ZKID+"",
                    DeptName=basicInfoService.GetDeptName((Int64)arrangeInfo.ZKID + ""),
                    DoctorJobTitle= doctor.JobTitle,
                    DiagnosisType = (string)arrangeInfo.ZLLX == "04" ? "2" : "1",
                    DoctorSex=doctor.Sex
                };



            }

            return response;
        }


        /// <summary>
        /// 获得排班信息
        /// </summary>
        /// <param name="arrangeId">排班标识</param>
        /// <returns></returns>
        public dynamic GetArrangeInfo(string arrangeId,OracleConnection con)
        {
            var command = @"select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,pb.zllx,pb.fzzbh,
                                        (select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllxmc,(select count(*) 
                                        from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,
                                        pb.zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje 
                                        from yyfz_yspb pb where pb.pbid=:ArrangeId";
            var condition = new { ArrangeId=Int64.Parse(arrangeId) };

            return con.Query(command, condition).FirstOrDefault();
        }
       





        /// <summary>
        /// 获取医生照片
        /// </summary>
        /// <param name="doctorId">温附一人员库标识</param>
        /// <returns></returns>
        private byte[] GetDoctorPhoto(string doctorId,OracleConnection con)
        {
            var command = @"SELECT RYPIC FROM R_RYK WHERE ID=:DoctorId";
            var condition = new { DoctorId=Int32.Parse(doctorId) };
            var result = con.Query(command, condition).FirstOrDefault();


            return (byte[])result.RYPIC;

        }




        /// <summary>
        /// 获取挂号费用
        /// </summary>
        /// <param name="diagnosisType">诊疗类型</param>
        /// <param name="jobTitle">挂牌工种</param>
        private decimal GetRegisteredAmount(string diagnosisTypeId,string jobTitleId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"select zllx,gzdm,xmid,xmje from cw_zllxghxm";
                //var result = new Tuple<String,String,Int32,decimal>()
                var result = con.Query(command).ToList();

                var amount = default(decimal);
                foreach (var item in result)
                {
                    if((string)item.ZLLX== diagnosisTypeId&&(string)item.GZDM== jobTitleId)
                    {
                        amount += (decimal)item.XMJE;
                    }
                }

                return amount;

            }
        }





        private Int32 GetUesdBookNum(String arrangeId,OracleConnection con)
        {
            var command = @"SELECT COUNT(*) AS Num FROM YYFZ_YYXX WHERE ZTBZ<>'9' AND PBID=:ArrangeId";
            var condition = new { ArrangeId=arrangeId };

            var result = con.Query(command, condition).FirstOrDefault();
            return (Int32)result.NUM;

        }


        /// <summary>
        /// 缓存获取挂号费用的辅助键值对信息
        /// 诊疗类型、工种代码，收费项目
        /// </summary>
        /// <returns></returns>
        private Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>> CacheAuxiliaryData()
        {
            //获取诊疗类型键值对
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var diagnosisTypesCommand = @"select DM,MC from  xtgl_ddlbn where lb='0051'";
                var jobTitlesCommand = @"select DM,MC from s_gz_zwdm where DM!='0000'";
                var itemTypesCommand = @"select XMID,MC from cw_sfxm where xmid in (select xmid from  cw_zhxmmx where zhid=12998)";

                var diagnosisTypes = con.Query(diagnosisTypesCommand).ToDictionary(k => (string)k.DM, v => (string)v.MC);
                var jobTitles = con.Query(jobTitlesCommand).ToDictionary(k => (string)k.DM, v => (string)v.MC);
                var itemTypes = con.Query(itemTypesCommand).ToDictionary(k => (string)k.XMID, v => (string)v.MC);

                var auxiliaryData = new Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, string>>(diagnosisTypes, jobTitles, itemTypes);
                //插入缓存
                CacheProvider.Set("ehps_auxiliaryData", auxiliaryData);

                return auxiliaryData;
            }
        }


    }
}
