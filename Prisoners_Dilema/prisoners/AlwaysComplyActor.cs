using Akka.Actor;
using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    public class AlwaysComplyActor:ReceiveActor
    {
        public AlwaysComplyActor()
        {
            Receive<PlayerMessages>(msg =>
            {
                switch (msg.MessageType)
                {
                    case PlayerMessages.PlayerMessagesType.NEWGAME:
                        //nothing to do here
                        break;
                    case PlayerMessages.PlayerMessagesType.REQUEST:
                        Sender.Tell(PrisonerOptions.COMPLY, Self);
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
