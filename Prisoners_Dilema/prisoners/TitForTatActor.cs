using Akka.Actor;
using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    /* public class TitForTatActor:ReceiveActor
     {
         private IActorRef Opponent { get; set; }
         private PrisonerOptions lastOpponentOption = PrisonerOptions.COMPLY;

         public TitForTatActor()
         {           
             Receive<PlayerMessages>(msg =>
             {
                 switch (msg.MessageType)
                 {
                     case PlayerMessages.PlayerMessagesType.NEWGAME:
                         lastOpponentOption = PrisonerOptions.COMPLY;
                         Opponent = msg.Opponent;
                         break;

                     case PlayerMessages.PlayerMessagesType.REQUEST:
                         Sender.Tell(lastOpponentOption, Self);
                         break;
                     default:
                         break;

                 }
             });

             Receive<Result>(msg =>
             {
                 if (msg.Player1.Equals(Self))
                 {
                     lastOpponentOption = msg.Player2Answer;
                 }
                 else
                 {
                     lastOpponentOption = msg.Player1Answer;
                 }
             });
         }
     }*/

    public class TitForTatActor : BasePrisoner
    {
        private PrisonerOptions lastOpponentOption;

        public TitForTatActor():base()
        {
            lastOpponentOption = PrisonerOptions.COMPLY;
        }

        protected override void OnResult(Result result)
        {
            lastOpponentOption = result.Player1.Equals(Self) ? result.Player2Answer :result.Player1Answer;
        }

        protected override PrisonerOptions GetAnswer()
        {
            if (FirstMove)
            {
                return PrisonerOptions.COMPLY;
            }
            return lastOpponentOption;
        }
    }
}
