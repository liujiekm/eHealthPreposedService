//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 类库说明
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/29 16:39:25
// 版本号：  V1.0.0.0
//===================================================================================




using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.API.Test
{
    public class PersonProfile:Profile
    {
        protected override void Configure()
        {
            
            CreateMap<Person, PersonDto>().ForMember(target => target.Name, opt => opt.MapFrom(src => src.FirstName + src.LastName));
        }
    }
}
