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
    [Route("screenshot")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private ScreenshotManager manager;
        private readonly HttpClient client;
        private readonly string url;

<<<<<<< HEAD
        public ScreenshotController(IConfiguration conf, HttpClient clientFactory)
        {
            client = clientFactory;
            //client = clientFactory.CreateClient("screenshot");
            client.Timeout = TimeSpan.FromSeconds(10);

            string ip = conf.GetValue<string>("Connections:ip");
            string port = conf.GetValue<string>("Connections:httpPort");

            url = "http://" + ip + ":" + port + "/screenshot";
=======
        public ScreenshotController(ScreenshotManager m)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            manager = m;
>>>>>>> 039aeab43ebc2ffc7b7757286bd3171354d335af
        }


        [HttpGet]
        public async Task<ActionResult> GetScreenshot()
        {
            HttpResponseMessage response;
            try
            {
                response = await manager.client.GetAsync(manager.url);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("couldn't get screenshot");
                }
            }
            catch
            {
                return NotFound("couldn't get screenshot");
            }


            var content = response.Content;
            var screenshot = await content.ReadAsByteArrayAsync();

            return File(screenshot, "image/jpeg");
        }
    }

}