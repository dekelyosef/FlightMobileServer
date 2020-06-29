using FlightMobileWeb.Model;
using FlightMobileWeb.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FlightMobileWeb
{
    public class CommandManager
    {
        private readonly IClientModel client;
        private readonly IServerModel serverModel;
        private readonly IConfiguration configuration;
        private readonly BlockingCollection<AsyncCommand> commandsQueue;
        public string Ip { get; set; }
        public int Port { get; set; }

        /**
         * Constructor
         **/
        public CommandManager(IConfiguration conf)
        {
            configuration = conf;
            commandsQueue = new BlockingCollection<AsyncCommand>();
            client = new MyClientModel();
            serverModel = new ServerModel(conf);
            Task.Factory.StartNew(SendingCommands);
        }


        /**
         * Checks if the command with valid parameters
         **/
        public bool IsValid(Command c)
        {
            bool isThrottle = false;

            if (!IsInRange(c.Aileron, isThrottle))
            {
                return false;
            }
            else if (!IsInRange(c.Rudder, isThrottle))
            {
                return false;
            }
            else if (!IsInRange(c.Elevator, isThrottle))
            {
                return false;
            }
            isThrottle = true;
            if (!IsInRange(c.Throttle, isThrottle))
            {
                return false;
            }
            return true;
        }


        /**
         * Checks if the given value is in the range
         **/
        public bool IsInRange(double value, bool isThrottle)
        {
            if (isThrottle)
            {
                if (value < 0.0 || value > 1.0)
                {
                    return false;
                }
                return true;

            }
            if (value < -1.0 || value > 1.0)
            {
                return false;
            }
            return true;
        }


        public Task<Result> SetNewCommand(Command command)
        {
            var asyncCommand = new AsyncCommand(command);
            commandsQueue.Add(asyncCommand);
            return asyncCommand.Task;
        }


        private void SendingCommands()
        {
            foreach (AsyncCommand asyncCommand in commandsQueue.GetConsumingEnumerable())
            {
                Result result = Result.Ok;
                try
                {
                    serverModel.WriteAndRead("aileron", asyncCommand.Command.Aileron);
                    serverModel.WriteAndRead("rudder", asyncCommand.Command.Rudder);
                    serverModel.WriteAndRead("elevator", asyncCommand.Command.Elevator);
                    serverModel.WriteAndRead("throttle", asyncCommand.Command.Throttle);
                }
                catch (TimeoutException)
                {
                    // time out
                    result = Result.TimeOut;
                }
                catch
                {
                    // error in writing or reading
                    result = Result.NotFound;
                }
                asyncCommand.Completion.SetResult(result);
            }
        }



    }


}
