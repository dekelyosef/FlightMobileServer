using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly CommandManager manager;

        public CommandController(CommandManager m)
        {
            manager = m;
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> PostFlightPlan([FromBody]Command command)
        {
            if (!manager.IsValid(command))
            {
                return await Task.FromResult(StatusCode(300));

            }
            if (manager.SetNewCommand(command))
            {
                return await Task.FromResult(Ok(command));
            } else
            {
                return await Task.FromResult(StatusCode(400));
            }
        }
    }

}
