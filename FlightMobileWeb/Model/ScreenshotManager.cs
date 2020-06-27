using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightMobileApp.Model
{
    public class ScreenshotManager
    {
        private static readonly Screenshot screenshot = new Screenshot();


        /**
         * Constructor
         **/
        public ScreenshotManager() { }


        /**
         * Set new screenshot's properties
         **/
        public static void SetProperties(IConfiguration config)
        {
            //screenshot.Ip = config.GetConnectionString("ip");
            //screenshot.Port = config.GetConnectionString("httpPort");

            screenshot.Ip = config.GetValue<string>("Connections:ip");
            screenshot.Port = config.GetValue<string>("Connections:httpPort");
        }


        /**
         * Get the screenshot
         **/
        public static async Task<Byte[]> GetScreenshotBytes()
        {
            dynamic bytes = (dynamic)null;
            string url = "http://" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";

            try
            {
                using HttpClient client = new HttpClient
                {
                    //set time out for the request
                    Timeout = TimeSpan.FromSeconds(10)
                };
                // request the screenshot from client
                bytes = await client.GetByteArrayAsync(url);
            }
            catch (Exception)
            {
                throw new Exception("Error in getting the screenshot");
            }

            return bytes;
        }

    }
}
