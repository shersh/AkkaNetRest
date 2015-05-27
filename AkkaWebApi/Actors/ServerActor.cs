using System;
using System.Net;
using Akka.Actor;
using AkkaWebApi.Api;

namespace AkkaWebApi.Actors
{
    public class ServerActor : ReceiveActor
    {
        private HttpListener _listener;

        public ServerActor()
        {
            _listener = new HttpListener();
            Receive<string>(s => s.Equals("start"), s =>
            {
                _listener.Prefixes.Add("http://*:5555/");
                _listener.Start();
                DoListen();
            });

            Receive<HttpListenerContext>(context =>
            {
                var handler = Context.ActorSelection("/user" + context.Request.RawUrl);
                try
                {
                    var actorRef = handler.ResolveOne(TimeSpan.FromMilliseconds(10)).Result;
                    actorRef.Tell(context);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    using (var str = context.Response.OutputStream) { }
                }
                finally
                {
                    DoListen();
                }
            });
        }

        protected override void Unhandled(object message)
        {
            base.Unhandled(message);
        }

        private void DoListen()
        {
            _listener.GetContextAsync().PipeTo(Self);
        }

        protected override void PreStart()
        {
            var actorRef = Context.ActorOf<HelloApi>("hello");
            base.PreStart();
        }
    }
}