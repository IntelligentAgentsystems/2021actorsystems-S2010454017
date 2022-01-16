using Akka.Actor;
using Prisoners_Dilema.messages;
using Prisoners_Dilema.prisoners;


namespace Prisoners_Dilema.management_actors
{
    public class PlayerManagementActor : ReceiveActor
    {
        private static readonly Random RNG = new Random();
        private static int idxFirstPlayer;
        private static int idxSecondPlayer;
        private List<IActorRef> Players { get; set; }

        public PlayerManagementActor()
        {
            idxFirstPlayer = 0;
            idxSecondPlayer = 1;
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
            ReceiveAsync<PlayerManagmentMessages>(HandleManagementMsgAsync);
            Receive<Result>(HandleResultMsg);
        }

        private void HandleResultMsg(Result res)
        {
            Players.ForEach(p => p.Tell(res));
        }

        private async Task HandleManagementMsgAsync(PlayerManagmentMessages msg)
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
                    };
                    Sender.Tell(answer);
                    break;
                case PlayerManagmentMessages.MESSAGETYPE.GETHISTORY:
                    var answers = new List<Task<PlayerMessages>>();
                    Players.AsParallel().ForAll(p =>
                    {
                        var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.GETHISTORY };
                        var t = p.Ask<PlayerMessages>(request);
                        answers.Add(t);
                    });
                    await Task.WhenAll(answers);
                    var history = answers.Select(a => a.Result).Select(a => a.History).MaxBy(a => a?.Count);
                    Sender.Tell(history);
                    break;
                default:
                    break;
            }
        }
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
               localOnlyDecider: ex => Directive.Restart
               );
        }
    }

}
