using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Model
{
    public interface IServerModel : INotifyPropertyChanged
    {
        // connetion to the simulator
        void Connect();
        void Disconnect();
        void AddStatement(string str);
        void WriteAndRead(string control, string value);

        string Ip { set; get; }
        string Port { set; get; }
        // notification
        string Note { set; get; }
        //string NoteColor { set; get; }
        // controls
        //double Aileron { set; get; }
        //double Rudder { set; get; }
        //double Elevator { set; get; }
        //double Throttle { set; get; }
    }
}
