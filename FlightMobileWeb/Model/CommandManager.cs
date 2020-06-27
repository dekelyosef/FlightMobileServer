using FlightMobileApp.Model;
using System;

namespace FlightMobileApp
{
    public class CommandManager
    {
        /**
         * Constructor
         **/
        public CommandManager() { }


        /**
         * Checks if the command with valid parameters
         **/
        public static bool IsValid(Command c)
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
        public static bool IsInRange(double value, bool isThrottle)
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
        public static void SetNewCommand(IServerModel myServer, Command command)
        {
            myServer.WriteAndRead("aileron", command.Aileron.ToString());
            myServer.WriteAndRead("rudder", command.Rudder.ToString());
            myServer.WriteAndRead("elevator", command.Elevator.ToString());
            myServer.WriteAndRead("throttle", command.Throttle.ToString());
        }
    }
}
