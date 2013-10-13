using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSwitch.Interfaces
{
    public interface IHasOnOffControls
    {
        int TurnOn();
        int TurnOff();
    }
}
