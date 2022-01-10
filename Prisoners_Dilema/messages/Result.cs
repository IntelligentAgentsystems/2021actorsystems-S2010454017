using Akka.Actor;


namespace Prisoners_Dilema.messages
{
    record class Result
    {
        public IActorRef Player1 { get; init; }
        public IActorRef Player2 { get; init; }
        public PrisonerOptions Player1Answer { get; init; }
        public PrisonerOptions Player2Answer { get; init; }
        public int Years { get; init; }
        
    }
}
