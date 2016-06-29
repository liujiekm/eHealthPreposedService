using eHealth.Date.DAL;
using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Service
{
    public class MedicinesYZ
    {

        public IList<m_yl_mzypyz> GetYPYZByZLHDID(long zlhdid)
        {
            var list = new dl_yl_mzypyz().GetByZLHDID(zlhdid);
            if (list.Count <= 0) list = new dl_yl_mzypyz().GetByZLHDID_CG(zlhdid);
            foreach (var m in list)
            {
                m.PL = GetPLMCByDM(m.PL);
                m.FF = GetFFMCByDM(m.FF);
                m.GYSJ = GetSJMCByDM(m.GYSJ);
                m.YFYL = m.FF + " " + m.PL + " " + m.GYSJ;
            }
            return list;
        }
        /// <summary>
        /// 获取药品医嘱用药频率名称根据代码
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="cdb"></param>
        /// <returns></returns>
        private string GetPLMCByDM(string dm)
        {
            if (string.IsNullOrWhiteSpace(dm)) return dm;
            var temp = Lenovo.Tool.CacheHelper.Get<string>("GetYYPLMCByDM" + dm);
            if (temp == null || string.IsNullOrWhiteSpace(temp))
            {
                var m = new dl_yjk_yypl().GetMCByDM(dm);//this nr_yjk_yypl
                if (m == null) return dm;
                temp = m.MC;
                Lenovo.Tool.CacheHelper.Insert("GetYYPLMCByDM" + dm, temp, 1440);
            }
            return temp;
        }
        /// <summary>
        /// 获取药品医嘱用药方法名称根据代码
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="cdb"></param>
        /// <returns></returns>
        private string GetFFMCByDM(string dm)
        {
            if (string.IsNullOrWhiteSpace(dm)) return dm;
            var temp = Lenovo.Tool.CacheHelper.Get<string>("GetYYFFMCByDM" + dm);
            if (temp == null || string.IsNullOrWhiteSpace(temp))
            {
                var m = new dl_yjk_yyff().GetMCByDM(dm);//this nr_yjk_yyff
                if (m == null) return dm;
                temp = m.MC;
                Lenovo.Tool.CacheHelper.Insert("GetYYFFMCByDM" + dm, temp, 1440);
            }
            return temp;
        }
        /// <summary>
        /// 获取药品医嘱用药时间名称根据代码
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="cdb"></param>
        /// <returns></returns>
        private string GetSJMCByDM(string dm)
        {
            if (string.IsNullOrWhiteSpace(dm)) return dm;
            var temp = Lenovo.Tool.CacheHelper.Get<string>("GetYYSJMCByDM" + dm);
            if (temp == null || string.IsNullOrWhiteSpace(temp))
            {
                var m = new dl_yjk_ddlbn().GetMCByDM(dm);//this nr_yjk_ddlbn
                if (m == null) return dm;
                temp = m.MC;
                Lenovo.Tool.CacheHelper.Insert("GetYYSJMCByDM" + dm, temp, 1440);
            }
            return temp;
        }
    }
}
