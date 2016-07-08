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

namespace eHPS.WYServiceImplement
{
    public class AppointmentService : IAppointment
    {
        public void CancelTheAppointment(string apponintId)
        {
            throw new NotImplementedException();
        }

        public List<BookHistory> GetAppointmentHistory(string patientId)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 获取医生可预约信息
        /// </summary>
        /// <param name="deptId">科室标识</param>
        /// <returns></returns>
        public List<BookableDoctor> GetBookableInfo(String doctorId, DateTime? startTime, DateTime? endTime)
        {
            var bookableDoctors = new List<BookableDoctor>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                //首先取医生排班信息
                var command = @"SELECT B.PBID,B.YSXM,B.YSYHID,B.RYKID,B.ZKID,R.GZDM2,B.ZLLX,B.SBSJ,B.XBSJ,B.ZKXH,B.YQDM,B.ZBYY 
                                            FROM YYFZ_YSPB B,R_RYK R WHERE B.ZTBZ='1'AND B.RYKID=R.ID AND R.GZDM2!='0000' AND B.RYKID =:DoctorId AND B.SBSJ >=:KSSJ and B.SBSJ<=:JSSJ";

                if(null==startTime)
                {
                    startTime = DateTime.Now;
                }
                if(null==endTime)
                {
                    endTime = DateTime.Now.AddDays(14);
                }
                var condition = new { DeDoctorIdptId = Int32.Parse(doctorId),KSSJ= startTime, JSSJ= endTime };
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
                        ArrangeStartTime = (DateTime)item.SBSJ,
                        ArrangeEndTime = (DateTime)item.XBSJ,
                        DeptId = ((Int32)item.ZKID).ToString(),
                        DeptName=CommonService.GetDeptName((Int32)item.ZKID),
                        DocotorId=((Int32)item.YSYHID).ToString(),
                        DoctorName=(String)item.YSXM,
                        JobTitleId=(String)item.GZDM2,
                        Photo = photo,
                        RegisteredAmount=GetRegisteredAmount((string)item.ZLLX, (String)item.GZDM2),
                        IsSpecialDisease=item.ZBYY==null?false:(string)item.ZBYY=="01"?true:false,
                        JobTitle =CommonService.GetJobTitle((String)item.GZDM2),
                        SpecialDiseaseState="",
                        SumBookNum=(Int32)item.ZKXH,
                        UsedBookNum=GetUesdBookNum(((Int32)item.PBID).ToString(),con)
                    };
                    bookableDoctors.Add(bookableDoctor);
                }
            }

            return bookableDoctors;
        }



        /// <summary>
        /// 预约行为
        /// </summary>
        /// <param name="appointment"></param>
        public void MakeAnAppointment(MakeAnAppointment appointment)
        {





            throw new NotImplementedException();
        }

        public List<BookableDoctor> PushBookableDoctors()
        {
            throw new NotImplementedException();
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
