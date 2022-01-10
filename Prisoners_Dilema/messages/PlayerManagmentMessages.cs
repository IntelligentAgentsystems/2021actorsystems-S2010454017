using Akka.Actor;

namespace Prisoners_Dilema.messages
{
    public class PlayerManagmentMessages
    {
        public enum MESSAGETYPE
        {
            GETPLAYERS,
            RETURNPLAYERS
        }
        public MESSAGETYPE Message { get; init; }
        public (IActorRef, IActorRef) Players { get; set; }
        public List<IActorRef> Observers { get; set; }
    }
}
