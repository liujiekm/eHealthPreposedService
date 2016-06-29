//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Domain Entity 与Data Transfer Object 适配器（互相转换）静态工厂方法
// 使用AutoMapper类库实现
// 提供统一的静态类来构造实现了ITypeAdapterFactory的实现类
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
    public class TypeAdapterFactory
    {
        private static  ITypeAdapterFactory _typeAdapterFactory;


        /// <summary>
        /// 设置当前要使用的实现类（依赖注入）
        /// </summary>
        /// <param name="current"></param>
        public static void SetCurrent(ITypeAdapterFactory current)
        {
            _typeAdapterFactory = current;
        }


        /// <summary>
        /// 创建具体的TypeAdapter实现类
        /// </summary>
        /// <returns></returns>
        public static  ITypeAdapter CreateAdapter()
        {
            return _typeAdapterFactory.CreatAdapter();
        }


    }
}
