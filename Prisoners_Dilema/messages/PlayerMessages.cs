using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema.messages
{
    
    public record PlayerMessages
    {
        public enum PlayerMessagesType
        {
            REQUEST,
            NEWGAME,
            ENDGAME,
            GETHISTORY,
            HISTORY
        }

        public PlayerMessagesType MessageType { get; init; }
        public IActorRef? Opponent { get; set; }
        public IDictionary<IActorRef,IList<Result>>? History { get; set; }

    }

}
