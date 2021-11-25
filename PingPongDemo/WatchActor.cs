using Akka.Actor;

namespace PingPongDemo
{
    public class WatchActor : ReceiveActor
    {
        private readonly IActorRef pingActor;
        private readonly IActorRef pongActor;
        private readonly List<IActorRef> childrenList;
        public WatchActor()
        {
            pongActor = Context.ActorOf<PongActor>("pong-1");
            pingActor = Context.ActorOf(Props.Create(() => new PingActor(100, pongActor)), "ping-1");

            childrenList = Context.GetChildren().ToList();
            foreach (var child in childrenList)
            {
                Context.Watch(child);
            }

            pingActor.Tell(PingMessage.START);

            Receive<Terminated>(terminated =>
            {

                if (childrenList.Contains(terminated.ActorRef))
                {
                    childrenList.Remove(terminated.ActorRef);
                }

                if(childrenList.Count == 0)
                {
                    Context.Stop(Self);
                    Context.System.Terminate();
                }
            });

        }

    }
}
