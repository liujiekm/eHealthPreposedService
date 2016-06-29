//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Domain Entity 与Data Transfer Object 适配器（互相转换）方法接口契约
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.CrossCutting.Adapter
{
    public interface ITypeAdapter
    {
        /// <summary>
        /// 转换TSource实例为TTarget实例
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="souce"></param>
        /// <returns></returns>
        TTarget Adapter<TSource, TTarget>(TSource souce) where TTarget : class, new() where TSource : class;

        /// <summary>
        /// 转换无指定类型实例为TTarget实例
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TTarget Adapter<TTarget>(object source) where TTarget : class, new();
    }
}
