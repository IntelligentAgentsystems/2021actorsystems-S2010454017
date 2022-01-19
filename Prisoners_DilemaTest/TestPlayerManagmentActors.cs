using Akka.Actor;
using Akka.TestKit.Xunit;
using PrisonersDilema.managementActors;
using PrisonersDilema.messages;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prisoners_DilemaTest
{
    public class TestPlayerManagmentActors:TestKit
    {

        [Fact]
        public void TestGetPlayers()
        {
            var subject = Sys.ActorOf(Props.Create(() => new PlayerManagementActor()), $"{nameof(PlayerManagementActor)}");
            var request = new PlayerManagmentMessages { Message = PlayerManagmentMessages.MESSAGETYPE.GETPLAYERS };
            subject.Tell(request);
            ExpectMsgFrom<PlayerManagmentMessages>(subject);

            var answers = subject.Ask<PlayerManagmentMessages>(request).Result;
            Assert.False(answers.Players.Item1.IsNobody());
            Assert.False(answers.Players.Item2.IsNobody());
        }

        [Fact]
        public void TestGetHistory()
        {
            var subject = Sys.ActorOf(Props.Create(() => new PlayerManagementActor()), $"{nameof(PlayerManagementActor)}");
            var request = new PlayerManagmentMessages { Message = PlayerManagmentMessages.MESSAGETYPE.GETHISTORY };
            for (int i = 0; i < 100; i++)
            {
                subject.Tell(new Result { GameId = i, Years = i });
            }
            subject.Tell(request);
            ExpectMsgFrom<IDictionary<int, IList<int>>>(subject, TimeSpan.FromSeconds(5));

            var answer = subject.Ask<IDictionary<int, IList<int>>>(request).Result;
            Assert.True(answer.Count == 100);


        }

    }
}
