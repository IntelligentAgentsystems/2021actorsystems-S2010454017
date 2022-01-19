using Akka.Actor;

namespace PrisonersDilema.messages
{
    public class PlayerManagmentMessages
    {
        public enum MESSAGETYPE
        {
            GETPLAYERS,
            RETURNPLAYERS,
            GETHISTORY,
        }
        public MESSAGETYPE Message { get; init; }
        public (IActorRef, IActorRef) Players { get; set; }
    }
}
