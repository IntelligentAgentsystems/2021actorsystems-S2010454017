using Akka.Actor;
using Prisoners_Dilema.messages;
using Result = Prisoners_Dilema.messages.Result;
using MessageTypes = Prisoners_Dilema.messages.PlayerMessages.PlayerMessagesType;

namespace Prisoners_Dilema.prisoners
{
    public abstract class BasePrisoner : ReceiveActor
    {
        protected IDictionary<IActorRef, IList<Result>> History { get; set; }
        protected IActorRef? Opponent { get; set; }
        protected bool FirstMove { get; set; }
        protected bool ActivePlayer { get; set; }

        public BasePrisoner()
        {
            ActivePlayer = false;
            History = new Dictionary<IActorRef, IList<Result>>();
            Receive<PlayerMessages>(PlayerMessageHandler);
            Receive<Result>(ResultMessageHandler);
        }

        private void ResultMessageHandler(Result msg)
        {
            if (!History.ContainsKey(msg.Player1))
            {
                History.Add(msg.Player1, new List<Result> { msg });
            }
            else
            {
                History[msg.Player1].Add(msg);
            }

            if (!History.ContainsKey(msg.Player2))
            {
                History.Add(msg.Player2, new List<Result> { msg });
            }
            else
            {
                History[msg.Player2].Add(msg);
            }
            if (ActivePlayer)
            {
                OnResult(msg);
            }
          
        }

        private void PlayerMessageHandler(PlayerMessages msg)
        {
            switch (msg.MessageType)
            {
                case MessageTypes.NEWGAME:
                    Opponent = msg.Opponent;
                    FirstMove = true;
                    ActivePlayer = true;
                    break;
                case MessageTypes.ENDGAME:
                    ActivePlayer = false;
                    break;
                case MessageTypes.REQUEST:
                    PrisonerOptions response = GetAnswer();
                    FirstMove = false;
                    Sender.Tell(response);
                    break;
                case MessageTypes.GETHISTORY:
                    var repsonse = new PlayerMessages { MessageType = MessageTypes.HISTORY, History = History };
                    Sender.Tell(repsonse);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Method to calculate if the prisoner comply or defect.
        /// </summary>
        /// <returns></returns>
        protected abstract PrisonerOptions GetAnswer();

        /// <summary>
        /// Provides Access to the last Result received, if the player participate 
        /// in the current game and is not an observer.
        /// </summary>
        /// <param name="result">The result for the last answer given</param>
        protected virtual void OnResult(Result result)
        {
            return;
        }
    }
}
