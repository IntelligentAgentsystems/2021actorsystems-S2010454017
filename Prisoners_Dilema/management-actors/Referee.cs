using Akka.Actor;
using Prisoners_Dilema.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema.management_actors
{
    public class Referee: ReceiveActor
    {
        private enum LifeCycles
        {
            NEWGAME,
            STARTGAME,
            TERMINATE
        }


        private static int ROUNDS = 100;
        private List<IActorRef>? Observers { get; set; }
        private (IActorRef, IActorRef) Players { get; set; }

        public Referee()
        {
            Receive<PlayerManagmentMessages>(ManagmentMsgHandler);
            ReceiveAsync<LifeCycles>(LifeCyclesHandlerAsync);
        }

        private async Task LifeCyclesHandlerAsync(LifeCycles msg)
        {
            switch (msg)
            {
                case LifeCycles.NEWGAME:
                    var request = new PlayerManagmentMessages { Message = PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS };
                    ActorBase.Context.System.ActorSelection("/player-management").Tell(request);
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
                    Self.Tell(LifeCycles.STARTGAME);
                    break;
                default:
                    break;
            }
        }

        private async Task StartGameAsync()
        {
            for (int i = 0; i < ROUNDS; i++)
            {
                var answer1 = (PrisonerOptions) await Players.Item1.Ask(PlayerMessages.REQUEST, timeout: TimeSpan.FromSeconds(2.0));
                var answer2 = (PrisonerOptions) await Players.Item2.Ask(PlayerMessages.REQUEST, timeout: TimeSpan.FromSeconds(2.0));

                //CALCULATE PRSIONTIME
                int time = 0;
                var result = new Result { Player1 = Players.Item1, Player2 = Players.Item2 , 
                                          Player1Answer = answer1, Player2Answer = answer2, Years = time};
            
                Observers.AsParallel().ForAll(player => player.Tell(result));
            }

        } 

    }
}
