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
                //nothing to save because "dumb" prisoner
            });
        }

    }
}
