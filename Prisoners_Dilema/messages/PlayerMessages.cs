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
        public int GameId { get; set;}
        public IDictionary<int,IList<int>>? History { get; set; }

    }

}
