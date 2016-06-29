//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基于TinyMapper 的adapter实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 13:55:09
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.CrossCutting.Adapter;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace eHPS.CrossCutting.NetFramework.Adapter
{
    public class TinyMapperTypeAdapter : ITypeAdapter
    {
        public TTarget Adapter<TTarget>(object source) where TTarget : class, new()
        {
            return TinyMapper.Map<TTarget>(source);      
        }

        public TTarget Adapter<TSource, TTarget>(TSource souce)
            where TSource : class
            where TTarget : class, new()
        {
            return TinyMapper.Map<TSource, TTarget>(souce);
        }
    }
}
