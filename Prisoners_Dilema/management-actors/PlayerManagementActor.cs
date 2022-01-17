using Akka.Actor;
using Prisoners_Dilema.messages;
using Prisoners_Dilema.prisoners;


namespace Prisoners_Dilema.management_actors
{
    public class PlayerManagementActor : ReceiveActor
    {
        private static readonly Random RNG = new();

        private List<IActorRef> Players { get; set; }
        private List<(IActorRef, IActorRef)> Games { get; set; }
        private static int currentGame;

        public PlayerManagementActor()
        { 
            currentGame = 0;
            Players = new List<IActorRef>
            {
                //spawn and add prisoners
                ActorBase.Context.ActorOf<AlwaysComplyActor>($"{nameof(AlwaysComplyActor)}"),
                ActorBase.Context.ActorOf<AlwaysDefectActor>($"{nameof(AlwaysDefectActor)}"),
                ActorBase.Context.ActorOf<TitForTatActor>($"{nameof(TitForTatActor)}"),
                ActorBase.Context.ActorOf(Props.Create(() => new PeriodicDefect(10)),$"{nameof(PeriodicDefect)}"),
                ActorBase.Context.ActorOf<LocalHistoryLearningActor>($"{nameof(LocalHistoryLearningActor)}"),
                ActorBase.Context.ActorOf<StatisticLearningActor>($"{nameof(StatisticLearningActor)}")
            };
            Games = CreateGames();
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

                    var selected = currentGame < Games.Count ? Games[currentGame]: Games[RNG.Next(0, Games.Count)];
                    ++currentGame;
                    var answer = new PlayerManagmentMessages
                    {
                        Message = PlayerManagmentMessages.MESSAGETYPE.RETURNPLAYERS,
                        Players = selected,
                    };
                    Sender.Tell(answer);
                    break;
                case PlayerManagmentMessages.MESSAGETYPE.GETHISTORY:
                    var answers = new List<Task<PlayerMessages>>();
                    Players.ForEach(p =>
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

        private List<(IActorRef, IActorRef)> CreateGames()
        {
            var combinations = from p1 in Players
                               from p2 in Players
                               where p1 != p2
                               select (p1, p2);
            return combinations.ToList();
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
