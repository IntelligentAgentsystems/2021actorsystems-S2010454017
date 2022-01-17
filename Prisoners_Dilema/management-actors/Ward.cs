﻿using Akka.Actor;
using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.management_actors
{
    public class Ward : ReceiveActor
    {
        public enum LifeCycles
        {
            NEWGAME,
            STARTGAME,
            TERMINATE
        }

        private readonly IPunishmentCalculator punishmentCalculator;
        private static readonly int ROUNDS = 1000;
        private static int lastRound;
        private IActorRef PlayerManagment { get; set; }
        private (IActorRef, IActorRef) Players { get; set; }
        private int PassedTournaments { get; set; }

        public Ward(IPunishmentCalculator calculator, IActorRef playerManagment)
        {
            PassedTournaments = 0;
            lastRound = 0;
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
                    Players = msg.Players;
                    Self.Tell(LifeCycles.STARTGAME);
                    break;
                default:
                    break;
            }
        }

        private async Task StartGameAsync()
        {
            try
            {
                Players.Item1.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = PassedTournaments });
                Players.Item2.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = PassedTournaments });
                Console.WriteLine($"Tournament {PassedTournaments}");
                while (lastRound < ROUNDS)
                {
                    var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };

                    var answer1 = await Players.Item1.Ask<PrisonerOptions>(request, timeout: TimeSpan.FromSeconds(2.0));
                    var answer2 = await Players.Item2.Ask<PrisonerOptions>(request, timeout: TimeSpan.FromSeconds(2.0));

                    int time = punishmentCalculator.GetPunishmentInYears(answer1, answer2);
                    var result = new Result { GameId = PassedTournaments, Years = time };

                    Context.ActorSelection($"{nameof(PlayerManagementActor)}").Tell(result);

                    ++lastRound;
                    Console.WriteLine($"\t[ROUND {lastRound}] ({Players.Item1.Path.Name} vs. {Players.Item2.Path.Name})" +
                        $"({answer1} vs {answer2} YEARS: {result.Years}");

                }
                var requestEndGamae = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.ENDGAME };
                Players.Item1.Tell(requestEndGamae);
                Players.Item2.Tell(requestEndGamae);
                lastRound = 0;

                ++PassedTournaments;
                Self.Tell(LifeCycles.NEWGAME);
            }
            catch (AskTimeoutException)
            {
                await Task.Delay(2000);
                Self.Tell(LifeCycles.NEWGAME);
            }
           
        }

    }
}
