using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlightMobileWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;


using System.Collections.Concurrent;

using System.Net.Http;

using FlightMobileWeb.Models;

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
        public async Task<ActionResult> PostCommand(Command command)
        {
            if (!manager.IsValid(command))
            {
                return await Task.FromResult(StatusCode(400));
            }

            Task<Result> resTask = manager.SetNewCommand(command);
            resTask.Wait();

            if (resTask.Result == Result.Ok)
            {
                return await Task.FromResult(Ok(command));
            }

            if (resTask.Result == Result.TimeOut)
            {
                return await Task.FromResult(StatusCode(600));
            }

            // not found - error in writing or reading
            return await Task.FromResult(StatusCode(300));
        }
    }


}
