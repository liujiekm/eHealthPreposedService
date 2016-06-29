//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 测试person 转换配置类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 14:21:57
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper;

using eHPS.CrossCutting.NetFramework.Adapter;

namespace eHPS.API.Test
{
    public class PersonMapperBind : TinyMapperBind
    {
        public  override void Bind()
        {
            TinyMapper.Bind<Person, PersonDto>(config =>
            {
                config.Bind(source =>source.LastName, target => target.Name);

            });
        }
    }
}
