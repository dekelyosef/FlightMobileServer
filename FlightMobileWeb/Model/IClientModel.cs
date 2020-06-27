using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Model
{
    public interface IClientModel
    {
        //open the server socket
        void Connect(string ip, int port);

        //close the connection
        void Disconnect();

        //Write to server
        void Write(string command);

        //Read from server
        string Read();
    }
}
