using System;
namespace TheSwitch.Interfaces
{
    public interface IDataParameters
    {
        string Class { get; set; }
        string Group { get; set; }
        string Method { get; set; }
        string Model { get; set; }
        string Protocol { get; set; }
    }
}
