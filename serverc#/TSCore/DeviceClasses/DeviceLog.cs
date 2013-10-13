using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TheSwitch.Core
{
    public class DeviceLog
    {
        private static XmlSerializer ser = new XmlSerializer(typeof(DeviceLog));

        public static DeviceLog Load(string file)
        {
            DeviceLog ret = null;
            //var file = baseDir.TrimEnd('\\') + "log_" + deviceId + ".xml";
            if (File.Exists(file))
            {
                try
                {
                    using (var s = File.OpenRead(file))
                    {
                        ret = (DeviceLog)ser.Deserialize(s);
                    }
                }
                catch (Exception ex)
                { ret = new DeviceLog() { }; }

            }
            else
                ret = new DeviceLog() { };
            return ret;
        }

        
        private List<DeviceEvent> _events;
        public List<DeviceEvent> Events
        {
            get
            {
                if (_events == null)
                    _events = new List<DeviceEvent>();
                return _events;
            }
            private set
            {
                _events = value;
            }
        }


        public static void Save(string logFile, DeviceLog log)
        {
            using (var s = File.OpenWrite(logFile))
            {
                ser.Serialize(s, log);
            }
        }
    }
}
