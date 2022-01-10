using Akka.Actor;
using Prisoners_Dilema.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema.prisoners
{
    public class TitForTatActor:ReceiveActor
    {
        private Dictionary<IActorRef, List<PrisonerOptions>> History{ get; set; }
        private IActorRef Opponent { get; set; }

        public TitForTatActor()
        {
            //TODO: ADD TO EVERY ACTOR THE REFERENCE OF OPPONENT/ALLAY

            History = new Dictionary<IActorRef,List<PrisonerOptions>>();
            Receive<PlayerMessages>(msg =>
            {
                switch (msg)
                {
                    case PlayerMessages.REQUEST:

                        Sender.Tell(PrisonerOptions.COMPLY);
                        break;
                    default:
                        break;
                }
            });

            Receive<Result>(msg =>
            {
                if(msg.Player1 == Self)
                {
                    if (History.ContainsKey(msg.Player2))
                    {
                        History[msg.Player2].Add(msg.Player2Answer);
                    }
                    else
                    {
                        History.Add(msg.Player2, new List<PrisonerOptions>{msg.Player2Answer});
                    }
                    return;
                }

                if (History.ContainsKey(msg.Player1))
                {
                    History[msg.Player1].Add(msg.Player1Answer);
                }
                else
                {
                    History.Add(msg.Player1, new List<PrisonerOptions> { msg.Player1Answer });
                }
            });

        }

    }
}
