﻿using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace M9_WebSocketServer.Controllers
{

    public class WebSocketController : ApiController
    {
        public HttpResponseMessage Get(string nom)
        {
            // La crida al websocket serà del tipus   ws://host:port/api/websocket?nom=Pere

            HttpContext.Current.AcceptWebSocketRequest(new SocketHandler(nom)); return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private class SocketHandler : WebSocketHandler
        {
            private static readonly WebSocketCollection Sockets = new WebSocketCollection();

            private readonly string _nom;

            public SocketHandler(string nom)
            {
                _nom = nom;
            }

            public override void OnOpen()
            {

                // Quan es connecta un nou usuari: cal afegir el SocketHandler a la Collection, notificar a tothom la incorporació i donar-li la benvinguda

                Sockets.Add(this);

                Sockets.Broadcast(_nom+" Se incluye en el videojuego!!!!"); 




            }

            public override void OnMessage(string missatge)

            {

                // Quan un usuari envia un missatge, cal que tothom el rebi

                Sockets.Broadcast(_nom+":"+missatge); 



            }

            public override void OnClose()
            {
                // Quan un usuari desconnecta, cal acomiadar-se'n, esborrar-ne el SocketHandler de la Collection i notificar a la resta que marxa

                this.Send(string.Format("A reveure,{0}!", _nom));
                Sockets.Remove(this);
                Sockets.Broadcast(string.Format("{0} ha marxat del grup", _nom));


            }
        }
    }

}