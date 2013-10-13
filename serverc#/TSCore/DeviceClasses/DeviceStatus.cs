using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelldusWrapper;

namespace TheSwitch.Core
{
    public class DeviceStatus
    {
        public DeviceStatus() { }

        public DeviceStatus(int idx, bool isindex)
        {
            if (isindex)
                Id = TelldusNETWrapper.tdGetDeviceId(idx);
            else
                Id = idx;
            
            LastValue = TelldusNETWrapper.tdLastSentValue(Id);
            LastCommand = TelldusNETWrapper.tdLastSentCommand(Id, TelldusNETWrapper.TELLSTICK_TURNON | TelldusNETWrapper.TELLSTICK_TURNOFF);
            
        }

        public string LastValue { get; set; }
        public int Id { get; set; }

        public int LastCommand { get; set; }
    }
}
