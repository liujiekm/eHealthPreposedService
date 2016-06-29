using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public partial class dl_yyfz_fjxx
    {
        /// <summary>
        /// 根据房价id获取房间信息 有用缓存 过期时间120分钟
        /// </summary>
        /// <param name="fjid"></param>
        /// <returns></returns>
        public m_yyfz_fjxx GetInfo(int fjid)
        {
            var list = Lenovo.Tool.CacheHelper.Get<IList<m_yyfz_fjxx>>("dl_yyfz_fjxxAllGetInfo");
            if (list == null)
            {
                    list = this.GetAll();
                    Lenovo.Tool.CacheHelper.Insert("dl_yyfz_fjxxAllGetInfo", list, 120);
                
            }
            return list.Where(a => a.FJID == fjid).FirstOrDefault();
        }
    }
}
