using eHealth.Date.DAL;
using eHealth.Date.Entity;
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

    public partial class HospitalController 
    {
        

        /// <summary>
        /// 模块：门诊预约
        /// 获取医院科室树，包含子科室
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseDep AllDep()//ResponseDep
        {
            try
            {
                //var dl = new dl_yyfz_zkfj_wh();
                //var list = dl.GetFirstFDM();
                
                //var data = new List<Dep>();
                //foreach (var item in list)
                //{
                //    var tempcChildrens = dl.GetByFDM(item.DM);
                //    if (tempcChildrens.Count > 0)
                //    {
                //        var childrens = new List<Dep>();
                //        foreach (var children in tempcChildrens)
                //        {
                //            childrens.Add(new Dep() { ID = children.DM, Title = children.MC, });
                //        }
                //        data.Add(new Dep() { ID = item.DM, Title = item.MC, Childrens = childrens });
                //    }
                //    else
                //    {
                //        data.Add(new Dep() { ID = item.DM, Title = item.MC });
                //    }
                //}
                //return new ResponseDep() {  Data=data};


                var dl = new dl_yyfz_zkfj_wh();
                var alllist = dl.GetAll();//
                var root = alllist.Where(a => a.FDM == "1");

                var data = new List<Dep>();
                foreach (var item in root)
                {
                    var tempcChildrens = alllist.Where(a=>a.FDM==item.DM).ToList();
                    if (tempcChildrens.Count > 0)
                    {
                        var childrens = new List<Dep>();
                        foreach (var children in tempcChildrens)
                        {
                            childrens.Add(new Dep() { ID = children.DM, Title = children.MC, });
                        }
                        data.Add(new Dep() { ID = item.DM, Title = item.MC, Childrens = childrens });
                    }
                    else
                    {
                        data.Add(new Dep() { ID = item.DM, Title = item.MC });
                    }
                }
                return new ResponseDep() { Data = data };
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseDep() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        #region 预约

        /// <summary>
        /// 模块：门诊预约
        /// 获取指定科室下的所有医生及该医生是否可以预约
        /// </summary>
        /// <param name="param">科室id</param>
        /// <returns>State=是否可以预约 1可以预约，2已经预满，3已经过期，4其他原因不可预约</returns>
        [HttpPost]
        public ResponseDoctorAppiontment DepDoctorAppiontment(Param param)
        {
             //param = ConverToT<Param>();
            if (param == null)
                return new ResponseDoctorAppiontment() { HasError = 1, ErrorMessage = "参数为null！" };
            if (string.IsNullOrWhiteSpace(param.ID))
                return new ResponseDoctorAppiontment() { HasError = 1, ErrorMessage = "参数为null！" };
            try
            {
                //查询数据
                var list = new DoctorInfo().GetDoctorByDM(param.ID);
                //数据实体转换
                var result = new ResponseDoctorAppiontment();
                var data = new List<DoctorAppiontment>();
                foreach (var item in list)
                {
                    data.Add(pressToDoctorAppiontment(item));
                }
                
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseDoctorAppiontment() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 模块：门诊预约
        /// 获取指定医生的预约排班列表，含多科室预约列表
        /// </summary>
        /// <param name="DoctorCode">医生代码DoctorCode不能为null</param>
        /// <returns>多科室预约排班列表</returns>
        [HttpPost]
        public ResponseAppointment Appointments(RequestDoctor doctor)
        { //医生预约列表，含多科室预约列表
             //doctor = ConverToT<RequestDoctor>();
            if (doctor == null)
                return new ResponseAppointment() { HasError = 1, ErrorMessage = "参数不正确！" };
            if (string.IsNullOrWhiteSpace(doctor.DoctorCode))
                return new ResponseAppointment() { HasError = 1, ErrorMessage = "医生id不正确！" };
            try
            {
                var dl_pbxx=new PhoneYYXX();
                //数据查询
                var list = dl_pbxx.GetDoctorPBXX(Convert.ToInt64(doctor.DoctorCode));
                //实体转换
                var result = new ResponseAppointment();
                var data = new List<DepartmentSchediling>();
                foreach (var item in list.Keys)
                {
                    var pbList = list[item];
                    var pbDate=new List<SchedulingInfo>();
                    foreach (var pb in pbList)
	                {
                        pbDate.Add(new SchedulingInfo()
                        {
                            ID = pb.PBID.ToString(),
                            Charge = pb.XMJE.ToString(),
                            State = new dl_yyfz_pbxx().GetSYH(pb.SYH, pb.ZKXH.Value, pb.SBSJ.Value, pb.XBSJ.Value) == 0 ? 2 : 1,
                            Type = pb.ZLLX,
                            StartTime = pb.SBSJ.Value,
                            EndTime = pb.XBSJ.Value,
                            AppWay = 2,
                             HaveAppTimeList=1
                        });
	                }
                    data.Add(new DepartmentSchediling()
                    {
                        ID = item.ToString(),
                        Title = new dl_yyfz_zkfj_wh().GetMCByBMID((int)item),
                        Data = pbDate
                    });
                }
                
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointment() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：门诊预约
        /// 获取排班的时间点列表,若没有时间点列表则不需要此接口
        /// </summary>
        /// <param name="param">排班ID，ID不能为null</param>
        /// <returns>可预约列表</returns>
        [HttpPost]
        public ResponseAppointmentTime AppointmentTimes(Param param)
        {
            // param = ConverToT<Param>();
            if (param == null)
                return new ResponseAppointmentTime() { HasError = 1, ErrorMessage = "参数为null！" };
            if (string.IsNullOrWhiteSpace(param.ID))
                return new ResponseAppointmentTime() { HasError = 1, ErrorMessage = "参数为null！" };
            try
            {
                //查询数据
                var dl_pbxx = new PhoneYYXX();
                var list = dl_pbxx.GetDoctorAvailable(Convert.ToInt64(param.ID)).Select(p => new AppointmentTime() { AppTime = p.YYSJ, Index = p.YYXH, IsUseable = 1 }).ToList();
                //数据实体转换
                var result = new ResponseAppointmentTime();
                result.Data = list;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointmentTime() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：门诊预约
        /// 对排班进行预约，可进行身份证预约，姓名手机号预约，医院就诊卡号预约。传递的id：KeyID 为排班id
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="KeyID">排班id</param>
        /// <param name="AppTime">预约时间，有实现‘获取时间点列表’接口则此字段值为预约时间，否则为空</param>
        /// <param name="Index">预约序号，有实现‘获取时间点列表‘接口则此字段值为预约序号，否则为空</param>
        /// <param name="Remark">备注，有实现‘获取时间点列表’接口则此字段值为接口返回值‘备注’字段内容，否则为空</param>
        /// <returns>Code：大于0就是预约成功，否则预约失败，预约成功需返回具体预约信息</returns>
        [HttpPost]
        public ResponseAppointmentState DoAppointment(PatientInfo param)
        {
           // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "参数不正确！" };
            if (string.IsNullOrWhiteSpace(param.KeyID))
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "预约排版id不正确！" };

            try
            {
                ReturnCode returnCode = null ;
                //预约操作
                if (!string.IsNullOrWhiteSpace(param.HospitalCard))
                {
                    returnCode = new PhoneYYXX().SaveOrderMessage(param.HospitalCard,Convert.ToInt64(param.KeyID),param.AppTime,param.Index);
                }
                else if (!string.IsNullOrWhiteSpace(param.Name) && !string.IsNullOrWhiteSpace(param.PhoneNumber))
                {
                    returnCode = new PhoneYYXX().OrderWithoutLogin(param.Name, param.PhoneNumber, Convert.ToInt64(param.KeyID), param.AppTime, param.Index);
                }
                else
                {
                    return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "患者参数不全！" };
                }
                var result = new ResponseAppointmentState();
                if (returnCode.RET_CODE > 0)
                {//获取预约信息
                    var yyxx = new dl_yyfz_yyxx().GetByFZYYID(Convert.ToInt64(returnCode.RET_INFO));
                    string ysxm = "", type = "普通门诊", charge = "10元";
                    long rykid = 0;
                    //医生信息
                    var ysxx = new dl_r_ryk().GetDoctorByYHID(yyxx.YSYHID.Value);
                    if (ysxx != null)
                    {
                        rykid = ysxx.ID;
                        ysxm = ysxx.XM;
                    }
                    //排班信息
                    var pbxx = new dl_yyfz_pbxx().GetPBByID(yyxx.PBID.Value);
                    if (pbxx != null)
                    {
                        type = pbxx.ZLLX;
                        charge = pbxx.XMJE + "元";
                    }
                    var data = new AppointmentState()
                    {
                        Code = returnCode.RET_CODE,
                        Msg = returnCode.RET_INFO,
                        Appointment = new AppointmentHistory()
                        {
                            AppointmentCode = yyxx.FZYYID.ToString(),
                            AppWay = 1,
                            Card = yyxx.BRBH,
                            Charge = charge,
                            DoctorCode = rykid.ToString(),
                            DepName = new dl_yyfz_zkfj_wh().GetMCByBMID((int)yyxx.ZKID.Value),
                            DoctorName = ysxm,
                            AppointmentTime = yyxx.YYSJ.Value,
                            IDCard = yyxx.SFZ,
                            PhoneNumber = yyxx.LXDH,
                            State = Convert.ToInt32(yyxx.ZTBZ),
                            StateName = ConverToAppointmentStateName(Convert.ToInt32(yyxx.ZTBZ)),
                            Type = type,
                            UserName = yyxx.BRXM,
                             AppointmentNo=yyxx.YYXH,
                              ScheduleID=yyxx.PBID.ToString()
                        }
                    };
                    result.Data = data;
                }
                else
                {
                    result.Data = new AppointmentState()
                    {
                        Code = returnCode.RET_CODE,
                        Msg = returnCode.RET_INFO
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：门诊预约
        /// 获取用户预约历史，可使用身份证查询，姓名手机号查询，医院就诊卡号查询
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <returns>预约历史记录列表</returns>
        [HttpPost]
        public ResponseAppointmentHistory AppointmentHistory(PatientInfo param)
        {
            if (param == null)
                param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseAppointmentHistory() { HasError = 1, ErrorMessage = "参数不正确" };

            try
            {
                List<m_yyfz_yyxx> list = new List<m_yyfz_yyxx>();
                //查询预约历史记录
                if(!string.IsNullOrWhiteSpace(param.HospitalCard))//用卡号查询
                    list.AddRange(new dl_yyfz_yyxx().GetByBRBH(param.HospitalCard));
                else   //用姓名手机号查询
                    list.AddRange(new dl_yyfz_yyxx().GetByXMDH(param.Name,param.PhoneNumber));
                list=list.OrderByDescending(a=>a.YYSJ).ToList();
                //数据实体转换
                var result = new ResponseAppointmentHistory();
                var data = new List<AppointmentHistory>();
                long rykid=0;
                string ysxm = "", type = "普通门诊", charge = "10元";
                foreach (var item in list)
                {
                    //医生信息
                    var ysxx = new dl_r_ryk().GetDoctorByYHID(item.YSYHID.Value);
                    if (ysxx != null)
                    {
                        rykid = ysxx.ID;
                        ysxm = ysxx.XM;
                    }
                    //排班信息
                    var pbxx = new dl_yyfz_pbxx().GetPBByID(item.PBID.Value);
                    if (pbxx != null)
                    {
                        type = pbxx.ZLLX;
                        charge = pbxx.XMJE + "元";
                    }
                    data.Add(new AppointmentHistory()
                    {
                        AppointmentCode = item.FZYYID.ToString(),
                        AppWay = 1,
                        Card = item.BRBH,
                        Charge = charge,
                        DoctorCode = rykid.ToString(),
                        DepName = new dl_yyfz_zkfj_wh().GetMCByBMID((int)item.ZKID.Value),
                        DoctorName = ysxm,
                        AppointmentTime = item.YYSJ.Value,
                        IDCard = item.SFZ,
                        PhoneNumber = item.LXDH.Trim(','),
                        State = Convert.ToInt32(item.ZTBZ),
                        StateName = ConverToAppointmentStateName(Convert.ToInt32(item.ZTBZ)),
                        Type = type,
                        UserName = item.BRXM,
                        AppointmentNo = item.YYXH,
                        ScheduleID = item.PBID.ToString()
                    });
                }
                
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointmentHistory() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 模块：门诊预约
        /// 取消预约记录，传递的id：KeyID 为预约记录id
        /// </summary>
        /// <param name="KeyID">预约记录id</param>
        /// <returns>State大于0就是取消预约成功，操作成功需再次返回该条预约记录信息</returns>
        [HttpPost]
        public ResponseAppointmentState UnAppointment(PatientInfo param)
        { //取消预约
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "参数为null！" };
            if (string.IsNullOrWhiteSpace(param.KeyID))
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "KeyID为null！" };
            try
            {
                //取消预约，返回执行结果
                var temp = new PhoneYYXX().CancelOrder(Convert.ToInt64(param.KeyID));
                //结果转化
                var result = new ResponseAppointmentState();
                if (temp.RET_CODE > 0)
                {
                    //获取预约信息
                    var yyxx = new dl_yyfz_yyxx().GetByFZYYID(Convert.ToInt64(param.KeyID));
                    string ysxm = "", type = "普通门诊", charge = "10元";
                    long rykid = 0;
                    //医生信息
                    var ysxx = new dl_r_ryk().GetDoctorByYHID(yyxx.YSYHID.Value);
                    if (ysxx != null)
                    {
                        rykid = ysxx.ID;
                        ysxm = ysxx.XM;
                    }
                    //排班信息
                    var pbxx = new dl_yyfz_pbxx().GetPBByID(yyxx.PBID.Value);
                    if (pbxx != null)
                    {
                        type = pbxx.ZLLX;
                        charge = pbxx.XMJE + "元";
                    }
                    var data = new AppointmentState()
                    {
                        Code = temp.RET_CODE,
                        Msg = temp.RET_INFO,
                        Appointment = new AppointmentHistory()
                        {
                            AppointmentCode = yyxx.FZYYID.ToString(),
                            AppWay = 1,
                            Card = yyxx.BRBH,
                            Charge = charge,
                            DoctorCode = rykid.ToString(),
                            DepName = new dl_yyfz_zkfj_wh().GetMCByBMID((int)yyxx.ZKID.Value),
                            DoctorName = ysxm,
                            AppointmentTime = yyxx.YYSJ.Value,
                            IDCard = yyxx.SFZ,
                            PhoneNumber = yyxx.LXDH,
                            State = Convert.ToInt32(yyxx.ZTBZ),
                            StateName = ConverToAppointmentStateName(Convert.ToInt32(yyxx.ZTBZ)),
                            Type = type,
                            UserName = yyxx.BRXM,
                            AppointmentNo = yyxx.YYXH,
                            ScheduleID = yyxx.PBID.ToString()

                        }
                    };
                    result.Data = data;
                }
                else {
                    result.Data = new AppointmentState()
                    {
                        Code = temp.RET_CODE,
                        Msg = temp.RET_INFO
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：预约签到
        /// 获取预约签到小票信息
        /// </summary>
        /// <param name="param">预约记录id</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseSignInfo GetSignInfo(Param param)
        {
            // param = ConverToT<Param>();
            if (param == null)
                return new ResponseSignInfo() { HasError = 1, ErrorMessage = "参数为null！" };
            if (string.IsNullOrWhiteSpace(param.ID))
                return new ResponseSignInfo() { HasError = 1, ErrorMessage = "ID为null！" };
            try
            {
                //根据预约id返回房间信息
                var data = new PhoneYYXX().GetSignInInfo(Convert.ToInt64(param.ID));
                //结果转化
                var result = new ResponseSignInfo();
                result.Data = new Models.SignInInfo() { Area = data.Area, DepName = data.DepName, DoctorName = data.DoctorName, OrderIndex = data.OrderIndex, OrderTime = data.OrderTime, RoomName = data.RoomName, RoomSite = data.RoomSite, SignTime = data.SignTime, UserName = data .UserName};
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseSignInfo() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：预约签到
        /// 预约签到
        /// </summary>
        /// <param name="Name">患者真实姓名</param>
        /// <param name="IDCard">身份证</param>
        /// <param name="HospitalCard">就诊卡号</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="KeyID">排班id</param>
        /// <returns>Code：大于0就是成功，否则失败</returns>
        [HttpPost]
        public ResponseAppointmentState AppointmentSign(PatientInfo param)
        {
            // param = ConverToT<PatientInfo>();
            if (param == null)
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = "参数为null！" };
            try
            {
                //预约签到，返回执行结果
                var temp = new PhoneYYXX().SignInCard(Convert.ToInt64(param.KeyID), param.HospitalCard);
                //结果转化
                var result = new ResponseAppointmentState();
                if (temp.RET_CODE > 0)
                {//获取预约信息
                    var yyxx = new dl_yyfz_yyxx().GetByFZYYID(Convert.ToInt64(param.KeyID));
                    string ysxm = "", type = "普通门诊", charge = "10元";
                    long rykid = 0;
                    //医生信息
                    var ysxx = new dl_r_ryk().GetDoctorByYHID(yyxx.YSYHID.Value);
                    if (ysxx != null)
                    {
                        rykid = ysxx.ID;
                        ysxm = ysxx.XM;
                    }
                    //排班信息
                    var pbxx = new dl_yyfz_pbxx().GetPBByID(yyxx.PBID.Value);
                    if (pbxx != null)
                    {
                        type = pbxx.ZLLX;
                        charge = pbxx.XMJE + "元";
                    }
                    var data = new AppointmentState()
                    {
                        Code = temp.RET_CODE,
                        Msg = temp.RET_INFO,
                        Appointment = new AppointmentHistory()
                        {
                            AppointmentCode = yyxx.FZYYID.ToString(),
                            AppWay = 1,
                            Card = yyxx.BRBH,
                            Charge = charge,
                            DoctorCode = rykid.ToString(),
                            DepName = new dl_yyfz_zkfj_wh().GetMCByBMID((int)yyxx.ZKID.Value),
                            DoctorName = ysxm,
                            AppointmentTime = yyxx.YYSJ.Value,
                            IDCard = yyxx.SFZ,
                            PhoneNumber = yyxx.LXDH,
                            State = Convert.ToInt32(yyxx.ZTBZ),
                            StateName = ConverToAppointmentStateName(Convert.ToInt32(yyxx.ZTBZ)),
                            Type = type,
                            UserName = yyxx.BRXM,
                            AppointmentNo = yyxx.YYXH,
                            ScheduleID = yyxx.PBID.ToString()

                        }
                    };
                    result.Data = data;
                }
                else
                {
                    result.Data = new AppointmentState()
                    {
                        Code = temp.RET_CODE,
                        Msg = temp.RET_INFO
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseAppointmentState() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        #endregion

        /// <summary>
        /// 回复实体转换
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        private DoctorAppiontment pressToDoctorAppiontment(m_r_ryk doctorInfo)
        {
            return new DoctorAppiontment()
            {
                DoctorCode = doctorInfo.ID.ToString(),
                DoctorName = doctorInfo.XM,
                HospitalName = "温医一院",
                JobName = doctorInfo.ZW,
                PhotoUrl = doctorInfo.URL,
                Expert = doctorInfo.JSNR,
                Info = doctorInfo.JSNR,
                DepID = doctorInfo.ZKID.ToString(),
                DepName = new dl_yyfz_zkfj_wh().GetMCByBMID(doctorInfo.ZKID),    //专科名称取父级对外科室名称
                State = new dl_yyfz_pbxx().GetDoctorSYH(doctorInfo.ID,DateTime.Now.Date,DateTime.Now.Date.AddDays(14))==0?2:1
            };
        }

        /// <summary>
        /// 获取预约状态:1预约成功；2等待；3已经呼叫；4已经进入；5已经完成；9已经放弃
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public static string ConverToAppointmentStateName(int State)
        {
            switch (State)
            {
                case 1:
                    return "预约成功";
                case 2:
                    return "等待";
                case 3:
                    return "已经呼叫";
                case 4:
                    return "已经进入";
                case 5:
                    return "已经完成";
                case 9:
                    return "已经放弃";
                default:
                    return string.Empty;
            }
        }
    }
}
