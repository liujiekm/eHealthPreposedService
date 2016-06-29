using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Date.DAL
{
    public partial class dl_yyfz_pbxx
    {
        /// <summary>
        /// 获取医生剩余号
        /// </summary> 
        /// <param name="rykid"></param>
        /// <param name="start"></param> 
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetDoctorSYH(long rykid,DateTime start,DateTime end)
        {//select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.sbsj ,pb.xbsj, (select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,pb.zkxh  from yyfz_yspb pb where pb.ztbz='1' and pb.rykid="+rykid+" and  pb.sbsj >=:KSSJ and pb.sbsj<=:JSSJ  and pb.zllx in('02','04','07')";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":KSSJ", start));
                    command.Parameters.Add(new OracleParameter(":JSSJ", end));
                    var result = OracleHelper.GetDataItems<m_yyfz_pbxx>(command);

                    int syh = 0;
                    foreach (var item in result)
                    {
                        syh += GetSYH(item.SYH, item.ZKXH.Value, item.SBSJ.Value, item.XBSJ.Value);
                    }
                    return syh;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 计算剩余号
        /// </summary>
        /// <param name="yys">已预约数</param>
        /// <param name="zkxh">专科限号</param>
        /// <param name="sbsj">上班时间</param>
        /// <param name="xbsj">下班时间</param>
        /// <returns></returns>
        public int GetSYH(int yys,long zkxh,DateTime sbsj,DateTime xbsj)
        {
            int syh = 0;
            if (xbsj <= DateTime.Now)
            {
                syh = 0;
            }
            else if (xbsj.Day == DateTime.Now.Day && sbsj <= DateTime.Now)//如果是同一天
            {
                long alltime = xbsj.Ticks - sbsj.Ticks;
                long sytime = xbsj.Ticks - DateTime.Now.Ticks;
                long hs = zkxh * sytime / alltime;

                syh = (int)hs - yys;

                if (syh == 1)
                {
                    syh = 0;
                }
            }
            else
            {
                syh = (int)zkxh - yys;
            }
            syh = syh < 0 ? 0 : syh;
            return syh;
        }
    }
}