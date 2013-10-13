using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSwitch.Core
{
    public class DeviceEvent
    {
        public DeviceEvent()
        {
            
        }
        public DeviceEvent(int method) : this()
        {
            When = DateTime.Now;
            Method = method;
        }
        public DeviceEvent(int method,string data)
            : this(method)
        {
            Data = data;
        }
        public DateTime When { get;set; }
        public int Method { get; set; }
        public string Data { get; set; }
    }
}
