using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TheSwitch.Core
{

    public class GroupList : List<DeviceGroup>
    {
        public void Save()
        {
            Common.WriteFile(this);
        }
    }
    
    public class DeviceGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> DeviceIds { get; set; }
        private List<TelldusDevice> _devices;

        [XmlIgnore]
        public List<TelldusDevice> Devices
        {
            get
            {
                if (_devices == null)
                {
                    var alldev = Common.Devices;
                    _devices = new List<TelldusDevice>();
                    foreach (var id in DeviceIds)
                    {
                        var dev = alldev.FirstOrDefault(d => d.Id == id);
                        if (dev != null)
                            _devices.Add(dev);
                    }

                }
                return _devices;
            }
        }

        public void TurnOn()
        {
            foreach (var dev in Devices)
            {
                dev.TurnOn();
            }
        }

        public void TurnOff()
        {
            foreach (var dev in Devices)
            {
                dev.TurnOn();
            }
        }
    }
}
