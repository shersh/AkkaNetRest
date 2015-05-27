using System.Collections.Generic;
using System.Net;

namespace AkkaWebApi
{
    public class Responce
    {
        public Responce(HttpStatusCode code)
        {
            Code = code;
        }

        public HttpStatusCode Code { get; private set; }

        public object Result { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}