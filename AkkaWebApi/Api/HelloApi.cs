using AkkaWebApi.Actors;

namespace AkkaWebApi.Api
{
    public class HelloApi : BaseApi
    {
        public HelloApi()
        {
            Get(req => "Hello from Akka");
        }
    }
}