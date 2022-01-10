using Akka.Actor;
using Akka.DependencyInjection;
using Prisoners_Dilema.messages;
using Prisoners_Dilema.prisoners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema.management_actors
{
    public class PlayerManagementActor : ReceiveActor
    {
        private static readonly int NrPerType = 10;
        private static readonly Random RNG = new Random();
        private List<IActorRef> Players { get; set; }

        public PlayerManagementActor()
        {
            Players = new List<IActorRef>();
            //spawn and add prisoners
            foreach (var prisonerType in GetAllTypes())
            {
                for (int i = 0; i < NrPerType; ++i)
                {
                    var prisoner = DependencyResolver.For(ActorBase.Context.System).Props(prisonerType);
                    Players.Add(ActorBase.Context.ActorOf(prisoner, $"{prisonerType.Name}_{i}"));
                }
            }

            Receive<PlayerManagmentMessages>(HandleManagementMsg);
        }

        private void HandleManagementMsg(PlayerManagmentMessages msg)
        {
            switch (msg.Message)
            {
                case PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS:
                    var selected = (Players[RNG.Next(0, Players.Count + 1)], Players[RNG.Next(0, Players.Count + 1)]);
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

        private static Type[] GetAllTypes()
        {
            return new Type[]
            {
                typeof(AlwaysComplyActor),
                typeof(AlwaysDefectActor),
                typeof(TitForTatActor)
            };
        }
    }

}
