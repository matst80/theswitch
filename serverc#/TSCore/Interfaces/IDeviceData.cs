using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSwitch.Interfaces
{
    public interface IDeviceData
    {
        string Protocol { get; set; }
        string Model { get; set; }
    }

    public interface IStoredDeviceData : IDeviceData
    {
        int RoomId { get; set; }
        string Name { get;set;}
        string Description { get; set; }
    }
}
