using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSwitch.Core
{
    public class EventList : List<TimedEvent>
    {
        public void Save()
        {
            Common.WriteFile(this);
        }
    }

    public class TimedEvent
    {

    }
}
