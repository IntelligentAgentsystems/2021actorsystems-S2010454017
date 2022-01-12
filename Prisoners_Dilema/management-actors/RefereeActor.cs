using Akka.Actor;
using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.management_actors
{
    public class RefereeActor: ReceiveActor
    {
        public enum LifeCycles
        {
            NEWGAME,
            STARTGAME,
            TERMINATE
        }

        private readonly IPunishmentCalculator punishmentCalculator;

        private IActorRef PlayerManagment { get; set; }

        private static readonly int ROUNDS = 100;
        private static readonly int TOURNAMENTS = 100;
        private List<IActorRef>? Observers { get; set; }
        private (IActorRef, IActorRef) Players { get; set; }
        private int PassedTournaments { get; set; } = 0;

        public RefereeActor(IPunishmentCalculator calculator, IActorRef playerManagment)
        {
            PlayerManagment = playerManagment;
            punishmentCalculator = calculator;
            Receive<PlayerManagmentMessages>(ManagmentMsgHandler);
            ReceiveAsync<LifeCycles>(LifeCyclesHandlerAsync);
        }

        private async Task LifeCyclesHandlerAsync(LifeCycles msg)
        {
            switch (msg)
            {
                case LifeCycles.NEWGAME:
                    var request = new PlayerManagmentMessages { Message = PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS };
                    PlayerManagment.Tell(request);
                    break;
                case LifeCycles.STARTGAME:
                    await StartGameAsync();
                    break;
            }
        }
        private void ManagmentMsgHandler(PlayerManagmentMessages msg)
        {
            switch (msg.Message)
            {
                case PlayerManagmentMessages.MESSAGETYPE.RETURNPLAYERS:
                    Observers = msg.Observers;
                    Players = msg.Players;
                    Players.Item1.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, Opponent = Players.Item2 });
                    Players.Item2.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, Opponent = Players.Item1 });
                    Self.Tell(LifeCycles.STARTGAME);
                    break;
                default:
                    break;
            }
        }

        private async Task StartGameAsync()
        {
            Console.WriteLine($"Tournament {PassedTournaments}");
            for (int i = 0; i < ROUNDS; i++)
            {
                var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };

                var answer1 = (PrisonerOptions) await Players.Item1.Ask(request, timeout: TimeSpan.FromSeconds(2.0));
                var answer2 = (PrisonerOptions) await Players.Item2.Ask(request, timeout: TimeSpan.FromSeconds(2.0));

                int time = punishmentCalculator.GetPunishmentInYears(answer1, answer2);
                var result = new Result { Player1 = Players.Item1, Player2 = Players.Item2 , 
                                          Player1Answer = answer1, Player2Answer = answer2, 
                                          Years = time};
            
                Observers?.AsParallel().ForAll(player => player.Tell(result));

                Console.WriteLine($"\t[ROUND {i}] {result.Player1.Path.Name}: {result.Player1Answer}; {result.Player2.Path.Name}: {result.Player2Answer}" +
                    $" YEARS: {result.Years}");

            }
            var requestEndGamae = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.ENDGAME };
            Players.Item1.Tell(requestEndGamae);
            Players.Item2.Tell(requestEndGamae);

            if(PassedTournaments < TOURNAMENTS)
            {
                ++PassedTournaments;
                Self.Tell(LifeCycles.NEWGAME);
            }
           
        } 

    }
}
