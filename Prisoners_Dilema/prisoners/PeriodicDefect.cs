using Akka.Actor;
using Prisoners_Dilema.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema.prisoners
{
    public class PeriodicDefect:ReceiveActor
    {
        private static readonly int defectPeriod = 10;
        private int currentCount { get; set; } = 0;
        public PeriodicDefect()
        {
            Receive<PlayerMessages>(msg =>
            {
                switch (msg.MessageType)
                {
                    case PlayerMessages.PlayerMessagesType.NEWGAME:
                        //nothing to do here
                        break;
                    case PlayerMessages.PlayerMessagesType.REQUEST:
                        if(currentCount < defectPeriod)
                        {
                            ++currentCount;
                            Sender.Tell(PrisonerOptions.COMPLY, Self);
                        }
                        else
                        {
                            currentCount = 0;
                            Sender.Tell(PrisonerOptions.DEFECT, Self);
                        }
                        break;
                    default:
                        break;
                }
            });

            Receive<Result>(msg =>
            {
                //nothing to save because "dumb" prisoner
            });
        }
    }
}
