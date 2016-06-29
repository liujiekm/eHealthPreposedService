//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 运用TinyMapper 转换 实体与DTO的转化规则 配置类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 14:15:20
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;

namespace eHPS.CrossCutting.NetFramework.Adapter
{
    public abstract class TinyMapperBind
    {
        public abstract void Bind();
    }
}
