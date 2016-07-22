//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 待支付项目推送服务类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Contract;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.WYServiceImplement;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;

using CacheMQService;
using Thrift.Transport;
using Thrift.Protocol;
using Jil;
using eHPS.Contract.Model;

namespace eHPS.BackgroundService
{
    partial class PushToPayService : ServiceBase
    {

        private IPayment paymentService;

        private static readonly string  baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private static readonly string  interval = ConfigurationManager.AppSettings["interval"];

        private static readonly string appid = ConfigurationManager.AppSettings["appId"];
        private static readonly string appIdSecret = ConfigurationManager.AppSettings["appIdSecret"];


        public PushToPayService()
        {
            InitializeComponent();

            //依赖注入各医院的具体推送实现
            var container = new UnityContainer();
            container.RegisterType<IPayment, PaymentService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>()
            );
            container.AddNewExtension<Interception>();
            paymentService = container.Resolve<IPayment>();


        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = Int32.Parse(interval); 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnPush);
            timer.Start();
        }


        /// <summary>
        /// 具体推送服务逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnPush(object sender, System.Timers.ElapsedEventArgs args)
        {
            var patientIds = RequestPatientIds();
            var treatments = paymentService.AwareOrderBooked(patientIds.Result);
            //调用消息队列服务，把treatments推送到消息队列
            using (TTransport transport = new TSocket("192.168.1.190", 9090))
            {
                TProtocol protocol = new TCompactProtocol(transport);
                CacheMQService.Calculator.Client client = new CacheMQService.Calculator.Client(protocol);
                transport.Open();
                var result = client.serverSendMsg("", JSON.Serialize<List<Treatment>>(treatments));//0失败；1成功
                if (result == 0)
                {
                }
            }

        }


        private async System.Threading.Tasks.Task<List<String>> RequestPatientIds()
        {
            var requestUri = new Uri(baseUrl);
            var patientIds = new List<String>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = requestUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/User/GetHospitalBindingCard/"+appid);
                if (response.IsSuccessStatusCode)
                {
                    patientIds = await response.Content.ReadAsAsync<List<String>>();
                }
            }
            return patientIds;
        }


        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
