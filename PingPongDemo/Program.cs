using Akka.Actor;

namespace PingPongDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using ActorSystem system = ActorSystem.Create("system");
            var watcherRef = system.ActorOf<WatchActor>("watcher-1");            
            await system.WhenTerminated;
        }
    }
}