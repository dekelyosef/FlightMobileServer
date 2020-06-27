using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        /**
         * Constructor
         **/
        public ScreenshotController(IConfiguration config)
        {
            ScreenshotManager.SetProperties(config);
        }


        // GET: api/Screenshot
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Byte[] bytes = await ScreenshotManager.GetScreenshotBytes();
            return File(bytes, "image/jpg");
        }

    }
}
