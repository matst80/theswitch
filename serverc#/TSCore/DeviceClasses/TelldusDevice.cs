using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TelldusWrapper;
using TheSwitch.Core;
using TheSwitch.Interfaces;



namespace TheSwitch
{

    //public class DeviceFile {
    //    public DeviceFile()
    //    {
    //        Groups = new List<DeviceGroup>();
    //        Devices = new List<TelldusDevice>();
    //        Rooms = new List<Room>();
    //        TimedEvents = new List<TimedEvent>();
    //    }
    //    public List<DeviceGroup> Groups { get; set; }
    //    public List<TelldusDevice> Devices { get; set; }

    //    public List<Room> Rooms { get; set; }

    //    public List<TimedEvent> TimedEvents { get; set; }
    //}

    public class DeviceList : List<TelldusDevice>
    {
        public void Save()
        {
            Common.WriteFile(this);
        }
    }

    public class TelldusDevice : DeviceStatus, IStoredDeviceData, IHasOnOffControls
    {
        public TelldusDevice()
        {
            logFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\log_" + Id + ".xml";
        }

        public TelldusDevice(int index, bool isindex)
            : base(index, isindex)
        {
            Name = TelldusNETWrapper.tdGetName(Id);
            Protocol = TelldusNETWrapper.tdGetProtocol(Id);
            Model = TelldusNETWrapper.tdGetModel(Id);
            //LastValue = TelldusNETWrapper.tdLastSentValue(Id);
            //LastCommand = TelldusNETWrapper.tdLastSentCommand(Id, TelldusNETWrapper.TELLSTICK_TURNON | TelldusNETWrapper.TELLSTICK_TURNOFF);
            logFile = Environment.CurrentDirectory.TrimEnd('\\') + "\\log_" + Id + ".xml";
        }

        public TelldusDevice(int index)
            : this(index, true)
        {

        }


        public static List<TelldusDevice> GetAvailableDevices()
        {
            var ret = new List<TelldusDevice>();
            var noi = TelldusNETWrapper.tdGetNumberOfDevices();
            for (int i = 0; i < noi; i++)
            {
                ret.Add(new TelldusDevice(i));
            }
            return ret;
        }


        private DeviceLog currentLog;

        private string logFile;

        public DeviceLog GetLog()
        {
            if (currentLog == null)
            {
                currentLog = DeviceLog.Load(logFile);
            }
            return currentLog;
        }

        public void AddDeviceLog(DeviceEvent evt)
        {
            var log = GetLog();
            log.Events.Add(evt);
            DeviceLog.Save(logFile, GetLog());
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                TelldusNETWrapper.tdSetName(Id, value);
                _name = value;
            }
        }
        //public string LastValue { get; set; }
        //public int Id { get; set; }

        //public int LastCommand { get; set; }
        private int _roomId;

        public int RoomId
        {
            get
            {
                return _roomId;
            }
            set
            {
                if (_roomId != value)
                    Common.RoomsChanged();
                _roomId = value;
            }
        }

        public string Description { get; set; }

        private string _protocol;
        public string Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                _protocol = value;
                SetProtocol(value);
            }
        }

        private string _model;
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                SetModel(value);
            }
        }

        public int Bell()
        {
            return TelldusNETWrapper.tdBell(Id);
        }

        public int Dim(char level)
        {
            return TelldusNETWrapper.tdDim(Id, level);
        }

        public int Down()
        {
            return TelldusNETWrapper.tdDown(Id);
        }

        public int Execute()
        {
            return TelldusNETWrapper.tdExecute(Id);
        }

        public string GetDeviceParameter(string name, string defaultValue)
        {
            return TelldusNETWrapper.tdGetDeviceParameter(Id, name, defaultValue);
        }

        public int GetDeviceType()
        {
            return TelldusNETWrapper.tdGetDeviceType(Id);
        }

        public string LastSentValue()
        {
            return TelldusNETWrapper.tdLastSentValue(Id);
        }

        public int Learn()
        {
            return TelldusNETWrapper.tdLearn(Id);
        }

        public int Methods(int methodsSupported)
        {
            return TelldusNETWrapper.tdMethods(Id, methodsSupported);
        }

        public bool SetDeviceParameter(string name, string value)
        {
            return TelldusNETWrapper.tdSetDeviceParameter(Id, name, value);
        }

        public bool SetModel(string model)
        {
            return TelldusNETWrapper.tdSetModel(Id, model);
        }

        public bool SetName(string name)
        {
            return TelldusNETWrapper.tdSetName(Id, name);
        }

        public bool SetProtocol(string protocol)
        {
            return TelldusNETWrapper.tdSetProtocol(Id, protocol);
        }

        public int Stop()
        {
            return TelldusNETWrapper.tdStop(Id);
        }

        public int TurnOff()
        {
            AddDeviceLog(new DeviceEvent(2));
            return TelldusNETWrapper.tdTurnOff(Id);
        }

        public int TurnOn()
        {
            AddDeviceLog(new DeviceEvent(1));
            return TelldusNETWrapper.tdTurnOn(Id);
        }

        public int Up()
        {
            return TelldusNETWrapper.tdUp(Id);
        }

        internal void UpdateProperty(string key, object value)
        {
            var prp = this.GetType().GetProperty(key);
            prp.SetValue(this, Convert.ChangeType(value, prp.PropertyType));
            //SaveDevices();
        }
    }
}

