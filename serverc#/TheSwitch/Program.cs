using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using TelldusWrapper;
using TheSwitch.Server;

namespace TheSwitch
{
    class Program
    {
        public static int DeviceChangeEventCallback(int deviceId, int changeEvent, int changeType, int callbackId, object obj)
        {
            return 0;
        }

        public static int EventCallback(int deviceId, int method, string data, int callbackId, object obj)
        {
            return 0;
        }

        private static int unknownId = 0;

        public static int ListeningCallback(string data, int controllerId, int callbackId, object obj)
        {
            //IDataParameters param = null;
            if (data.Contains(":sensor"))
            {
                Common.SensorValues.Add(new SensorParameters(data));
            }
            else
            {
                var param = new SwitchParameters(data);
                if (!Common.UnknownDevices.Any(d => d.Params.House == param.House && d.Params.Unit == param.Unit && d.Params.Protocol == param.Protocol))
                {
                    Common.UnknownDevices.Add(new UnknownDevice()
                    {
                        Id = unknownId++,
                        Params = param,
                        When = DateTime.Now,
                        Data = data,
                        ControllerId = controllerId,
                        CallbackId = callbackId
                    });
                }
            }
            return 0;
        }

        static void Main(string[] args)
        {
            // Base address
            Uri baseServiceAddress = new Uri("http://localhost:8091/");

            using (var host = new ServiceHost(typeof(SwitchService), baseServiceAddress))
            {

                var wrp = new TelldusNETWrapper();
                wrp.tdRegisterDeviceChangeEvent(DeviceChangeEventCallback, null);
                wrp.tdRegisterDeviceEvent(EventCallback, null);
                wrp.tdRegisterRawDeviceEvent(ListeningCallback, null);

                // Enable MetaData publishing.
                var serviceMetaDataBehaviour = new ServiceMetadataBehavior();
                serviceMetaDataBehaviour.HttpGetEnabled = true;
                serviceMetaDataBehaviour.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(serviceMetaDataBehaviour);

                // Open the ServiceHost to start listening for messages. No endpoint are explicitly defined, runtime creates default endpoint.
                host.Open();

                HttpServer ws = new HttpServer(SendResponse, "http://192.168.1.91:8080/");
                ws.Run();

                Console.WriteLine("The service is ready at {0} and host at {1}", baseServiceAddress, DateTime.Now.ToString());
                Console.WriteLine("The service and client is running in the same process.");

                //SwitchService selfHostService = new SwitchService();
                //var newdev = selfHostService.AddDevice();
                //var val = selfHostService.Learn(newdev);
                ////selfHostService.SetProtocol(newdev,)
                //selfHostService.SetName(newdev,"Testar");




                Console.Write("Enter your name. : ");
                //Console.WriteLine(selfHostService.WelComeMessage(Console.ReadLine()));
                Console.WriteLine("Host is running... Press <Enter> key to stop the service.");
                Console.ReadLine();

                //Close the service.
                host.Close();
            }

        }

        private static ServerCache SrvCache = new ServerCache(Environment.CurrentDirectory.TrimEnd('\\') + "\\ServerFiles\\");
        private static RestServerHandler RestServer = new RestServerHandler(new SwitchService());


        public static bool SendResponse(HttpListenerContext ctx)
        {
            if (ctx.Request.RawUrl.StartsWith("/rest/"))
            {
                RestServer.ServeUrl(ctx);
            }
            else
            {
                SrvCache.ServeUrl(ctx.Request.Url, ctx.Response);
            }
            return true;
            //return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }
    }
}
