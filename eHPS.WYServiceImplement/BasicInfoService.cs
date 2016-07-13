//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基础信息服务，温附一HIS系统实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 11:14:54
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

using Dapper;
using eHPS.Contract.Model;
using eHPS.CrossCutting.NetFramework.Caching;

namespace eHPS.WYServiceImplement
{
    public class BasicInfoService : IBasicInfo
    {
        public List<Department> GetDepts(string areaId)
        {
            var depts = new List<Department>();
            if(CacheProvider.Exist("ehps_depts"))
            {
                depts = (List<Department>)CacheProvider.Get("ehps_depts");
            }
            else
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    var command = @"SELECT BMID AS DeptId,BMMC AS DeptName,' ' AS ParentDeptId  FROM XTGL_BMDM WHERE SJBM=1 AND YQDM=:AreaId";
                    var condition = new { AreaId=areaId };
                    depts = con.Query<Department>(command, condition).ToList();
                    CacheProvider.Set("ehps_depts", depts);
                }
            }
            return depts;
        }

        /// <summary>
        /// 获取科室名称
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public string GetDeptName(string deptId)
        {
            var depts = new List<Department>();
            if (CacheProvider.Exist("ehps_allDepts"))
            {
                depts = (List<Department>)CacheProvider.Get("ehps_depts");
            }
            else
            {
                using (var con = DapperFactory.CrateOracleConnection())
                {
                    var command = @"SELECT BMID AS DeptId,BMMC AS DeptName,' ' AS ParentDeptId  FROM XTGL_BMDM WHERE SJBM=1 ";
                    depts = con.Query<Department>(command).ToList();
                    CacheProvider.Set("ehps_allDepts", depts);
                }
            }
            return depts.Where(p => p.DeptId == deptId).Select(p => p.DeptName).FirstOrDefault();
        }


        /// <summary>
        /// 根据用户标识获取用户姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public string GetNameById(string userId)
        //{
        //    using (var con = DapperFactory.CrateOracleConnection())
        //    {
        //        var command = @"select yhxm from xtgl_yhxx where yhid =:UserId";

        //        var condition = new { UserId = userId };

        //        var result = con.Query(command, condition).FirstOrDefault();

        //        return (string)result.yhxm;
        //    }
        //}



        public Doctor GetDoctorById(string doctorId)
        {
            var doctor = default(Doctor);
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT A.YHID, A.RYKID,A.XM,A.XB,A.GZDM,B.JSNR,B.PIC,A.XZKID FROM YL_RYK A LEFT JOIN YYFZ_YSJS B ON A.RYKID=B.RYKID WHERE  A.ZTBZ=1 AND A.YHID=:DoctorId";
                var condition = new { DoctorId = doctorId };
                var result = con.Query(command, condition).FirstOrDefault();
                if (result!=null)
                {
                    doctor = new Doctor
                    {
                        DeptId = (Int32)result.XZKID+"",
                        DeptName = CommonService.GetDeptName((Int32)result.XZKID),
                        DoctorId = ((Int64)result.YHID).ToString(),
                        DoctorName = (string)result.XM,
                        Expert = "",
                        Introduction = (string)result.JSNR,
                        JobTitle = CommonService.GetJobTitle((string)result.GZDM),
                        Sex = (string)result.XB == "1" ? "男" : "女",
                        Photo = (byte[])result.PIC
                    };
                    
                }

            }

            return doctor;
        }


        /// <summary>
        /// 医生标识
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public List<Doctor> GetDoctors(string deptId)
        {
            var doctors = new List<Doctor>();
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT A.YHID, A.RYKID,A.XM,A.XB,A.GZDM,B.JSNR,B.PIC FROM YL_RYK A LEFT JOIN YYFZ_YSJS B ON A.RYKID=B.RYKID WHERE  A.ZTBZ=1 AND A.XZKID=:DeptId";
                var condition = new { DeptId=deptId };
                var result = con.Query(command, condition).ToList();
                foreach (var item in result)
                {
                    var doctor = new Doctor {
                         DeptId=deptId,
                         DeptName=CommonService.GetDeptName(Int32.Parse(deptId)),
                         DoctorId=((Int64)item.YHID).ToString(),
                         DoctorName=(string)item.XM,
                         Expert="",
                         Introduction=(string)item.JSNR,
                         JobTitle=CommonService.GetJobTitle((string)item.GZDM),
                         Sex=(string)item.XB=="1"?"男":"女",
                         Photo=(byte[])item.PIC
                    };
                    doctors.Add(doctor);
                }

            }
            return doctors;
        }



        /// <summary>
        /// 根据患者标识获取患者基本信息
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public Patient GetPatientInfo(string patientId)
        {
            using (var con = DapperFactory.CrateOracleConnection())
            {
                var command = @"SELECT BRBH,DWDM,KNSJ,BRXM,BRXB,GJDM,HYZK,ZYDM,JGDM,
                                            MZDM,SFZH,CSRQ,XZZDM,LXDZ,LXDH,YDDH,SZSJ,ZTBZ FROM CW_KHXX WHERE BRBH = :PATIENTID";
                var condition = new { PatientId =patientId};

                var result = con.Query(command, condition).FirstOrDefault();

                if(result!=null)
                {
                    var patient = new Patient {
                            PatientId =(string)result.BRBH,
                            PatientName=(string)result.BRXM,
                            BornDate=(DateTime)result.CSRQ,
                            ContactAddress=(string)result.LXDZ,
                            IdCode=(string)result.SFZH,
                            Mobile=(string)result.YDDH,
                            Sex=(string)result.BRXB,
                            Telephone=(string)result.LXDH,
                            FirstTimeDiagnosis=(DateTime)result.SZSJ
                    };
                    return patient;
                }
                else
                {
                    return default(Patient);
                }

           
            }
        }
    }
}
