//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Domain Entity 与Data Transfer Object 适配器（互相转换）具体实现类
// 使用AutoMapper类库实现
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================


using AutoMapper;
using System;
using eHPS.CrossCutting.Adapter;

namespace eHPS.CrossCutting.NetFramework.Adapter
{
    public class AutomapperTypeAdapter : ITypeAdapter
    {
        public TTarget Adapter<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }

        public TTarget Adapter<TSource, TTarget>(TSource souce)
            where TSource : class
            where TTarget : class, new()
        {
            return Mapper.Map<TSource, TTarget>(souce);
        }
    }
}
