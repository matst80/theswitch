using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TelldusWrapper;
using TheSwitch.Core;
using TheSwitch.Interfaces;

namespace TheSwitch
{
    public class SwitchService : ISwitchService, IRestService
    {
        

        public object CreateGroup(string name)
        {
            var ret = new DeviceGroup()
            {
                Name = name,
                Id = Common.Groups.Count+1,
                DeviceIds = new List<int>()
            };
            //ret.DeviceIds.AddRange(ids.Split(',').Select(d=>int.Parse(d)));
            Common.Groups.Add(ret);
            Common.Groups.Save();
            return ret;
        }

        public object CreateRoom(string name, string description)
        {
            var ret = new Room()
            {
                Name = name,
                Id = Common.Rooms.Count+1,
            };

            Common.Rooms.Add(ret);
            Common.Rooms.Save();
            return ret;
        }

        public object GetRooms()
        {
            return Common.Rooms.ToArray();
        }

        public object GetTimedEvents()
        {
            return Common.TimedEvents.ToArray();
        }

        public object GetGroups()
        {
            return Common.Groups.ToArray();
        }

        public int CreateFromUnknown(int id, string name, int type, string make)
        {
            var unknown = Common.UnknownDevices.FirstOrDefault(d => d.Id == id);
            var newid = TelldusNETWrapper.tdAddDevice();
            TelldusNETWrapper.tdSetName(newid, name);
            var sp = unknown.Params as SwitchParameters;
            if (sp != null)
            {
                sp.GetDeviceSetup().ConfigureDevice(newid, type, make);
            }
            return newid;
        }


        public object GetDevices()
        {
            return Common.Devices;
            /*var ret = new List<TelldusDevice>();
            var noi = TelldusNETWrapper.tdGetNumberOfDevices();
            for (int i = 0; i < noi; i++)
            {
                ret.Add(new TelldusDevice(i));
            }
            return ret.ToArray();*/
        }

        public object AddDevice()
        {
            int id = TelldusNETWrapper.tdAddDevice();

            var newdevice = new TelldusDevice(id, false);
            Common.Devices.Add(newdevice);
            //Common.SaveDevices();
            return newdevice;
        }

        public int Bell(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Bell();
            return -1;
            //return TelldusNETWrapper.tdBell(deviceId);
        }

        public int Dim(int deviceId, char level)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Dim(level);
            return -1;
            //return TelldusNETWrapper.tdDim(deviceId, level);
        }

        public int Down(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Down();
            return -1;
        }

        public int Execute(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Execute();
            return -1;
        }

        public string GetDeviceParameter(int deviceId, string name, string defaultValue)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.GetDeviceParameter(name,defaultValue);
            return "not found";
        }

        public int GetDeviceType(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.GetDeviceType();
            return -1;
            //return TelldusNETWrapper.tdGetDeviceType(deviceId);
        }

        public string GetErrorString(int errorNo)
        {
            return TelldusNETWrapper.tdGetErrorString(errorNo);
        }
        /*
        public string GetModel(int deviceId)
        {
            return TelldusNETWrapper.tdGetModel(deviceId);
        }

        public string GetName(int deviceId)
        {
            return TelldusNETWrapper.tdGetName(deviceId);
        }
        
        public int GetNumberOfDevices()
        {
            return TelldusNETWrapper.tdGetNumberOfDevices();
        }
        
        public string GetProtocol(int deviceId)
        {
            return TelldusNETWrapper.tdGetProtocol(deviceId);
        }
        */
        public string LastSentValue(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.LastSentValue();
            return "not found";
            //return TelldusNETWrapper.tdLastSentValue(deviceId);
        }

        public int Learn(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Learn();
            return -1;
            //return TelldusNETWrapper.tdLearn(deviceId);
        }

        public object GetLastStatus() {
            var ret = new List<DeviceStatus>();
            foreach (var d in Common.Devices)
            {
                ret.Add(new DeviceStatus(d.Id,false));
            }
            return ret.ToArray();
        }

        public int Methods(int deviceId, int methodsSupported)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Methods(methodsSupported);
            return -1;
            //return TelldusNETWrapper.tdMethods(deviceId, methodsSupported);
        }
        
        public bool RemoveDevice(int deviceId)
        {
            return false;
            //return TelldusNETWrapper.tdRemoveDevice(deviceId);
        }
        
        public int SendRawCommand(string command, int reserved)
        {
            return TelldusNETWrapper.tdSendRawCommand(command, reserved);
        }

        public bool SetDeviceParameter(int deviceId, string name, string value)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.SetDeviceParameter(name, value);
            return false;

            //return TelldusNETWrapper.tdSetDeviceParameter(deviceId, name, value);
        }

        public bool SetModel(int deviceId, string model)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.SetModel(model);
            return false;
            //return TelldusNETWrapper.tdSetModel(deviceId, model);
        }

        public bool SetName(int deviceId, string name)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
             if (dev != null)
             {
                 dev.Name = name;
                 return true;
             }
             return false;
        }

        public bool SetProtocol(int deviceId, string protocol)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.SetProtocol(protocol);
            return false;
        }

        public int Stop(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Stop();
            return -1;
        }

        public int TurnOff(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.TurnOff();
            return -1;
        }

        public object SaveDevice(string key, string value, int id)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == id);
            if (dev != null)
            {
                dev.UpdateProperty(key, value);
            }
            Common.SaveAll();
            return dev;
        }

        public object GetLog(int deviceId,int page)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.GetLog().Events;
            return new List<DeviceLog>();
        }

        public int TurnOn(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.TurnOn();
            return -1;
        }

        public int Up(int deviceId)
        {
            var dev = Common.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (dev != null)
                return dev.Up();
            return -1;
        }

        public object GetUnknownDevices()
        {
            return Common.UnknownDevices.ToArray();
        }

        public object GetSensorValues()
        {
            return Common.SensorValues.ToArray();
        }
    }
}

