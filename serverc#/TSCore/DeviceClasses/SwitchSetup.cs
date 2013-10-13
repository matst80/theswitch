using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelldusWrapper;
using TheSwitch.Interfaces;

namespace TheSwitch
{
    public class DevParam
    {
        public DevParam() { }
        public DevParam(string param,string value) {
            Parameter = param;
            Value = value;

        }

        public string Parameter { get; set; }
        public string Value { get; set; }

        public bool SetToDevice(int devid)
        {
            return TelldusNETWrapper.tdSetDeviceParameter(devid, Parameter, Value);
        }
    }

    public class SwitchSetup
    {
        public SwitchSetup() { }

        private string[] param = new string[] { "house", "group", "unit" };
        private string[] types = new string[] { "-switch", "-dimmer" };

        public List<DevParam> DeviceParameters { get; set; }

        public string Model { get; set; }
        public string Protocol { get; set; }

        public bool ConfigureDevice(int id, int type, string make)
        {
            TelldusNETWrapper.tdSetModel(id, Model+(types[type])+":"+make);
            TelldusNETWrapper.tdSetProtocol(id, Protocol);
            DeviceParameters.ForEach(d => d.SetToDevice(id));
            return true;
        }

        public SwitchSetup(IDataParameters data)
        {
            var t = data.GetType();
            Protocol = data.Protocol;
            Model = data.Model;
            DeviceParameters = new List<DevParam>();
            foreach (var pname in param)
            {
                var prp = t.GetProperty(pname.UppercaseFirst());
                if (prp != null && prp.CanRead)
                {
                    var val = (string)prp.GetValue(data, null);
                    if (!string.IsNullOrEmpty(val))
                        DeviceParameters.Add(new DevParam(pname, val));
                }
            }
        }
    }
}
