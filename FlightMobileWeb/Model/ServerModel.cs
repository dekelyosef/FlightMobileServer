﻿using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMobileWeb.Model
{
    public class ServerModel : IServerModel
    {
        public MyClientModel client;
        public event PropertyChangedEventHandler PropertyChanged;

        //mutex
        private readonly Mutex mutex;
        private readonly Mutex m;

        public bool stop;
        public bool isConnect;

        private string note;
        //private string noteColor;

        private string ip;
        private string port;


        /**
         * Constructor
         **/
        public ServerModel(IConfiguration config)
        {
            mutex = new Mutex();
            m = new Mutex();
            client = new MyClientModel();

            Ip = config.GetValue<string>("Connections:ip");
            Port = config.GetValue<string>("Connections:socketPort");

            Connect();
            if (IsConnect())
            {
                client.Write("data\n");
            } 
        }


        /**
         * Event
         **/
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }


        /**
         * Method that called by the Set accessor of each property
         **/
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        /**
         * Opens a server to recieve data from client
         **/
        public void Connect()
        {
            bool invalid = false;

            //checks if the given port number contains only numbers
            if (!int.TryParse(port, out int portNum))
            {
                invalid = true;
                //SetNoteColor("RED");
                AddStatement("Port number is not valid, Try again...");
            }

            try
            {
                if (!IsConnect())
                {
                    //connect to client
                    this.client.Connect(ip, portNum);
                    //SetNoteColor("GREEN");
                    AddStatement("Connected to Server on ip: " + Ip + ", port: " + Port);
                    isConnect = true;
                }
            }
            catch (Exception)
            {
                if (!invalid)
                {
                    //SetNoteColor("RED");
                    AddStatement("Failed to connect! Try again..");
                }
            }
        }


        /**
         * Close the connection
         **/
        public void Disconnect()
        {
            stop = true;
            if (IsConnect())
            {
                try
                {
                    client.Disconnect();
                    isConnect = false;
                }
                catch (Exception)
                {
                    //SetNoteColor("RED");
                    AddStatement("Client was disconnected!");
                }
            }
        }


        /**
         * Is the connection open
         **/
        public Boolean IsConnect()
        {
            return this.isConnect;
        }


        /**
         * Set notification property
         **/
        public void AddStatement(string str)
        {
            //mute
            m.WaitOne();
            this.Note = str;
            m.ReleaseMutex();
        }


        /**
         * Main window notification property
         **/
        public string Note
        {
            get { return this.note; }
            set
            {
                if (this.note != value)
                {
                    this.note = value;
                    //notify the changes
                    NotifyPropertyChanged("Note");
                }
            }
        }


        /**
         * Ip property
         **/
        public string Ip
        {
            get { return this.ip; }
            set { this.ip = value; }
        }


        /**
         * Port property
         **/
        public string Port
        {
            get { return this.port; }
            set { this.port = value; }
        }


        /**
         * Write to simulator and read from simulator
         **/
        public Boolean WriteAndRead(string control, string value)
        {
            string msg = null;
            // lock
            mutex.WaitOne();
            if (control.Equals("throttle"))
            {
                msg = "set /controls/engines/current-engine/throttle " + value + "\n";
            }
            else
            {
                msg = "set /controls/flight/" + control + " " + value + "\n";
            }
            // set command doesn't return value
            client.Write(msg);
            // unlock
            mutex.ReleaseMutex();
            // checks if the value successfully set
            return WasSet(control, value);
        }


        /**
         * Checks if the given value was set
         **/
        private Boolean WasSet(string control, string value)
        {
            string msg;
            // get command to simulator
            if (control.Equals("throttle"))
            {
                msg = "get /controls/engines/current-engine/throttle\r\n";
            }
            else
            {
                msg = "get /controls/flight/" + control + "\r\n";
            }
            // write to simulator
            client.Write(msg);
            // read from simulator
            string returnVal = client.Read();


            // checks if the value was set
            if (returnVal.Equals("ERR"))
            {
                AddStatement("Error in reading from simulator");
                return false;
            } else if (float.Parse(returnVal) == float.Parse(value))
            {
                return true;
            } else
            {
                AddStatement("Error in setting the given value");
                return false;
            }
        }

    }
}
