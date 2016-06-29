using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.ComponentModel;
namespace eHealth.Date.DAL
{

    #region 注释：eHealth.Date.Entity.m_cw_ghmx挂号明细
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 挂号明细
    /// </summary>
    public partial class dl_cw_ghmx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 查看预约是否存在挂号记录
        /// </summary>
        /// <param name="YYID">查询方法所需参数YYID</param>
        /// <returns>返回eHealth.Date.Entity.m_cw_ghmx实体</returns>
        public eHealth.Date.Entity.m_cw_ghmx GetByYYID(Int64 YYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select GHID from cw_ghmx where YYID=:YYID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":YYID",YYID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_cw_ghmx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_cw_ghmx();
            }
        }

        /// <summary>
        /// 修改方法，生成时间：2015-07-13 16:50:33
        /// 更新挂号信息为还未使用
        /// </summary>
        /// <param name="updateYYID">updateYYID</param>
        /// <param name="whereGHID">whereGHID</param>
        /// <returns></returns>
        public int UpdateYYIDByGHID(Int64 updateYYID,Int64 whereGHID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":updateYYID",updateYYID));
                    
                    command.Parameters.Add(new OracleParameter(":GHID",whereGHID));
                    command.CommandText = "update cw_ghmx set YYID=:updateYYID where GHID=:GHID";
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_i_ysxx医生信息
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 医生信息
    /// </summary>
    public partial class dl_i_ysxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据ID获取医生信息
        /// </summary>
        /// <param name="id">查询方法所需参数id</param>
        /// <returns>返回eHealth.Date.Entity.m_i_ysxx实体</returns>
        public eHealth.Date.Entity.m_i_ysxx GetDoctorByID(Int32 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select id,yszp,xm,ksmc,ksmc1 from i_ysxx where id=:id";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":id",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_i_ysxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_i_ysxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据科室获取医生
        /// </summary>
        /// <param name="ksmc">查询方法所需参数ksmc</param>
        /// <param name="ksmc1">查询方法所需参数ksmc1</param>
        /// <returns>返回eHealth.Date.Entity.m_i_ysxx实体</returns>
        public IList<eHealth.Date.Entity.m_i_ysxx> GetDoctorsByKS(String KSMC,String KSMC1)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select ksmc,id,xm,jj,zc,yszp,ksmc1,xh,xh1 from i_ysxx where  shzt='已审核' and flag=1 and (ksmc in (:ksmc) or ksmc1 in (:ksmc1))";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ksmc",KSMC));
                    command.Parameters.Add(new OracleParameter(":ksmc1",KSMC1));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_i_ysxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_i_ysxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据ID获取医生简介
        /// </summary>
        /// <param name="id">查询方法所需参数id</param>
        /// <returns>返回eHealth.Date.Entity.m_i_ysxx实体</returns>
        public eHealth.Date.Entity.m_i_ysxx GetDocInfoByID(Int32 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zc,mzsj,xm,jj from i_ysxx where id=:id";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":id",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_i_ysxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_i_ysxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据医生姓名获取医生照片
        /// </summary>
        /// <param name="xm">查询方法所需参数xm</param>
        /// <returns>返回eHealth.Date.Entity.m_i_ysxx实体</returns>
        public eHealth.Date.Entity.m_i_ysxx GetDoctorPicByName(String XM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select id,yszp from i_ysxx where xm=:xm and shzt='已审核' and xm not in ('卢才教','何金彩','褚茂平','周颖') ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":xm",XM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_i_ysxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_i_ysxx();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_l_testresult化验结果表
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 化验结果表
    /// </summary>
    public partial class dl_l_testresult
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据条形码获取数据
        /// </summary>
        /// <param name="TXM">查询方法所需参数TXM</param>
        /// <returns>返回eHealth.Date.Entity.m_l_testresult实体</returns>
        public IList<eHealth.Date.Entity.m_l_testresult> GetListByTXM(String TXM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "SELECT L_TESTDESCRIBE.IMPORTANT,L_TESTDESCRIBE.CHINESENAME,L_TESTRESULT.UNIT, L_TESTRESULT.HINT,lpad(reflo,7,' ')||decode(nvl(length(refhi),0),0,'','～')||rpad(nvl(refhi,' '),decode(nvl(length(refhi),0),0,5 - length(reflo),7),' ') ckfw,L_TESTRESULT.TESTRESULT,L_TESTRESULT.TESTID FROM L_TESTDESCRIBE,L_TESTRESULT WHERE L_TESTDESCRIBE.TESTID = L_TESTRESULT.TESTID and L_TESTRESULT.SAMPLENO  in (SELECT sampleno from L_PATIENTINFO where  doctadviseno =:TXM)  AND    L_TESTRESULT.ISPRINT = 1 and L_TESTRESULT.Teststatus >=4 ORDER BY L_TESTDESCRIBE.PRINTORD ASC ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":TXM",TXM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_l_testresult>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_l_testresult>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_r_ryk人员库信息
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 人员库信息
    /// </summary>
    public partial class dl_r_ryk
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取医生姓名，专科，人员库id，通过医生用户id
        /// </summary>
        /// <param name="YSYHID">查询方法所需参数YSYHID</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public eHealth.Date.Entity.m_r_ryk GetDoctorByYHID(Int64 YSYHID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select xm,rykid id,xzkid zkid from yl_ryk where yhid=:YSYHID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":YSYHID",YSYHID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_r_ryk();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据rykid获取医生专科信息
        /// </summary>
        /// <param name="ID">查询方法所需参数ID</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public eHealth.Date.Entity.m_r_ryk GetZKByRykid(Int64 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select xzkid zkid,mc from yl_ryk r,yyfz_zkfj_wh d where r.xzkid=d.bmid and r.rykid=:ID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ID",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_r_ryk();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据搜索内容获取所有医生的信息
        /// </summary>
        /// <param name="XM">查询方法所需参数XM</param>
        /// <param name="XB">查询方法所需参数XB</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public IList<eHealth.Date.Entity.m_r_ryk> GetDoctorByHZ(String XM,String XB)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid=r.id and r.id in (select distinct rykid from yyfz_yspb where ztbz='1' and zllx<>'04' and zllx<>'15' and sbsj>sysdate-100 and zbyy is null ) and (r.py like :XM  or r.xm like :XB)";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":XM",XM));
                    command.Parameters.Add(new OracleParameter(":XB",XB));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_r_ryk>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据人员库id获取用户性别
        /// </summary>
        /// <param name="id">查询方法所需参数id</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public eHealth.Date.Entity.m_r_ryk GetXBByID(Int64 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select xb from r_ryk where id=:id";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":id",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_r_ryk();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据rykid获取医生的信息
        /// </summary>
        /// <param name="ID">查询方法所需参数ID</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public eHealth.Date.Entity.m_r_ryk GetDoctorByRykid(Int64 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid(+)=r.id and r.id =:ID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ID",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_r_ryk();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据部门id获取科室下的所有医生的信息
        /// </summary>
        /// <param name="zkid">查询方法所需参数zkid</param>
        /// <returns>返回eHealth.Date.Entity.m_r_ryk实体</returns>
        public IList<eHealth.Date.Entity.m_r_ryk> GetDoctorByBMID(Int32 ZKID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select r.id,r.xm,xb,r.gzdm,r.gzdm2,r.zy_new zw,y.jsnr from r_ryk r,YYFZ_YSJS y where y.rykid=r.id and r.id in (select distinct rykid from yyfz_yspb where ztbz='1' and zllx<>'04' and zllx<>'15' and sbsj>sysdate-100 and zkid=:zkid and zbyy is null )";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zkid",ZKID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_r_ryk>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_r_ryk>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_xtgl_ism_sendmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_xtgl_ism_send
    {
        /// <summary>
        /// 添加方法，生成时间：2015-07-13 16:50:33
        /// 添加短信发送信息
        /// </summary>
        /// <param name="model">添加对象</param>
        /// <returns>大于0为添加成功</returns>
        public int AddSend(eHealth.Date.Entity.m_xtgl_ism_send model)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    var fields = new List<string>();
                    command.Parameters.Clear();
                    if(model.FSID!=null) fields.Add("FSID");if(model.FSID!=null) command.Parameters.Add(new OracleParameter(":FSID", model.FSID));
                    if(model.MBS!=null) fields.Add("MBS");if(model.MBS!=null) command.Parameters.Add(new OracleParameter(":MBS", model.MBS));
                    if(model.XXLX!=null) fields.Add("XXLX");if(model.XXLX!=null) command.Parameters.Add(new OracleParameter(":XXLX", model.XXLX));
                    if(model.XXLB!=null) fields.Add("XXLB");if(model.XXLB!=null) command.Parameters.Add(new OracleParameter(":XXLB", model.XXLB));
                    if(model.XXNR!=null) fields.Add("XXNR");if(model.XXNR!=null) command.Parameters.Add(new OracleParameter(":XXNR", model.XXNR));
                    if(model.DSFSSJ!=null) fields.Add("DSFSSJ");if(model.DSFSSJ!=null) command.Parameters.Add(new OracleParameter(":DSFSSJ", model.DSFSSJ));
                    if(model.ZTBZ!=null) fields.Add("ZTBZ");if(model.ZTBZ!=null) command.Parameters.Add(new OracleParameter(":ZTBZ", model.ZTBZ));
                    if(model.BJSJ!=null) fields.Add("BJSJ");if(model.BJSJ!=null) command.Parameters.Add(new OracleParameter(":BJSJ", model.BJSJ));
                    if(model.BJYHID!=null) fields.Add("BJYHID");if(model.BJYHID!=null) command.Parameters.Add(new OracleParameter(":BJYHID", model.BJYHID));
                    if(model.SHSJ!=null) fields.Add("SHSJ");if(model.SHSJ!=null) command.Parameters.Add(new OracleParameter(":SHSJ", model.SHSJ));
                    if(model.SHBMID!=null) fields.Add("SHBMID");if(model.SHBMID!=null) command.Parameters.Add(new OracleParameter(":SHBMID", model.SHBMID));
                    if(model.SHYHID!=null) fields.Add("SHYHID");if(model.SHYHID!=null) command.Parameters.Add(new OracleParameter(":SHYHID", model.SHYHID));
                    if(model.BZ!=null) fields.Add("BZ");if(model.BZ!=null) command.Parameters.Add(new OracleParameter(":BZ", model.BZ));
                    command.CommandText = string.Format("insert into xtgl_ism_send({0}) values(:{1})",string.Join(",",fields),string.Join(",:",fields));
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_xtgl_ism_sendmxmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_xtgl_ism_sendmx
    {
        /// <summary>
        /// 添加方法，生成时间：2015-07-13 16:50:33
        /// 添加短信明细信息
        /// </summary>
        /// <param name="model">添加对象</param>
        /// <returns>大于0为添加成功</returns>
        public int AddMX(eHealth.Date.Entity.m_xtgl_ism_sendmx model)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    var fields = new List<string>();
                    command.Parameters.Clear();
                    if(model.FSID!=null) fields.Add("FSID");if(model.FSID!=null) command.Parameters.Add(new OracleParameter(":FSID", model.FSID));
                    if(model.FSSJ!=null) fields.Add("FSSJ");if(model.FSSJ!=null) command.Parameters.Add(new OracleParameter(":FSSJ", model.FSSJ));
                    if(model.FSYHID!=null) fields.Add("FSYHID");if(model.FSYHID!=null) command.Parameters.Add(new OracleParameter(":FSYHID", model.FSYHID));
                    if(model.SJHM!=null) fields.Add("SJHM");if(model.SJHM!=null) command.Parameters.Add(new OracleParameter(":SJHM", model.SJHM));
                    if(model.TXFY!=null) fields.Add("TXFY");if(model.TXFY!=null) command.Parameters.Add(new OracleParameter(":TXFY", model.TXFY));
                    if(model.CJSJ!=null) fields.Add("CJSJ");if(model.CJSJ!=null) command.Parameters.Add(new OracleParameter(":CJSJ", model.CJSJ));
                    if(model.BZ!=null) fields.Add("BZ");if(model.BZ!=null) command.Parameters.Add(new OracleParameter(":BZ", model.BZ));
                    command.CommandText = string.Format("insert into xtgl_ism_sendmx({0}) values(:{1})",string.Join(",",fields),string.Join(",:",fields));
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yjk_ddlbnmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_yjk_ddlbn
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取名称
        /// </summary>
        /// <param name="dm">查询方法所需参数dm</param>
        /// <returns>返回eHealth.Date.Entity.m_yjk_ddlbn实体</returns>
        public eHealth.Date.Entity.m_yjk_ddlbn GetMCByDM(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from yjk_ddlbn where dm=:dm";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":dm",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yjk_ddlbn>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yjk_ddlbn();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yjk_yyffmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_yjk_yyff
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取名称
        /// </summary>
        /// <param name="dm">查询方法所需参数dm</param>
        /// <returns>返回eHealth.Date.Entity.m_yjk_yyff实体</returns>
        public eHealth.Date.Entity.m_yjk_yyff GetMCByDM(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from yjk_yyff where dm=:dm";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":dm",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yjk_yyff>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yjk_yyff();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yjk_yyplmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_yjk_yypl
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取名称
        /// </summary>
        /// <param name="dm">查询方法所需参数dm</param>
        /// <returns>返回eHealth.Date.Entity.m_yjk_yypl实体</returns>
        public eHealth.Date.Entity.m_yjk_yypl GetMCByDM(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from yjk_yypl where dm=:dm";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":dm",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yjk_yypl>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yjk_yypl();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yl_mzhyyz化验医嘱
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 化验医嘱
    /// </summary>
    public partial class dl_yl_mzhyyz
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据诊疗活动id获取化验医嘱
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_mzhyyz实体</returns>
        public IList<eHealth.Date.Entity.m_yl_mzhyyz> GetHYByZLHDID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select z.YZID,z.CJSJ,z.HYXMMC,z.YZYHID,z.TXM,(select brxm from cw_khxx where brbh=z.brbh) xm from yl_mzhyyz z where z.zlhdid=:zlhdid order by z.hyxmid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_mzhyyz>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_mzhyyz>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据条形码找到化验单
        /// </summary>
        /// <param name="TXM">查询方法所需参数TXM</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_mzhyyz实体</returns>
        public eHealth.Date.Entity.m_yl_mzhyyz GetHYByTXM(String TXM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select  sampleno TXM,receivetime CJSJ,examinaim HYXMMC,patientname XM,notes Result from L_PATIENTINFO,XTGL_BMDM,L_SAMPLETYPE where bqid=bmid(+) and L_PATIENTINFO.sampletype=l_sampletype.sampletype  and resultstatus>=4 and doctadviseno=:TXM ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":TXM",TXM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_mzhyyz>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yl_mzhyyz();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yl_mzypyzmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_yl_mzypyz
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据zlhdid获取药品医嘱信息存根
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_mzypyz实体</returns>
        public IList<eHealth.Date.Entity.m_yl_mzypyz> GetByZLHDID_CG(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zlhdid,mc,dw,jx,jl,bzl,dj,sl,cs,ycyl,jldw,pl,ff,gysj from yl_mzypyzcg where zlhdid = :zlhdid and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_mzypyz>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_mzypyz>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据zlhdid获取药品医嘱信息
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_mzypyz实体</returns>
        public IList<eHealth.Date.Entity.m_yl_mzypyz> GetByZLHDID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zlhdid,mc,dw,jx,jl,bzl,dj,sl,cs,ycyl,jldw,pl,ff,gysj from yl_mzypyz where zlhdid = :zlhdid and (ztbz is null or ztbz<>'0')";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_mzypyz>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_mzypyz>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yl_sqdsj特检
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 特检
    /// </summary>
    public partial class dl_yl_sqdsj
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据诊疗活动id获取特检信息
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_sqdsj实体</returns>
        public IList<eHealth.Date.Entity.m_yl_sqdsj> GetTJByZLHDID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select l.tj,xq.mc,l.sclrsj,l.xm,l.yzyhid from yl_sqdsj l,yl_yzml xq where l.mlid=xq.yzmlid(+) and l.zlhdid=:zlhdid AND L.ZTBZ=1 order by l.yzsj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_sqdsj>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_sqdsj>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_fjxxmapp1
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// mapp1
    /// </summary>
    public partial class dl_yyfz_fjxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取所有房间信息，状态标志可用
        /// </summary>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_fjxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_fjxx> GetAll()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select FJID,FJWZ,FJMC,ZTBZ from yyfz_fjxx where ZTBZ=1";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_fjxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_fjxx>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_hjmb短信模版内容
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 短信模版内容
    /// </summary>
    public partial class dl_yyfz_hjmb
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取专病预约模版内容
        /// </summary>
        /// <param name="ZBYY">查询方法所需参数ZBYY</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_hjmb实体</returns>
        public eHealth.Date.Entity.m_yyfz_hjmb GetZBMBNR(String ZBYY)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mbnr from yyfz_hjmb ,yyfz_gxdy where yyfz_hjmb.fzzbh = '000' and yyfz_hjmb.dm = yyfz_gxdy.zd2 and yyfz_gxdy.lb = 'DXTX'and fjz='预约' and zd1=:ZBYY";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ZBYY",ZBYY));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_hjmb>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_hjmb();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取短信预约模版
        /// </summary>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_hjmb实体</returns>
        public eHealth.Date.Entity.m_yyfz_hjmb GetYYMBNR()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mbnr from yyfz_hjmb where fzzbh = '000' and dm = 'ZK_YY'";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_hjmb>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_hjmb();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_pbxx医生排班信息，包括医生排班信息和剩余号
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 医生排班信息，包括医生排班信息和剩余号
    /// </summary>
    public partial class dl_yyfz_pbxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据医生姓名检索给定日期内所有班和剩余号
        /// </summary>
        /// <param name="ysxm">查询方法所需参数ysxm</param>
        /// <param name="yspy">查询方法所需参数yspy</param>
        /// <param name="SBSJ">查询方法所需参数SBSJ</param>
        /// <param name="XBSJ">查询方法所需参数XBSJ</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_pbxx> GetAllDoctor(String YSXM,String YSPY,DateTime? SBSJ,DateTime? XBSJ)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb,r_ryk r where pb.rykid=r.id(+) and pb.ztbz='1'  and (trim(replace(pb.ysxm,' ','')) like :ysxm or r.py like :yspy) and  pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ysxm",YSXM));
                    command.Parameters.Add(new OracleParameter(":yspy",YSPY));
                    command.Parameters.Add(new OracleParameter(":SBSJ",SBSJ));
                    command.Parameters.Add(new OracleParameter(":XBSJ",XBSJ));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_pbxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取排版上下班时间(结果作为ZTBZ传出)
        /// </summary>
        /// <param name="ZTBZ">查询方法所需参数ZTBZ</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_pbxx GetSXBSJ(String ZTBZ)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zd2 ZTBZ from yyfz_gxdy where lb='YFSJ' and zd1=:ZTBZ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ZTBZ",ZTBZ));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_pbxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 判断排班是否已被取消
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_pbxx CheckPB(Int64 PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(*) pbid from yyfz_yspb where ztbz<>'9' and  pbid=:PBID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_pbxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 检索当天下午班和全天班和剩余号
        /// </summary>
        /// <param name="ZKID">查询方法所需参数ZKID</param>
        /// <param name="SBSJ">查询方法所需参数SBSJ</param>
        /// <param name="XBSJ">查询方法所需参数XBSJ</param>
        /// <param name="XBSJ2">查询方法所需参数XBSJ2</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_pbxx> GetTodayHalfPB(Int64? ZKID,DateTime? SBSJ,DateTime? XBSJ,DateTime? XBSJ2)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.ztbz='1' and pb.zkid=:ZKID and  pb.sbsj >=:SBSJ and pb.xbsj>=:XBSJ and pb.xbsj<=:XBSJ2 and pb.zllx in('02','04','07') order by pb.sbsj, pb.yqdm desc, pb.gzdm";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ZKID",ZKID));
                    command.Parameters.Add(new OracleParameter(":SBSJ",SBSJ));
                    command.Parameters.Add(new OracleParameter(":XBSJ",XBSJ));
                    command.Parameters.Add(new OracleParameter(":XBSJ2",XBSJ2));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_pbxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 检索给定日期内所有班和剩余号
        /// </summary>
        /// <param name="ZKID">查询方法所需参数ZKID</param>
        /// <param name="SBSJ">查询方法所需参数SBSJ</param>
        /// <param name="XBSJ">查询方法所需参数XBSJ</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_pbxx> GetAllPB(Int64? ZKID,DateTime? SBSJ,DateTime? XBSJ)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.ztbz='1'  and pb.zkid=:ZKID and  pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') order by pb.sbsj, pb.yqdm desc, pb.gzdm";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ZKID",ZKID));
                    command.Parameters.Add(new OracleParameter(":SBSJ",SBSJ));
                    command.Parameters.Add(new OracleParameter(":XBSJ",XBSJ));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_pbxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据排班ID获取排班信息
        /// </summary>
        /// <param name="pbid">查询方法所需参数pbid</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_pbxx GetPBByID(Int64 PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,pb.zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.pbid=:pbid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":pbid",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_pbxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据医生人员库id检索给定日期内所有班和剩余号
        /// </summary>
        /// <param name="RYKID">查询方法所需参数RYKID</param>
        /// <param name="SBSJ">查询方法所需参数SBSJ</param>
        /// <param name="XBSJ">查询方法所需参数XBSJ</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_pbxx> GetByRYKID(Int64? RYKID,DateTime? SBSJ,DateTime? XBSJ)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select pb.fjid, pb.ysyhid,pb.bmid ,pb.gzdm ,pb.pbid ,pb.rykid ,pb.sbsj ,pb.xbsj, pb.yqdm,pb.ysxm,pb.zkid,zkxh,(select bn.mc from xtgl_ddlbn bn where bn.lb='0051' and bn.dm=pb.zllx) zllx,(select count(*) from yyfz_yyxx xx where xx.yyxh<=pb.zkxh and xx.ztbz<>'9' and xx.pbid=pb.pbid and xx.yysj>sysdate) as syh,(select n.mc from yyfz_ddlbn n where n.lb='FZ15' and n.nblb='01' and n.dm=pb.zbyy ) zbyy ,(select sum(xm.xmje) from  cw_zllxghxm xm where pb.zllx=xm.zllx and pb.gzdm=xm.gzdm) xmje from yyfz_yspb pb where pb.rykid=:RYKID and pb.ztbz='1' and pb.xbsj >=:SBSJ and pb.sbsj<=:XBSJ  and pb.zllx in('02','04','07') ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":RYKID",RYKID));
                    command.Parameters.Add(new OracleParameter(":SBSJ",SBSJ));
                    command.Parameters.Add(new OracleParameter(":XBSJ",XBSJ));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_pbxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取诊疗类型
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_pbxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_pbxx GetZLLXByPBID(Int64 PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zllx from yyfz_yspb where pbid=:PBID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_pbxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_pbxx();
            }
        }

    }
    #endregion



    #region 注释：eHealth.Date.Entity.m_yyfz_yyls预约流水记录
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 预约流水记录
    /// </summary>
    public partial class dl_yyfz_yyls
    {
        /// <summary>
        /// 删除方法 创建时间：2015-07-13 16:50:33
        /// 根据预约id删除预约流水记录
        /// </summary>
        /// <param name="whereFZYYID">删除条件FZYYID</param>
        /// <returns></returns>
        public int DelYYLS(Int64 whereFZYYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":FZYYID", whereFZYYID));
                    command.CommandText = "delete from yyfz_yyls where fzyyid = :FZYYID";
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 添加方法，生成时间：2015-07-13 16:50:33
        /// 新增方法
        /// </summary>
        /// <param name="model">添加对象</param>
        /// <returns>大于0为添加成功</returns>
        public int Add(eHealth.Date.Entity.m_yyfz_yyls model)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    var fields = new List<string>();
                    command.Parameters.Clear();
                    if(model.BRBH!=null) fields.Add("BRBH");if(model.BRBH!=null) command.Parameters.Add(new OracleParameter(":BRBH", model.BRBH));
                    if(model.FZYYID!=null) fields.Add("FZYYID");if(model.FZYYID!=null) command.Parameters.Add(new OracleParameter(":FZYYID", model.FZYYID));
                    if(model.YYFSSJ!=null) fields.Add("YYFSSJ");if(model.YYFSSJ!=null) command.Parameters.Add(new OracleParameter(":YYFSSJ", model.YYFSSJ));
                    if(model.YYJZSJ!=null) fields.Add("YYJZSJ");if(model.YYJZSJ!=null) command.Parameters.Add(new OracleParameter(":YYJZSJ", model.YYJZSJ));
                    if(model.YSXM!=null) fields.Add("YSXM");if(model.YSXM!=null) command.Parameters.Add(new OracleParameter(":YSXM", model.YSXM));
                    if(model.ZTBZ!=null) fields.Add("ZTBZ");if(model.ZTBZ!=null) command.Parameters.Add(new OracleParameter(":ZTBZ", model.ZTBZ));
                    command.CommandText = string.Format("insert into yyfz_yyls({0}) values(:{1})",string.Join(",",fields),string.Join(",:",fields));
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_yyxx预约信息
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 预约信息
    /// </summary>
    public partial class dl_yyfz_yyxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取算时间点的预约信息
        /// </summary>
        /// <param name="pbid">查询方法所需参数pbid</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetYYXXForSJD(Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,yysj,yyxh,pbid from yyfz_yyxx where ztbz<>'9' and pbid=:pbid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":pbid",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取已预约的预约序号
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetYYXHByPBID(Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select yyxh from yyfz_yyxx where pbid=:PBID and ztbz<>'9' order by yyxh";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// (已登录)检查一个半天限约两个号源,数量作为YYXH返回
        /// </summary>
        /// <param name="BRBH">查询方法所需参数BRBH</param>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckYYCountByBRBH(String BRBH,Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(a.lxdh) yyxh from yyfz_yyxx a,yyfz_yspb b,cw_khxx c where c.brbh=:BRBH and b.pbid=:PBID and  ((c.lxdh is not null and a.lxdh like '%'||c.lxdh||'%')or(c.yddh is not null and  a.lxdh like '%'||c.yddh||'%')) and a.yysj>=b.sbsj  and a.yysj<=b.xbsj and (a.ztbz='1' or a.ztbz='2') and (a.zllx='02' or a.zllx='04' or a.zllx='07')";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRBH",BRBH));
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人姓名和手机号获取预约信息
        /// </summary>
        /// <param name="BRXM">查询方法所需参数BRXM</param>
        /// <param name="LXDH">查询方法所需参数LXDH</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetByXMDHTody(String BRXM,String LXDH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where trunc(sysdate)=trunc(yysj) and yyfs<>'F' and brxm=:BRXM and lxdh like :LXDH order by yysj desc,YSYHID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRXM",BRXM));
                    command.Parameters.Add(new OracleParameter(":LXDH",LXDH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取指定排班指定预约号是否已被预约，数量作为YYXH返回
        /// </summary>
        /// <param name="YYSJ">查询方法所需参数YYSJ</param>
        /// <param name="YYXH">查询方法所需参数YYXH</param>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckSJDExist(DateTime? YYSJ,Int32? YYXH,Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(yysj) yyxh from yyfz_yyxx where (yysj=:YYSJ or yyxh=:YYXH) and ztbz<>'9' and pbid=:PBID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":YYSJ",YYSJ));
                    command.Parameters.Add(new OracleParameter(":YYXH",YYXH));
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取预约信息通过预约id
        /// </summary>
        /// <param name="fzyyid">查询方法所需参数fzyyid</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx GetByFZYYID(Int64 FZYYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where fzyyid = :fzyyid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":fzyyid",FZYYID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 修改方法，生成时间：2015-07-13 16:50:33
        /// 修改签到标志和时间
        /// </summary>
        /// <param name="updateZTBZ">updateZTBZ</param>
        /// <param name="updateQDSJ">updateQDSJ</param>
        /// <param name="whereFZYYID">whereFZYYID</param>
        /// <returns></returns>
        public int UpdateQDBZ(String updateZTBZ,DateTime? updateQDSJ,Int64 whereFZYYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":updateZTBZ",updateZTBZ));
                    command.Parameters.Add(new OracleParameter(":updateQDSJ",updateQDSJ));
                    
                    command.Parameters.Add(new OracleParameter(":FZYYID",whereFZYYID));
                    command.CommandText = "update yyfz_yyxx set ZTBZ=:updateZTBZ,QDSJ=:updateQDSJ where FZYYID=:FZYYID";
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改方法，生成时间：2015-07-13 16:50:33
        /// 修改状态标志
        /// </summary>
        /// <param name="updateZTBZ">updateZTBZ</param>
        /// <param name="whereFZYYID">whereFZYYID</param>
        /// <returns></returns>
        public int UpdateZTBZ(String updateZTBZ,Int64 whereFZYYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":updateZTBZ",updateZTBZ));
                    
                    command.Parameters.Add(new OracleParameter(":FZYYID",whereFZYYID));
                    command.CommandText = "update yyfz_yyxx set ZTBZ=:updateZTBZ where FZYYID=:FZYYID";
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 修改方法，生成时间：2015-07-13 16:50:33
        /// 修改状态标志和操作者修改时间
        /// </summary>
        /// <param name="updateZTBZ">updateZTBZ</param>
        /// <param name="updateCZZID">updateCZZID</param>
        /// <param name="updateXGSJ">updateXGSJ</param>
        /// <param name="whereFZYYID">whereFZYYID</param>
        /// <returns></returns>
        public int UpdateZTBZCZZ(String updateZTBZ,Int64? updateCZZID,DateTime? updateXGSJ,Int64 whereFZYYID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":updateZTBZ",updateZTBZ));
                    command.Parameters.Add(new OracleParameter(":updateCZZID",updateCZZID));
                    command.Parameters.Add(new OracleParameter(":updateXGSJ",updateXGSJ));
                    
                    command.Parameters.Add(new OracleParameter(":FZYYID",whereFZYYID));
                    command.CommandText = "update yyfz_yyxx set ZTBZ=:updateZTBZ,CZZID=:updateCZZID,XGSJ=:updateXGSJ where FZYYID=:FZYYID";
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 添加方法，生成时间：2015-07-13 16:50:33
        /// 新增方法
        /// </summary>
        /// <param name="model">添加对象</param>
        /// <returns>大于0为添加成功</returns>
        public int Add(eHealth.Date.Entity.m_yyfz_yyxx model)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    var fields = new List<string>();
                    command.Parameters.Clear();
                    model.FZYYID= OracleHelper.GetNextValue("seq_yyfz_yyxx_id");
                    if(model.FZYYID!=null) fields.Add("FZYYID");if(model.FZYYID!=null) command.Parameters.Add(new OracleParameter(":FZYYID", model.FZYYID));
                    if(model.YYSJ!=null) fields.Add("YYSJ");if(model.YYSJ!=null) command.Parameters.Add(new OracleParameter(":YYSJ", model.YYSJ));
                    if(model.YYXH!=null) fields.Add("YYXH");if(model.YYXH!=null) command.Parameters.Add(new OracleParameter(":YYXH", model.YYXH));
                    if(model.PBID!=null) fields.Add("PBID");if(model.PBID!=null) command.Parameters.Add(new OracleParameter(":PBID", model.PBID));
                    if(model.BRBH!=null) fields.Add("BRBH");if(model.BRBH!=null) command.Parameters.Add(new OracleParameter(":BRBH", model.BRBH));
                    if(model.BRXM!=null) fields.Add("BRXM");if(model.BRXM!=null) command.Parameters.Add(new OracleParameter(":BRXM", model.BRXM));
                    if(model.BRXB!=null) fields.Add("BRXB");if(model.BRXB!=null) command.Parameters.Add(new OracleParameter(":BRXB", model.BRXB));
                    if(model.CSRQ!=null) fields.Add("CSRQ");if(model.CSRQ!=null) command.Parameters.Add(new OracleParameter(":CSRQ", model.CSRQ));
                    if(model.LXDH!=null) fields.Add("LXDH");if(model.LXDH!=null) command.Parameters.Add(new OracleParameter(":LXDH", model.LXDH));
                    if(model.SFZ!=null) fields.Add("SFZ");if(model.SFZ!=null) command.Parameters.Add(new OracleParameter(":SFZ", model.SFZ));
                    if(model.LXDZ!=null) fields.Add("LXDZ");if(model.LXDZ!=null) command.Parameters.Add(new OracleParameter(":LXDZ", model.LXDZ));
                    if(model.PDH!=null) fields.Add("PDH");if(model.PDH!=null) command.Parameters.Add(new OracleParameter(":PDH", model.PDH));
                    if(model.FZZBH!=null) fields.Add("FZZBH");if(model.FZZBH!=null) command.Parameters.Add(new OracleParameter(":FZZBH", model.FZZBH));
                    if(model.ZKID!=null) fields.Add("ZKID");if(model.ZKID!=null) command.Parameters.Add(new OracleParameter(":ZKID", model.ZKID));
                    if(model.YSYHID!=null) fields.Add("YSYHID");if(model.YSYHID!=null) command.Parameters.Add(new OracleParameter(":YSYHID", model.YSYHID));
                    if(model.YYFS!=null) fields.Add("YYFS");if(model.YYFS!=null) command.Parameters.Add(new OracleParameter(":YYFS", model.YYFS));
                    if(model.ZTBZ!=null) fields.Add("ZTBZ");if(model.ZTBZ!=null) command.Parameters.Add(new OracleParameter(":ZTBZ", model.ZTBZ));
                    if(model.ZLLX!=null) fields.Add("ZLLX");if(model.ZLLX!=null) command.Parameters.Add(new OracleParameter(":ZLLX", model.ZLLX));
                    if(model.QDSJ!=null) fields.Add("QDSJ");if(model.QDSJ!=null) command.Parameters.Add(new OracleParameter(":QDSJ", model.QDSJ));
                    if(model.JZSJ!=null) fields.Add("JZSJ");if(model.JZSJ!=null) command.Parameters.Add(new OracleParameter(":JZSJ", model.JZSJ));
                    if(model.GHID!=null) fields.Add("GHID");if(model.GHID!=null) command.Parameters.Add(new OracleParameter(":GHID", model.GHID));
                    if(model.BZ!=null) fields.Add("BZ");if(model.BZ!=null) command.Parameters.Add(new OracleParameter(":BZ", model.BZ));
                    if(model.DJRYID!=null) fields.Add("DJRYID");if(model.DJRYID!=null) command.Parameters.Add(new OracleParameter(":DJRYID", model.DJRYID));
                    if(model.CZZID!=null) fields.Add("CZZID");if(model.CZZID!=null) command.Parameters.Add(new OracleParameter(":CZZID", model.CZZID));
                    if(model.XGSJ!=null) fields.Add("XGSJ");if(model.XGSJ!=null) command.Parameters.Add(new OracleParameter(":XGSJ", model.XGSJ));
                    if(model.HJLX!=null) fields.Add("HJLX");if(model.HJLX!=null) command.Parameters.Add(new OracleParameter(":HJLX", model.HJLX));
                    if(model.YJNR!=null) fields.Add("YJNR");if(model.YJNR!=null) command.Parameters.Add(new OracleParameter(":YJNR", model.YJNR));
                    if(model.DJSJ!=null) fields.Add("DJSJ");if(model.DJSJ!=null) command.Parameters.Add(new OracleParameter(":DJSJ", model.DJSJ));
                    if(model.QDRYID!=null) fields.Add("QDRYID");if(model.QDRYID!=null) command.Parameters.Add(new OracleParameter(":QDRYID", model.QDRYID));
                    if(model.GZDM!=null) fields.Add("GZDM");if(model.GZDM!=null) command.Parameters.Add(new OracleParameter(":GZDM", model.GZDM));
                    if(model.QHPZDM!=null) fields.Add("QHPZDM");if(model.QHPZDM!=null) command.Parameters.Add(new OracleParameter(":QHPZDM", model.QHPZDM));
                    if(model.ZMTX!=null) fields.Add("ZMTX");if(model.ZMTX!=null) command.Parameters.Add(new OracleParameter(":ZMTX", model.ZMTX));
                    if(model.ZBYY!=null) fields.Add("ZBYY");if(model.ZBYY!=null) command.Parameters.Add(new OracleParameter(":ZBYY", model.ZBYY));
                    command.CommandText = string.Format("insert into yyfz_yyxx({0}) values(:{1})",string.Join(",",fields),string.Join(",:",fields));
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据排版id获取预约科室位置信息
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx GetYYWZByPBID(Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select a.yqdm,b.fjmc,b.fjwz from yyfz_yspb a,yyfz_fjxx b where a.fjid=b.fjid and a.pbid=:PBID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// (已登录)检查用户在此排班是否已经有预约,数量作为YYXH返回
        /// </summary>
        /// <param name="BRBH">查询方法所需参数BRBH</param>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckExist1(String BRBH,Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(brbh) yyxh from yyfz_yyxx where ztbz<>'9' and brbh=:BRBH and pbid=:PBID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRBH",BRBH));
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// (未登录)检查一个半天限约两个号源,数量作为YYXH返回
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <param name="LXDH">查询方法所需参数LXDH</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckYYCountBySJH2(Int64? PBID,String LXDH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(lxdh) YYXH from yyfz_yyxx,(select sbsj, xbsj from yyfz_yspb where pbid=:PBID) where lxdh like :LXDH and yysj>=sbsj and yysj<=xbsj and (ztbz='1' or ztbz='2') and (zllx='02' or zllx='04' or zllx='07')";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));
                    command.Parameters.Add(new OracleParameter(":LXDH",LXDH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// (未登录)检查用户在此排班是否已经有预约,数量作为YYXH返回
        /// </summary>
        /// <param name="BRXM">查询方法所需参数BRXM</param>
        /// <param name="LXDH">查询方法所需参数LXDH</param>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckExist2(String BRXM,String LXDH,Int64? PBID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(fzyyid) YYXH from yyfz_yyxx where ztbz<>'9' and brxm=:BRXM and lxdh like :LXDH and pbid=:PBID;";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRXM",BRXM));
                    command.Parameters.Add(new OracleParameter(":LXDH",LXDH));
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 检查该时间点是否已被其他人预约,数量作为YYXH返回
        /// </summary>
        /// <param name="PBID">查询方法所需参数PBID</param>
        /// <param name="YYSJ">查询方法所需参数YYSJ</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public eHealth.Date.Entity.m_yyfz_yyxx CheckYYSJ(Int64? PBID,DateTime? YYSJ)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select count(pbid) YYXH from yyfz_yyxx where pbid=:PBID and yysj=:YYSJ and ztbz<>'9'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":PBID",PBID));
                    command.Parameters.Add(new OracleParameter(":YYSJ",YYSJ));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_yyxx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人编号获取用户历史31天内的预约记录
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetByBRBH(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where djsj>sysdate-31 and yyfs<>'F' and brbh=:brbh order by ztbz,yysj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人编号获取用户当天的预约记录
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetByBRBHTody(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where trunc(sysdate)=trunc(yysj) and yyfs<>'F' and brbh=:brbh order by yysj desc,YSYHID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人姓名和手机号获取病人历史预约
        /// </summary>
        /// <param name="BRXM">查询方法所需参数BRXM</param>
        /// <param name="LXDH">查询方法所需参数LXDH</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_yyxx实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_yyxx> GetByXMDH(String BRXM,String LXDH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select fzyyid,brbh,brxm,brxb,csrq,lxdh,sfz,lxdz,yyxh,pdh,yysj,fzzbh,zkid,ysyhid,yyfs,ztbz,zllx,qdsj,jzsj,ghid,bz,djryid,czzid,xgsj,pbid,hjlx,yjnr,djsj,qdryid,gzdm,qhpzdm,zmtx,zbyy from yyfz_yyxx where djsj>sysdate-31 and yyfs<>'F' and brxm=:BRXM and lxdh like :LXDH order by ztbz,yysj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRXM",BRXM));
                    command.Parameters.Add(new OracleParameter(":LXDH",LXDH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_yyxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_yyxx>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_zkfj_wh部门信息，限制查询层次，只对外显示cxbz='1'的部门
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 部门信息，限制查询层次，只对外显示cxbz='1'的部门
    /// </summary>
    public partial class dl_yyfz_zkfj_wh
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取信息
        /// </summary>
        /// <param name="BMID">查询方法所需参数BMID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetByDM(Int32? BMID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where DM=:BMID and cxbz='1' and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BMID",BMID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据名称获取代码
        /// </summary>
        /// <param name="MC">查询方法所需参数MC</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public eHealth.Date.Entity.m_yyfz_zkfj_wh GetDMByMC(String MC)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select  dm from yyfz_zkfj_wh where cxbz='1' and ztbz='1' and mc=:MC";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":MC",MC));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_zkfj_wh();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据名称获取信息
        /// </summary>
        /// <param name="MC">查询方法所需参数MC</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetByMC(String MC)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where MC=:MC  and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":MC",MC));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据父代码获取信息
        /// </summary>
        /// <param name="FDM">查询方法所需参数FDM</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetByFDM(String FDM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where DM=:FDM and cxbz='1'  and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":FDM",FDM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取科室名称根据部门id
        /// </summary>
        /// <param name="BMID">查询方法所需参数BMID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public eHealth.Date.Entity.m_yyfz_zkfj_wh GetMCByID(Int32? BMID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc,FDM,cxbz from yyfz_zkfj_wh where bmid=:BMID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BMID",BMID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_zkfj_wh();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据父代码获取信息
        /// </summary>
        /// <param name="FDM">查询方法所需参数FDM</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetAllByFDM(String FDM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM=:FDM  and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":FDM",FDM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据获取一级分类信息
        /// </summary>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetFirstFDM()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM='1' and cxbz='1' and ztbz='1'";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据获取所有二级级部门信息
        /// </summary>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetSecendBM()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,fdm,mc,bmid from yyfz_zkfj_wh where FDM>2 and cxbz='1' and ztbz='1'";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取所有的部门id
        /// </summary>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkfj_wh实体</returns>
        public IList<eHealth.Date.Entity.m_yyfz_zkfj_wh> GetAllBMID()
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select bmid from yyfz_zkfj_wh where bmid is not null";

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkfj_wh>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yyfz_zkfj_wh>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_br_jctx病人检查报告单
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 病人检查报告单
    /// </summary>
    public partial class dl_br_jctx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 通过病人编号返回所有的数据数据
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_br_jctx实体</returns>
        public IList<eHealth.Date.Entity.m_br_jctx> GetBGList(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select distinct t1.bgbs, t1.brbh,t1.brxm,t1.bgsj,nvl(t2.mc,'检验') as jclx,t1.jclx as jclx1 from br_jctx t1,yyfz_jclx t2 where t1.jclx=t2.dm(+) and t1.brbh=:brbh and t1.ztbz='0' and t1.bgsj>sysdate-365";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_br_jctx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_br_jctx>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_cw_khxx客户信息
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 客户信息
    /// </summary>
    public partial class dl_cw_khxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据手机号获取客户信息
        /// </summary>
        /// <param name="LXDH">查询方法所需参数LXDH</param>
        /// <param name="YDDH">查询方法所需参数YDDH</param>
        /// <returns>返回eHealth.Date.Entity.m_cw_khxx实体</returns>
        public IList<eHealth.Date.Entity.m_cw_khxx> GetKHXXBySJH(String LXDH,String YDDH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select brbh,brxm,brxb,sfzh,csrq,lxdz,lxdh,yddh,szsj,xgsj from cw_khxx where ztbz='1' and (lxdh=:LXDH or yddh=:YDDH)";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":LXDH",LXDH));
                    command.Parameters.Add(new OracleParameter(":YDDH",YDDH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_cw_khxx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_cw_khxx>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人编号获取病人卡号信息
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_cw_khxx实体</returns>
        public eHealth.Date.Entity.m_cw_khxx GetKHXXByBRBH(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select brbh,dwdm,knsj,brxm,brxb,gjdm,hyzk,zydm,jgdm,mzdm,sfzh,csrq,py,wb,xzzdm,lxdz,lxdh,yddh,szsj,ztbz,czzid,xgsj from cw_khxx where brbh = :brbh";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_cw_khxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_cw_khxx();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_cw_ycxfmx用户消费记录
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 用户消费记录
    /// </summary>
    public partial class dl_cw_ycxfmx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取用户余额
        /// </summary>
        /// <param name="BRBH">查询方法所需参数BRBH</param>
        /// <returns>返回eHealth.Date.Entity.m_cw_ycxfmx实体</returns>
        public eHealth.Date.Entity.m_cw_ycxfmx GetYHYE(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select ckje-xfje-mzdjje XFJE  from cw_ycje where brbh=:BRBH and ycdm='02'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BRBH",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_cw_ycxfmx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_cw_ycxfmx();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据用户健康卡号获取用户消费记录
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_cw_ycxfmx实体</returns>
        public IList<eHealth.Date.Entity.m_cw_ycxfmx> GetXFJL(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select xfje,sflx,sfsj from cw_ycxfmx where sfsj>sysdate-90 and brbh=:brbh order by sfsj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_cw_ycxfmx>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_cw_ycxfmx>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_e_zybldm病例代码表，用于根据代码获取病例内容表
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 病例代码表，用于根据代码获取病例内容表
    /// </summary>
    public partial class dl_e_zybldm
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取病例内容表
        /// </summary>
        /// <param name="DM">查询方法所需参数DM</param>
        /// <returns>返回eHealth.Date.Entity.m_e_zybldm实体</returns>
        public eHealth.Date.Entity.m_e_zybldm GetNrb(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select dm,nrb from e_zybldm where dm=:DM";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":DM",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_e_zybldm>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_e_zybldm();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_oa_glfjb关联附件表
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 关联附件表
    /// </summary>
    public partial class dl_oa_glfjb
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据附件id获取附件
        /// </summary>
        /// <param name="FJID">查询方法所需参数FJID</param>
        /// <returns>返回eHealth.Date.Entity.m_oa_glfjb实体</returns>
        public eHealth.Date.Entity.m_oa_glfjb GetGLByFJID(String FJID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select FJNR from OA_GLFJB where FJID=:FJID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":FJID",FJID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_oa_glfjb>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_oa_glfjb();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 通过关联id获取附件信息
        /// </summary>
        /// <param name="GLID">查询方法所需参数GLID</param>
        /// <returns>返回eHealth.Date.Entity.m_oa_glfjb实体</returns>
        public IList<eHealth.Date.Entity.m_oa_glfjb> GetGLByGLID(Int64 GLID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select FJID,WJGS from OA_GLFJB where glid=:GLID and gllb ='6' order by SXH ";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":GLID",GLID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_oa_glfjb>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_oa_glfjb>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_oa_xw新闻
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 新闻
    /// </summary>
    public partial class dl_oa_xw
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据新闻id获取新闻
        /// </summary>
        /// <param name="id">查询方法所需参数id</param>
        /// <returns>返回eHealth.Date.Entity.m_oa_xw实体</returns>
        public eHealth.Date.Entity.m_oa_xw GetOAXWByID(Int64 ID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select BT,ZZ,TJSJ,WZNR from oa_xw where ID=:id";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":id",ID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_oa_xw>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_oa_xw();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_xtgl_bmdm部门代码
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 部门代码
    /// </summary>
    public partial class dl_xtgl_bmdm
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据部门id获取部门名称
        /// </summary>
        /// <param name="BMID">查询方法所需参数BMID</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_bmdm实体</returns>
        public eHealth.Date.Entity.m_xtgl_bmdm GetBMMCByBMID(Int64 BMID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select bmmc from xtgl_bmdm where bmid=:BMID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BMID",BMID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_bmdm>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_bmdm();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据部门dm获取sjbm=1的部门名称
        /// </summary>
        /// <param name="BMDM">查询方法所需参数BMDM</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_bmdm实体</returns>
        public eHealth.Date.Entity.m_xtgl_bmdm GetBMMCByBMDM(String BMDM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select bmmc from xtgl_bmdm where sjbm=1 and bmdm=:BMDM";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":BMDM",BMDM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_bmdm>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_bmdm();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_xtgl_ddlbn部门代码与部门名称对应表
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 部门代码与部门名称对应表
    /// </summary>
    public partial class dl_xtgl_ddlbn
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取专病预约名称
        /// </summary>
        /// <param name="DM">查询方法所需参数DM</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_ddlbn实体</returns>
        public eHealth.Date.Entity.m_xtgl_ddlbn GetZBYYByDM(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from yyfz_ddlbn where lb='FZ15' and dm=:DM";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":DM",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_ddlbn>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_ddlbn();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 获取指定类别和代码的名称
        /// </summary>
        /// <param name="lb">查询方法所需参数lb</param>
        /// <param name="dm">查询方法所需参数dm</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_ddlbn实体</returns>
        public eHealth.Date.Entity.m_xtgl_ddlbn GetByLBDM(String LB,String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from xtgl_ddlbn where lb = :lb and dm = :dm and ztbz='1'";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":lb",LB));
                    command.Parameters.Add(new OracleParameter(":dm",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_ddlbn>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_ddlbn();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据代码获取lb='0005'的院区名称
        /// </summary>
        /// <param name="DM">查询方法所需参数DM</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_ddlbn实体</returns>
        public eHealth.Date.Entity.m_xtgl_ddlbn GetYQMC(String DM)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select mc from xtgl_ddlbn where lb='0014' and dm=:DM";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":DM",DM));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_ddlbn>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_ddlbn();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_xtgl_yhxx用户信息
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 用户信息
    /// </summary>
    public partial class dl_xtgl_yhxx
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据用户id获取用户姓名
        /// </summary>
        /// <param name="YHID">查询方法所需参数YHID</param>
        /// <returns>返回eHealth.Date.Entity.m_xtgl_yhxx实体</returns>
        public eHealth.Date.Entity.m_xtgl_yhxx GetYHXMByYHID(Int64 YHID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select yhxm from xtgl_yhxx where yhid =:YHID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":YHID",YHID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_xtgl_yhxx>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_xtgl_yhxx();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yl_zljl诊疗记录
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 诊疗记录
    /// </summary>
    public partial class dl_yl_zljl
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据病人编号获取诊疗活动
        /// </summary>
        /// <param name="brbh">查询方法所需参数brbh</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_zljl实体</returns>
        public IList<eHealth.Date.Entity.m_yl_zljl> GetZLHDByBRBH(String BRBH)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zlhdid,sczlhdid,zllx,jzzkid,jzysyhid,kssj from yl_zlhd where brbh = :brbh and zlzt = '1' order by kssj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":brbh",BRBH));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_zljl>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_zljl>();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据诊疗活动id获取诊疗活动
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_zljl实体</returns>
        public eHealth.Date.Entity.m_yl_zljl GetZLHDByID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select zlhdid,sczlhdid,zllx,jzzkid,jzysyhid,kssj from yl_zlhd where zlhdid = :zlhdid and zlzt = '1' order by kssj desc";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_zljl>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yl_zljl();
            }
        }

        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据zlhdid获取jlid
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_zljl实体</returns>
        public eHealth.Date.Entity.m_yl_zljl GetJLID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select jlid from yl_zljl a,xtgl_zjsbg b,yl_zlhd c where a.gsdm = b.gsdm and a.zlhdid = c.zlhdid and (b.zkid = c.jzzkid or b.zkid=0) and a.zlhdid = :zlhdid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_zljl>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yl_zljl();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yl_zlzd诊疗诊断
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 诊疗诊断
    /// </summary>
    public partial class dl_yl_zlzd
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 根据诊疗活动id获取诊疗诊断
        /// </summary>
        /// <param name="zlhdid">查询方法所需参数zlhdid</param>
        /// <returns>返回eHealth.Date.Entity.m_yl_zlzd实体</returns>
        public IList<eHealth.Date.Entity.m_yl_zlzd> GetZKZDByZLHDID(Int64 ZLHDID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select qz,lczd,hz  from yl_zlzd where zlhdid=:zlhdid order by zdxh";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":zlhdid",ZLHDID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yl_zlzd>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_yl_zlzd>();
            }
        }

    }
    #endregion


    #region 注释：eHealth.Date.Entity.m_yyfz_zkyysz获取是否自动签到
    /// <summary>
    /// 模板生成
    /// CreateBy 童岭 2015-07-13 16:50:33
    /// 获取是否自动签到
    /// </summary>
    public partial class dl_yyfz_zkyysz
    {
        /// <summary>
        /// 查询方法，生成时间：2015-07-13 16:50:33
        /// 
        /// </summary>
        /// <param name="ZKID">查询方法所需参数ZKID</param>
        /// <returns>返回eHealth.Date.Entity.m_yyfz_zkyysz实体</returns>
        public eHealth.Date.Entity.m_yyfz_zkyysz GetSFQD(Int64 ZKID)
        {
            try
            {
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = "select sfqd from yyfz_zkyysz where zkid=:ZKID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter(":ZKID",ZKID));

                    var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_yyfz_zkyysz>(command);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new eHealth.Date.Entity.m_yyfz_zkyysz();
            }
        }

    }
    #endregion


}







