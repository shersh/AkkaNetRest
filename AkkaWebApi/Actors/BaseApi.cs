using System;
using System.Net;
using System.Text;
using Akka;
using Akka.Actor;
using Newtonsoft.Json;

namespace AkkaWebApi.Actors
{
    public class BaseApi : ReceiveActor
    {
        public void Get(Func<HttpListenerRequest, object> handler)
        {
            Get(req => new Responce(HttpStatusCode.OK) { Result = handler(req) });
        }

        public void Get(Func<HttpListenerRequest, Responce> handler)
        {
            Receive<HttpListenerContext>(ctx => ctx.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase),
                ctx => SendReply(handler, ctx));
        }

        public void Put(Func<HttpListenerRequest, object> handler)
        {
            Put(req => new Responce(HttpStatusCode.OK) { Result = handler(req) });
        }

        public void Put(Func<HttpListenerRequest, Responce> handler)
        {
            Receive<HttpListenerContext>(ctx => ctx.Request.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase),
                ctx => SendReply(handler, ctx));
        }

        public void Delete(Func<HttpListenerRequest, object> handler)
        {
            Delete(req => new Responce(HttpStatusCode.OK) { Result = handler(req) });
        }

        public void Delete(Func<HttpListenerRequest, Responce> handler)
        {
            Receive<HttpListenerContext>(ctx => ctx.Request.HttpMethod.Equals("DELETE", StringComparison.OrdinalIgnoreCase),
                ctx => SendReply(handler, ctx));
        }

        public void Post(Func<HttpListenerRequest, Responce> handler)
        {
            Receive<HttpListenerContext>(ctx => ctx.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase),
                ctx => SendReply(handler, ctx));
        }

        public void Post(Func<HttpListenerRequest, object> handler)
        {
            Post(req => new Responce(HttpStatusCode.OK) { Result = handler(req) });
        }

        private void SendReply(Func<HttpListenerRequest, Responce> handler, HttpListenerContext ctx)
        {
            var res = handler(ctx.Request);
            using (var stream = ctx.Response.OutputStream)
            {
                if (res == null)
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                else
                {
                    ctx.Response.StatusCode = (int)res.Code;
                    if (res.Headers != null)
                        foreach (var header in res.Headers)
                        {
                            ctx.Response.Headers[header.Key] = header.Value;
                        }

                    if (res.Result != null)
                    {
                        var s = JsonConvert.SerializeObject(res.Result);
                        var buffer = Encoding.UTF8.GetBytes(s);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        protected override void Unhandled(object message)
        {
            message.Match()
                .With<HttpListenerContext>(ctx =>
                {
                    SendReply(_ => new Responce(HttpStatusCode.NotFound), ctx);
                });
        }
    }
}