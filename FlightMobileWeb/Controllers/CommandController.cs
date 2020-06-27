using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly IServerModel myServer;


        /**
         * Constructor
         **/
        public CommandController(IServerModel serverModel)
        {
            myServer = serverModel;
        }


        // POST: api/Command
        [HttpPost]
        public ActionResult Post(Command command)
        {
            if (!CommandManager.IsValid(command))
            {
                return NotFound();
            }
            try
            {
                CommandManager.SetNewCommand(myServer, command);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
