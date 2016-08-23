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
// 创建时间：2016/8/23 11:07:19
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract;
using eHPS.Contract.Model;

namespace eHPS.SomeImplement
{
    public class SomePaymentService : IPayment
    {
        public List<PatientConsumption> AwareOrderBooked(List<string> patientIds)
        {
            throw new NotImplementedException();
        }

        public TradingAccount GetPatientAvaliableAmount(string patientId)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> Pay(string tradingId, string activityId, string amount, string actualAmount)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> PayRegistration(string tradingId, string hospitalId, string appointId, string amount, string actualAmount)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> Recharge(string patientId, string tradingId, string amount)
        {
            throw new NotImplementedException();
        }
    }
}
