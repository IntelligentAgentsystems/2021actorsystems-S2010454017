using Akka.Actor;
using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    public class AlwaysDefectActor:ReceiveActor
    {
        public AlwaysDefectActor()
        {
            Receive<PlayerMessages>(msg =>
            {
                switch (msg)
                {
                    case PlayerMessages.REQUEST:
                        Sender.Tell(PrisonerOptions.DEFECT);
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
