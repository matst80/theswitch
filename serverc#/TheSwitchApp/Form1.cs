using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TelldusWrapper;
using TheSwitch;
using TheSwitch.Server;

namespace TheSwitchApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private HttpServer webServer;

        public static int DeviceChangeEventCallback(int deviceId, int changeEvent, int changeType, int callbackId, object obj)
        {
            return 0;
        }

        public static int EventCallback(int deviceId, int method, string data, int callbackId, object obj)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            dev.AddDeviceLog(new TheSwitch.Core.DeviceEvent()
            {
                Data = data,
                Method = method
            });
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


        private void Form1_Load(object sender, EventArgs e)
        {
            webServer = new HttpServer(SendResponse, "http://192.168.1.91:8080/");
            webServer.Run();
            //devices = TelldusDevice.GetDevices();
            var wrp = new TelldusNETWrapper();
            wrp.tdRegisterDeviceChangeEvent(DeviceChangeEventCallback, null);
            wrp.tdRegisterDeviceEvent(EventCallback, null);
            wrp.tdRegisterRawDeviceEvent(ListeningCallback, null);

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.SaveAll();
        }
    }
}
