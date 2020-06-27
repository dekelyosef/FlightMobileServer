using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Model
{
    public class Screenshot
    {
        /**
         * Constructor
         **/
        public Screenshot()
        {
            Ip = null;
            Port = null;
        }

        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
