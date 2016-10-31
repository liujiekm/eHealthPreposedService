//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 生成基于TinyMapper 转换DTO 的工厂类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 14:00:56
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.CrossCutting.Adapter;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.CrossCutting.NetFramework.Adapter
{
    public class TinyMapperTypeAdapterFactory : ITypeAdapterFactory
    {

        /// <summary>
        /// 配置各实体类与DTO的转化规则
        /// </summary>
        public TinyMapperTypeAdapterFactory()
        {
            //获得所有自定义的继承自TinyMapperBind的子类（自定义映射规则）
            var binds = AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes()).Where(t => t.BaseType == typeof(TinyMapperBind));

            foreach (var bind in binds)
            {
                //bind.GetConstructor(Type.EmptyTypes).Invoke(null)
                bind.GetMethod("Bind").Invoke(Activator.CreateInstance(bind), null);
            }

        }


        public ITypeAdapter CreatAdapter()
        {
            return new TinyMapperTypeAdapter();
        }
    }
}
