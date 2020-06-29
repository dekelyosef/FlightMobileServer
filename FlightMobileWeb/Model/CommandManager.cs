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
        public Boolean SetNewCommand(Command command)
        {
            bool flag = true;

            flag = serverModel.WriteAndRead("aileron", command.Aileron.ToString());
            if (!flag)
            {
                return false;
            }

            flag = serverModel.WriteAndRead("rudder", command.Rudder.ToString());
            if (!flag)
            {
                return false;
            }

            flag = serverModel.WriteAndRead("elevator", command.Elevator.ToString());
            if (!flag)
            {
                return false;
            }

            flag = serverModel.WriteAndRead("throttle", command.Throttle.ToString());
            if (!flag)
            {
                return false;
            }

            return true;
        }
    }
}
