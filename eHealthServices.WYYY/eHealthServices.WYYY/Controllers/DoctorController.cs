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
    public partial class HospitalController : ApiController
    {
        /// <summary>
        /// 获取医生基本信息
        /// </summary> 
        /// <param name="doctor">医院医生代码</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseDoctorBase DoctorBase(RequestDoctor doctor)
        {
            if(doctor==null)
                doctor = ConverToT<RequestDoctor>();
            if (doctor == null)
                return new ResponseDoctorBase() { HasError = 1, ErrorMessage = "参数不正确！" };
            if (string.IsNullOrWhiteSpace(doctor.DoctorCode))
                return new ResponseDoctorBase() { HasError = 1, ErrorMessage = "医生id不正确！" };
            try
            {
                //从数据库搜索信息
                var doctorInfo = new DoctorInfo().getDoctorByRykid(Convert.ToInt64(doctor.DoctorCode));
                var result = new ResponseDoctorBase();
                //实体转换
                var data = pressToDoctorBase(doctorInfo);
                if (data == null)
                    return new ResponseDoctorBase() { HasError = 1, ErrorMessage = "医生不存在！" };
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseDoctorBase() { HasError = 1, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// 搜索医生
        /// </summary>
        /// <param name="param">ID，搜索值内容</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseSearchDoctorBase SearchDoctorBase(Param param)
        {
             //param = ConverToT<Param>();
            if (param == null)
                return new ResponseSearchDoctorBase() { HasError = 1, ErrorMessage = "参数为null！" };
            try
            {
                //数据查询
                var docList = new DoctorInfo().GetDoctorByHZ(param.ID);
                var result = new ResponseSearchDoctorBase();
                var data = new List<DoctorBase>();
                //实体转换
                foreach (var item in docList)
                {
                    data.Add(pressToDoctorBase(item));
                }
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseSearchDoctorBase() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 回复实体转换
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        private DoctorBase pressToDoctorBase(m_r_ryk doctorInfo)
        {
            if (doctorInfo == null) return null;
            return new DoctorBase()
            {
                DoctorCode = doctorInfo.ID.ToString(),
                DoctorName = doctorInfo.XM,
                HospitalName = "温医一院",
                JobName = doctorInfo.ZW,
                PhotoUrl = doctorInfo.URL,
                Expert = doctorInfo.JSNR,
                Info = doctorInfo.JSNR,
                DepID = doctorInfo.ZKID.ToString(),
                 Sex=doctorInfo.XB,
                DepName = new dl_yyfz_zkfj_wh().GetMCByBMID(doctorInfo.ZKID)//专科名称取父级对外科室名称
            };
        }
    }
}
