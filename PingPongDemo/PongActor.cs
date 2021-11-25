using Akka.Actor;
namespace PingPongDemo
{
    public class PongActor: ReceiveActor
    {
        public PongActor()
        {
            Receive<PongMessage>(msg =>
            {
                switch (msg)
                {
                    case PongMessage.PING:
                        Console.WriteLine($"[{Self.Path}] RECEIVED PING FROM {Sender.Path}");
                        Sender.Tell(PingMessage.PONG);
                        Console.WriteLine($"[{Self.Path}] SENDING PONG TO {Sender.Path}");
                        break;
                    case PongMessage.SHUTDOWN:
                        Console.WriteLine($"[{Self.Path}] SHUTDOWN");
                        Sender.Tell(PingMessage.SHUTDOWN);
                        Context.Stop(Self);
                        break;
                }
            });
        }
    }
}
