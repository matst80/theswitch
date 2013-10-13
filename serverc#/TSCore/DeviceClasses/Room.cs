using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TheSwitch.Interfaces;


namespace TheSwitch.Core
{
    public class RoomList : List<Room>
    {
        public void Save()
        {
            Common.WriteFile(this);
        }
    }
    
    public class Room : IHasOnOffControls
    {
        public Room()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

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
                    foreach (var d in alldev)
                    {
                        if (d.RoomId==Id)
                            _devices.Add(d);
                    }

                }
                return _devices;
            }
        }

        public int TurnOn()
        {
            
            foreach (var dev in Devices)
            {
                dev.TurnOn();
            }
            return 0; // Add chechsum
        }

        public int TurnOff()
        {
            foreach (var dev in Devices)
            {
                dev.TurnOn();
            }
            return 0;// Add chechsum
        }
    }
}
