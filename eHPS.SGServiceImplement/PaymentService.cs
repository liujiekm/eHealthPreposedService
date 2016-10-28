//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 上海曙光医院支付服务
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/10/28 13:31:10
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHPS.Contract.Model;

namespace eHPS.SGServiceImplement
{
    public class PaymentService : IPayment
    {
        public List<PatientConsumption> AwareOrderBooked(List<string> patientIds)
        {
            throw new NotImplementedException();
        }

        public TradingAccount GetPatientAvaliableAmount(string patientId)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> Pay(string tradingId, string activityId, decimal amount, decimal actualAmount)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> PayRegistration(string tradingId, string hospitalId, string appointId, string amount, string actualAmount)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<string> Recharge(string patientId, string tradingId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
