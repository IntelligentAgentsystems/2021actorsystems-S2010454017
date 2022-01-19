using Akka.Actor;

namespace PrisonersDilema.messages
{
    public class Report
    {
        public IActorRef Player { get; init; }
        public int Year { get; init; }
    }
}
