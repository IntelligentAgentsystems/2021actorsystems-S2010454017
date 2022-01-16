using Akka.Actor;
using Prisoners_Dilema.messages;
using Result = Prisoners_Dilema.messages.Result;
using MessageTypes = Prisoners_Dilema.messages.PlayerMessages.PlayerMessagesType;

namespace Prisoners_Dilema.prisoners
{
    public abstract class BasePrisoner : ReceiveActor
    {
        protected IDictionary<int, IList<int>> History { get; set; }
        protected PrisonerOptions LastChoice { get; set; }

        protected int GameId { get; set; }

        protected readonly IPunishmentCalculator calculator;
        protected bool FirstMove { get; set; }
        protected bool ActivePlayer { get; set; }

        public BasePrisoner()
        {
            calculator = new PunsimentCalculator();
            ActivePlayer = false;
            LastChoice = PrisonerOptions.COMPLY;
            History = new Dictionary<int, IList<int>>();
            Receive<PlayerMessages>(PlayerMessageHandler);
            Receive<Result>(ResultMessageHandler);
            Receive<IDictionary<int, IList<int>>>(msg => History = msg);
        }

        private void ResultMessageHandler(Result msg)
        {
            if (History.ContainsKey(msg.GameId))
            {
                History[msg.GameId].Add(msg.Years);
            }
            else
            {
                History.Add(msg.GameId, new List<int> { msg.Years });
            }

            OnResult(msg);

        }

        private void PlayerMessageHandler(PlayerMessages msg)
        {
            switch (msg.MessageType)
            {
                case MessageTypes.NEWGAME:
                    FirstMove = true;
                    ActivePlayer = true;
                    GameId = msg.GameId;
                    GameStarted();
                    break;
                case MessageTypes.ENDGAME:
                    ActivePlayer = false;
                    GameEnded();
                    break;
                case MessageTypes.REQUEST:
                    PrisonerOptions response = GetAnswer();
                    FirstMove = false;
                    LastChoice = response;
                    Sender.Tell(response);
                    break;

                default:
                    break;
            }
        }

        protected override void PostRestart(Exception reason)
        {
            Console.Error.WriteLine($"{Self.Path.Name} POSTRESTART!");
            var request = new PlayerMessages { MessageType = MessageTypes.GETHISTORY, History = History };
            Context.Parent.Tell(request);
            base.PostRestart(reason);
        }

        protected abstract PrisonerOptions GetAnswer();
        protected virtual void OnResult(Result result) { }
        protected virtual void GameEnded() { }
        protected virtual void GameStarted() { }
    }
}
