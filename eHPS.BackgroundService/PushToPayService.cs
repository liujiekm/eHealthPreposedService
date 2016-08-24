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
using System.Net.Http.Formatting;

namespace eHPS.BackgroundService
{
    partial class PushToPayService : ServiceBase
    {

        private IPayment paymentService;

        private static readonly string  baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        private static readonly string  interval = ConfigurationManager.AppSettings["Interval"];

        private static readonly string appid = ConfigurationManager.AppSettings["AppID"];
        private static readonly string appIdSecret = ConfigurationManager.AppSettings["AppIDSecret"];


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
            System.Timers.Timer timer = new System.Timers.Timer {Interval = Int32.Parse(interval)};
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

            var patientIdList = new List<String>();
            try
            {
                if(patientIds.IsCompleted)
                {
                    patientIdList = patientIds.Result;
                }

                //patientIdList = new List<String> { "0000003001777361", "0000003001775739", "0000003001779855" };
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().Error(String.Format(

                  "获取用户绑定卡号信息服务{0} 抛出异常 {1}\r\n堆栈信息：{2}", "RequestPatientIds().Result",
                  ex.Message,
                  ex.StackTrace), ex);
                throw;
            }
            


            LoggerFactory.CreateLog().Info("获得用户平台绑定卡号信息", patientIdList);

            List<PatientConsumption> patientConsumptions;
            try
            {
                patientConsumptions = paymentService.AwareOrderBooked(patientIdList);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().Error(String.Format(

                  "获取患者待支付项目服务 {0} 抛出异常 {1}\r\n堆栈信息：{2}", "paymentService.AwareOrderBooked",
                  ex.Message,
                  ex.StackTrace), ex);
                throw;
            }
            

            var resultCode = 0;
            try
            {

                //按患者来推送收费项目
                foreach (var patientConsumption in patientConsumptions)
                {
                    resultCode = MessageQueueHelper.PushMessage<PatientConsumption>(QueueDescriptor.AwareOrderBooked.Item1, patientConsumption);
                    LoggerFactory.CreateLog().Info("推送患者: "+patientConsumption.PatientName+"待支付项目" + (resultCode == 0 ? "失败" : "成功"));
                }

                

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().Error(String.Format(

                  "推送患者待支付项目到RabbitMQ队列 {0} 抛出异常 {1}\r\n堆栈信息：{2}", "MessageQueueHelper.PushMessage<List<PatientConsumption>>",
                  ex.Message,
                  ex.StackTrace), ex);

                throw;
            }
            


        }


        private async System.Threading.Tasks.Task<List<String>> RequestPatientIds()
        {
            var requestUri = new Uri(baseUrl);
            var patientIds =new  PlatformServiceResponse<List<String>>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = requestUri;
                client.DefaultRequestHeaders.Accept.Clear();
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var response = await client.GetAsync("api/User/GetHospitalBindingCard/" + appid);
                if (response.IsSuccessStatusCode)
                {
                    patientIds = await response.Content.ReadAsAsync<PlatformServiceResponse<List<String>>>(new List<MediaTypeFormatter> { new JilFormatter() });
                }
            }
            return patientIds.Data;
        }


        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
