using Schedule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TheSwitch.Core;

namespace TheSwitch
{
    public static class Common
    {
        static ScheduleTimer Timer = new ScheduleTimer();

        public static List<UnknownDevice> UnknownDevices = new List<UnknownDevice>();
        public static List<SensorParameters> SensorValues = new List<SensorParameters>();

        private static DeviceList _devices;
        public static DeviceList Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices = (DeviceList)ReadFile(typeof(DeviceList));
                    var availDevices = TelldusDevice.GetAvailableDevices();
                    if (!_devices.Any())
                    {
                        _devices.AddRange(availDevices);
                        _devices.Save();
                    }
                    else
                    {
                        bool hasChanged = false;
                        foreach (var dev in availDevices)
                        {
                            var same = _devices.FirstOrDefault(d => d.Id == dev.Id);
                            if (same != null)
                            {
                                same.LastValue = dev.LastValue;
                                same.LastCommand = dev.LastCommand;
                                if (!same.Protocol.Equals(dev.Protocol))
                                {
                                    same.Protocol = dev.Protocol;
                                    hasChanged = true;
                                }
                                if (!same.Model.Equals(dev.Model)) { 
                                    same.Model = dev.Model;
                                    hasChanged = true;
                                }
                            }
                            else
                            {
                                hasChanged = true;
                                _devices.Add(dev);
                            }
                        }
                        if (hasChanged)
                            _devices.Save();
                    }
                }
                return _devices;
            }
        }

        private static RoomList _rooms;
        public static RoomList Rooms
        {
            get
            {
                if (_rooms == null)
                {
                    _rooms = (RoomList)ReadFile(typeof(RoomList));
                }
                return _rooms;
            }
        }

        private static EventList _timedEvents;
        public static EventList TimedEvents
        {
            get
            {
                if (_timedEvents == null)
                {
                    _timedEvents = (EventList)ReadFile(typeof(EventList));
                }
                return _timedEvents;
            }
        }


        private static GroupList _groups;
        public static GroupList Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = (GroupList)ReadFile(typeof(GroupList));
                }
                return _groups;
            }
        }

        public static object ReadFile(Type t)
        {
            object ret = null;
            XmlSerializer ser = new XmlSerializer(t);
            var deviceFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\" + t.Name.ToLower() + ".xml";
            if (!File.Exists(deviceFile))
            {
                var cons = t.GetConstructor(new Type[0]);
                return cons.Invoke(null);
            }
            using (var s = File.OpenRead(deviceFile))
            {
                ret = ser.Deserialize(s);
            }
            return ret;
        }

        public static object WriteFile(object data)
        {
            object ret = null;
            var t = data.GetType();
            XmlSerializer ser = new XmlSerializer(t);
            var deviceFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\" + t.Name.ToLower() + ".xml";
            using (var s = File.OpenWrite(deviceFile))
            {
                ser.Serialize(s, data);
            }
            return ret;
        }
        /*
        private static void GetDeviceFile()
        {
            
            var deviceFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\devicesgroups.xml";
            if (!File.Exists(deviceFile))
            {
                devicesAndGroups = new DeviceFile();

                var noi = TelldusNETWrapper.tdGetNumberOfDevices();
                for (int i = 0; i < noi; i++)
                {
                    devicesAndGroups.Devices.Add(new TelldusDevice(i));
                }
            }
            else
            {
                using (var s = File.OpenRead(deviceFile))
                {
                    devicesAndGroups = (DeviceFile)ser.Deserialize(s);
                    var noi = TelldusNETWrapper.tdGetNumberOfDevices();
                    if (devicesAndGroups.Devices.Count != noi)
                    {
                        // Sync data
                    }
                }
                SaveDevices();
            }
        }

        public static void SaveDevices()
        {
            if (devicesAndGroups != null)
            {
                var deviceFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\devicesgroups.xml";
                using (var s = File.OpenWrite(deviceFile))
                {
                    ser.Serialize(s, devicesAndGroups);
                }
            }
        }
        */
        public static string UppercaseFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }


        public static void SaveAll()
        {
            Devices.Save();
            Rooms.Save();
            TimedEvents.Save();
            Groups.Save();
        }

        public static void RoomsChanged()
        {
            _rooms = null;
        }
    }
}
