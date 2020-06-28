using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightMobileWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace FlightMobileWeb.Controllers
{
    [Route("/screenshot")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly string url;

        public ScreenshotController(IConfiguration conf, IHttpClientFactory clientFactory)
        {
            client = clientFactory.CreateClient("screenshot");
            client.Timeout = TimeSpan.FromSeconds(10);

            string ip = conf.GetValue<string>("ip");
            string port = conf.GetValue<string>("Connections:httpPort");

            url = "http://" + ip + ":" + port + "/screenshot";
        }


        [HttpGet]
        public async Task<ActionResult> GetScreenshot()
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("couldn't get screenshot from the simulator");
                }
            }
            catch
            {
                return NotFound("couldn't get screenshot from the simulator");
            }

            var content = response.Content;
            var bytes = await content.ReadAsByteArrayAsync();

            return File(bytes, "image/jpeg");
        }
    }

}