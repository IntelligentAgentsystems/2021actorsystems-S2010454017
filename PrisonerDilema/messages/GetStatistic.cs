using Akka.Actor;

namespace PrisonersDilema.messages
{
    public record GetStatistic
    {
        public IDictionary<IActorRef, double>? AvgPerPrisoner { get; set; }
        public int RoundPlayed { get; set; }
    }
}
