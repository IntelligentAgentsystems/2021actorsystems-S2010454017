using Akka.Actor;
using Prisoners_Dilema.management_actors;

namespace Prisoners_Dilema
{
    public class Program
    {
        private static readonly IPunishmentCalculator calculator = new PunsimentCalculator();
        public static async Task Main(string[] args)
        {
            using ActorSystem system = ActorSystem.Create("system");
            var playerManagment = system.ActorOf<PlayerManagementActor>($"{nameof(PlayerManagementActor)}-1");
            var referee = system.ActorOf(Props.Create(() => new RefereeActor(calculator, playerManagment)), $"{nameof(RefereeActor)}-1");

            referee.Tell(RefereeActor.LifeCycles.NEWGAME);

            await system.WhenTerminated;
        }
    }
}