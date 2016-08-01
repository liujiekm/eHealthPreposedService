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

using eHPS.Contract.Model;
using eHPS.Common;
using eHPS.MessageQueueContract;
using eHPS.CrossCutting.Logging;
using eHPS.CrossCutting.NetFramework.Logging;
using Microsoft.Practices.Unity.Configuration;

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
            container.LoadConfiguration();
            //container.RegisterType<IPayment, PaymentService>(
            //    new Interceptor<InterfaceInterceptor>(),
            //    new InterceptionBehavior<LoggingInterceptionBehavior>()
            //);
            //container.AddNewExtension<Interception>();
            paymentService = container.Resolve<IPayment>();

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());

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



            LoggerFactory.CreateLog().Info("开始推送用户待收费项目");
            var patientIds =RequestPatientIds();
            
            var patientConsumption = paymentService.AwareOrderBooked(patientIds.Result);

            var resultCode = 0;
            try
            {
                resultCode = MessageQueueHelper.PushMessage<List<PatientConsumption>>(QueueDescriptor.AwareOrderBooked.Item1, patientConsumption);

                LoggerFactory.CreateLog().Info("推送"+(resultCode==0?"失败":"成功"),patientIds.Result);

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().Error(String.Format(

                  "方法 {0} 抛出异常 {1}\r\n堆栈信息：{2}", "MessageQueueHelper.PushMessage<List<PatientConsumption>>",
                  ex.Message,
                  ex.StackTrace), ex);

                throw;
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
