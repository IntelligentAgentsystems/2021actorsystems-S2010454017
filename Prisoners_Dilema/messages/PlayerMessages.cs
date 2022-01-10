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
        }

        public PlayerMessagesType MessageType { get; init; }
        public IActorRef Opponent { get; init; }
    }

}
