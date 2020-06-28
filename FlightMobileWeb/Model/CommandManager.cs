using FlightMobileWeb.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;

namespace FlightMobileWeb
{
    public class CommandManager
    {
        private readonly IClientModel client;
        private readonly IServerModel serverModel;
        private readonly IConfiguration configuration;
        public string Ip { get; set; }
        public int Port { get; set; }

        /**
         * Constructor
         **/
        public CommandManager(IConfiguration conf)
        {
            configuration = conf;
            client = new MyClientModel();
            serverModel = new ServerModel(conf);
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


        /**
         * Set new command's values
         **/
        public void SetNewCommand(Command command)
        {
            serverModel.WriteAndRead("aileron", command.Aileron.ToString());
            serverModel.WriteAndRead("rudder", command.Rudder.ToString());
            serverModel.WriteAndRead("elevator", command.Elevator.ToString());
            serverModel.WriteAndRead("throttle", command.Throttle.ToString());
        }
    }
}
