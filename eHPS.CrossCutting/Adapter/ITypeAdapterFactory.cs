//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Domain Entity 与Data Transfer Object 适配器 工厂接口
// 创建具体适配器类
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
    public interface ITypeAdapterFactory
    {
        ITypeAdapter CreatAdapter();
    }
}
