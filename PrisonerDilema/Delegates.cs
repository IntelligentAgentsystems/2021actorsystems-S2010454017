using Akka.Actor;

namespace PrisonersDilema
{
    public class Delegates
    {
        public delegate IActorRef PManagementProvider();
        public delegate IActorRef WardProvider();
        public delegate IActorRef APIProvider();
    }
}
