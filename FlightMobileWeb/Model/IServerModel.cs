using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    public interface IServerModel : INotifyPropertyChanged
    {
        // connetion to the simulator
        void Connect();
        void Disconnect();
        void AddStatement(string str);
        void WriteAndRead(string control, double value);

        string Ip { set; get; }
        string Port { set; get; }
        // notification
        string Note { set; get; }
    }
}
