using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSwitch.Interfaces;


namespace TheSwitch
{
    public class SensorParameters : DataParameters
    {
        public SensorParameters() { }
        public SensorParameters(string data)
            : base(data)
        {

        }
        public string Temp { get; set; }
        public string Humidity { get; set; }
    }

    public class SwitchParameters : DataParameters
    {
        public SwitchParameters() { }
        public SwitchParameters(string data)
            : base(data)
        {

        }
        public string House { get; set; }
        public string Unit { get; set; }

        

        public SwitchSetup GetDeviceSetup()
        {
            return new SwitchSetup(this);
        }

    }

    public class DataParameters : IDataParameters, IDeviceData
    {
        

        public DataParameters() { }
        public DataParameters(string data)
        {
            foreach (var part in data.Trim().TrimEnd(';').Split(';'))
            {
                var pp = part.Split(':');
                var prp = this.GetType().GetProperty(pp[0].UppercaseFirst());
                if (prp != null)
                    prp.SetValue(this, pp[1]);
                else
                    Console.WriteLine("Not found prp:" + pp[0]);
            }
        }
        public string Class { get; set; }
        public string Id { get; set; }

        public string Protocol { get; set; }
        public string Model { get; set; }
        public string Method { get; set; }
        public string Group { get; set; }

    }

    public class UnknownDevice
    {
        public SwitchParameters Params { get; set; }
        public string Data { get; set; }
        public int ControllerId { get; set; }
        public int CallbackId { get; set; }

        public DateTime When { get; set; }

        public int Id { get; set; }
    }
}
