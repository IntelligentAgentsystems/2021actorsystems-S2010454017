using Akka.Actor;
using Prisoners_Dilema.messages;
using Prisoners_Dilema.prisoners;


namespace Prisoners_Dilema.management_actors
{
    public class PlayerManagementActor : ReceiveActor
    {
        private static readonly int NrPerType = 3;
        private static readonly Random RNG = new Random();
        private List<IActorRef> Players { get; set; }

        public PlayerManagementActor()
        {
            Players = new List<IActorRef>();
            //spawn and add prisoners

            for (int i = 0; i < NrPerType; ++i)
            {
                Players.Add(ActorBase.Context.ActorOf<AlwaysComplyActor>($"{nameof(AlwaysComplyActor)}_{i}"));
                Players.Add(ActorBase.Context.ActorOf<AlwaysComplyActor>($"{nameof(AlwaysDefectActor)}_{i}"));
                Players.Add(ActorBase.Context.ActorOf<TitForTatActor>($"{nameof(TitForTatActor)}_{i}"));
                Players.Add(ActorBase.Context.ActorOf<PeriodicDefect>($"{nameof(PeriodicDefect)}_{i}"));
            }

            //TODO SUPERVISING

            Receive<PlayerManagmentMessages>(HandleManagementMsg);
        }

        private void HandleManagementMsg(PlayerManagmentMessages msg)
        {
            switch (msg.Message)
            {
                case PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS:
                    var selected = (Players[RNG.Next(0, Players.Count)], Players[RNG.Next(0, Players.Count)]);
                    var answer = new PlayerManagmentMessages { Message = PlayerManagmentMessages.MESSAGETYPE.RETURNPLAYERS, 
                                                               Players = selected,
                                                               Observers = Players};
                    Sender.Tell(answer);
                    break;
                case PlayerManagmentMessages.MESSAGETYPE.RETURNPLAYERS:
                    //Nothing To do;
                    break;
                default:
                    break;
            }
        }
    }

}
