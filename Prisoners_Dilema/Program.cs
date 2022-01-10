using Akka.Actor;

namespace Prisoners_Dilema
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using ActorSystem system = ActorSystem.Create("system");
            await system.WhenTerminated;
        }
    }
}