using Akka.Actor;

namespace PingPongDemo
{
    public class PingActor : ReceiveActor
    {
        private int _pingPongCount = 0;
        private readonly int cntMessages;
        private IActorRef _pongActor;

        public PingActor(int cntMessages, IActorRef pongActor)
        {
            this.cntMessages = cntMessages;
            _pongActor = pongActor;
            Receive<PingMessage>(msg =>
            {
                switch (msg)
                {
                    case PingMessage.PONG:
                        if (_pingPongCount < cntMessages)
                        {
                            Console.WriteLine($"[{Self.Path}] RECEIVED PONG FROM {_pongActor.Path}");  
                            pongActor.Tell(PongMessage.PING);
                            Console.WriteLine($"[{Self.Path}] SENDING PING TO {_pongActor.Path}");
                        }
                        else
                        {
                            pongActor.Tell(PongMessage.SHUTDOWN);
                        }
                        break;
                    case PingMessage.SHUTDOWN:
                        Console.WriteLine($"[{Self.Path}] SHUTDOWN");
                        Context.Stop(Self);
                        break;
                    case PingMessage.START:
                        Console.WriteLine($"[{Self.Path}] SEINDING FIRST PING TO {_pongActor.Path}");
                        pongActor.Tell(PongMessage.PING);
                        break;
                }
                ++_pingPongCount;
            });
        }



    }
}
