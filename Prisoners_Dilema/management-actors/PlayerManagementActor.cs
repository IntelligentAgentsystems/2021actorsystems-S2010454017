using Akka.Actor;
using Prisoners_Dilema.messages;
using Prisoners_Dilema.prisoners;


namespace Prisoners_Dilema.management_actors
{
    public class PlayerManagementActor : ReceiveActor
    {
        private static readonly Random RNG = new Random();
        private List<IActorRef> Players { get; set; }

        public PlayerManagementActor()
        {
            Players = new List<IActorRef>
            {
                //spawn and add prisoners
                ActorBase.Context.ActorOf<AlwaysComplyActor>($"{nameof(AlwaysComplyActor)}"),
                ActorBase.Context.ActorOf<AlwaysDefectActor>($"{nameof(AlwaysDefectActor)}"),
                ActorBase.Context.ActorOf<TitForTatActor>($"{nameof(TitForTatActor)}"),
                ActorBase.Context.ActorOf<PeriodicDefect>($"{nameof(PeriodicDefect)}"),
                ActorBase.Context.ActorOf<LocalHistoryLearningActor>($"{nameof(LocalHistoryLearningActor)}"),
                ActorBase.Context.ActorOf<StatisticLearningActor>($"{nameof(StatisticLearningActor)}")
            };

            //TODO: Prisoner died => restart prisoner with history of other actors

            Receive<PlayerManagmentMessages>(HandleManagementMsg);
        }

        private void HandleManagementMsg(PlayerManagmentMessages msg)
        {
            switch (msg.Message)
            {
                case PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS:
                    
                    //TODO jeder-gegen-jeden               
                    var selected = (Players[RNG.Next(0, Players.Count)], Players[RNG.Next(0, Players.Count)]);
                    var answer = new PlayerManagmentMessages
                    {
                        Message = PlayerManagmentMessages.MESSAGETYPE.RETURNPLAYERS,
                        Players = selected,
                        Observers = Players
                    };
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
