# AkkaNetRest
Proof of concept for using Akka.NET for creating REST api services

## Using

Creating simple api "controller":

```CSHARP
  public class HelloApi : BaseApi
    {
        public HelloApi()
        {
            Get(req => "Hello from Akka");
        }
    }
```

And after instiatiate it in ```api``` actor :

```
Context.ActorOf<HelloApi>("hello");
```

now you can call ```GET``` method for ```/api/hello```
