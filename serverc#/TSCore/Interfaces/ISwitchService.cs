using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TheSwitch.Interfaces
{
    public interface IRestService
    { 
    }

    [ServiceContract]
    interface ISwitchService
    {
        [OperationContract]
        object GetUnknownDevices();
        [OperationContract]
        object AddDevice();
        [OperationContract]
        int Bell(int deviceId);
        [OperationContract]
        int Dim(int deviceId, char level);
        [OperationContract]
        int Down(int deviceId);
        [OperationContract]
        int Execute(int deviceId);
        //int GetDeviceId(int order);
        [OperationContract]
        string GetDeviceParameter(int deviceId, string name, string defaultValue);
        [OperationContract]
        int GetDeviceType(int deviceId);
        [OperationContract]
        string GetErrorString(int errorNo);
        /*[OperationContract]
        string GetModel(int deviceId);
        [OperationContract]
        string GetName(int deviceId);*/
        //[OperationContract]
        //int GetNumberOfDevices();
        //[OperationContract]
        //string GetProtocol(int deviceId);
        //int LastSentCommand(int deviceId, int methods);
        [OperationContract]
        string LastSentValue(int deviceId);
        [OperationContract]
        int Learn(int deviceId);
        [OperationContract]
        int Methods(int deviceId, int methodsSupported);
        //public int RegisterDeviceChangeEvent(DeviceChangeEventCallbackFunction deviceChangeEventFunc, object obj);
        //public int RegisterDeviceEvent(EventCallbackFunction deviceEventFunc, object obj);
        //public int RegisterRawDeviceEvent(RawListeningCallbackFunction listeningFunc, object obj);
        [OperationContract]
        bool RemoveDevice(int deviceId);
        [OperationContract]
        int SendRawCommand(string command, int reserved);
        [OperationContract]
        bool SetDeviceParameter(int deviceId, string name, string value);
        [OperationContract]
        bool SetModel(int deviceId, string model);
        [OperationContract]
        bool SetName(int deviceId, string name);
        [OperationContract]
        bool SetProtocol(int deviceId, string protocol);
        [OperationContract]
        int Stop(int deviceId);
        [OperationContract]
        int TurnOff(int deviceId);
        [OperationContract]
        int TurnOn(int deviceId);
        [OperationContract]
        int Up(int deviceId);

    }
}
