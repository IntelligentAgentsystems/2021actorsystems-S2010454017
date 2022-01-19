using Akka.Actor;
using Akka.TestKit.Xunit;
using PrisonersDilema.messages;
using PrisonersDilema.prisoners;
using Xunit;

namespace Prisoners_DilemaTest
{
    public class TestPrisonerActors : TestKit
    {
        [Fact]
        public void TestAlwaysComply()
        {
            var subject = Sys.ActorOf(Props.Create(() => new AlwaysComplyActor()));
            subject.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = 0 });

            var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };
            for (int i = 0; i < 100; i++)
            {
                subject.Tell(request);
                ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);
            }
        }

        [Fact]
        public void TestAlwaysDefect()
        {
            var subject = Sys.ActorOf(Props.Create(() => new AlwaysDefectActor()));
            subject.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = 0 });

            var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };
            for (int i = 0; i < 100; i++)
            {
                subject.Tell(request);
                ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);
            }
           
        }

        [Fact]
        public void TestTitForTat()
        {
            var subject = Sys.ActorOf(Props.Create(() => new TitForTatActor()));
            subject.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = 0 });

            var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);
           
            //Opponent Defected
            subject.Tell(new Result { GameId = 0, Years = 7 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);

            //Opponent Complied
            subject.Tell(new Result { GameId = 0, Years = 7 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);

            //Opponent Complied
            subject.Tell(new Result { GameId = 0, Years = 4 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(30)]
        public void TestPeriodicDefect(int period)
        {
            var subject = Sys.ActorOf(Props.Create(() => new PeriodicDefect(period)));
            subject.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = 0 });

            var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < period; i++)
                {
                    subject.Tell(request);
                    ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);
                }
                subject.Tell(request);
                ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);
            }
        }

        [Fact]
        public void TestStatisticLearning()
        {
            var subject = Sys.ActorOf(Props.Create(() => new StatisticLearningActor()));
            subject.Tell(new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.NEWGAME, GameId = 0 });
            
            var request = new PlayerMessages { MessageType = PlayerMessages.PlayerMessagesType.REQUEST };

            for (int i = 0; i < 50; i++)
            {
                subject.Tell(new Result { GameId = 1 , Years = 7});
            }
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);

            // more defect than comply
            subject.Tell(new Result { GameId = 1, Years = 8 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);
            
            // even more defect than comply
            subject.Tell(new Result { GameId = 1, Years = 8 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);

            // nearly as many comply as defect
            subject.Tell(new Result { GameId = 1, Years = 4 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.DEFECT);

            // same again
            subject.Tell(new Result { GameId = 1, Years = 4 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);

            // more comply than defect
            subject.Tell(new Result { GameId = 1, Years = 4 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);

            // comply count is higher than defect count 
            subject.Tell(new Result { GameId = 1, Years = 7 });
            subject.Tell(request);
            ExpectMsg<PrisonerOptions>(PrisonerOptions.COMPLY);


        }

    }
}