using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    public class ScreenshotManager
    {
        public Screenshot screenshot = new Screenshot();
        public HttpClient client;
        public string url;


        /**
         * Constructor
         **/
        public ScreenshotManager(IConfiguration conf)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            screenshot.Ip = conf.GetValue<string>("Connections:ip");
            screenshot.Port = conf.GetValue<string>("Connections:httpPort");

            url = "http://" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";
        }


        /**
         * Set new screenshot's properties
         **/
        public void SetProperties(IConfiguration config)
        {
            screenshot.Ip = config.GetValue<string>("Connections:ip");
            screenshot.Port = config.GetValue<string>("Connections:httpPort");
        }

    }
}
