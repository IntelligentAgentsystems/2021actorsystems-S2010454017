using Akka.Actor;
using PrisonersDilema.messages;
using PrisonersDilema.prisoners;


namespace PrisonersDilema.managementActors
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
                ActorBase.Context.ActorOf<StatisticLearningActor>($"{nameof(StatisticLearningActor)}"),
                ActorBase.Context.ActorOf<RandomActor>($"{nameof(RandomActor)}")
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
            var combinations = new List<(IActorRef, IActorRef)>();
            for(int i = 0; i < Players.Count; i++)
            {
                for (int j = 0; j < Players.Count; j++)
                {
                    if(i < j)
                    {
                        combinations.Add((Players[i], Players[j]));
                    }
                }
            }

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
