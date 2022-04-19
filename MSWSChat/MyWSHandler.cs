using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace MSWSChat
{
    public class MyWSHandler : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        public override void OnOpen()
        {
            //this.Send("Welcom from " + this.WebSocketContext.UserHostAddress);
            clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            if (message == "ping")
            {
                this.Send("pong");
            }
            else if (message == "requestClose")
            {
                this.Close();
            }
            else
            {
                foreach (var c in clients)
                {
                    if (c != this && c.WebSocketContext.IsClientConnected)
                    {
                        c.Send(message);
                    }
                }
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
            base.OnClose();
        }

        public override void OnError()
        {
            base.OnError();
        }
    }
}