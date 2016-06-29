//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Domain Entity 与Data Transfer Object 适配器（互相转换）工厂方法
// 使用AutoMapper类库实现
// 创建AutomapperTypeAdapter
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using AutoMapper;
using System;
using System.Linq;
using eHPS.CrossCutting.Adapter;


namespace eHPS.CrossCutting.NetFramework.Adapter
{
    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {

        /// <summary>
        /// 初始化AutoMapper配置（profile，configuration）
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            //获得所有自定义的继承自profile的子类（自定义映射规则）
            var profiles = AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes()).Where(t => t.BaseType == typeof(Profile));

            Mapper.Initialize(configuration=>
                {
                    foreach (var item in profiles)
                    {
                        if(item.FullName!="AutoMapper.SeflProfiler`2"&&item.Name!= "NamedProfile")
                        {
                            configuration.AddProfile(Activator.CreateInstance(item) as Profile);
                        }
                        
                    }

                });
        }


        public ITypeAdapter CreatAdapter()
        {
            return new AutomapperTypeAdapter();
        }




    }
}
