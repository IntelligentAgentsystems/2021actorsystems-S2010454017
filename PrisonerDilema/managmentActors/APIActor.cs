using Akka.Actor;
using PrisonersDilema.messages;

namespace PrisonersDilema.managementActors
{
    public class APIActor : ReceiveActor
    {
        IDictionary<IActorRef, List<int>> histories;

        public APIActor()
        {
            histories = new Dictionary<IActorRef, List<int>>();
            Receive<Report>(msg =>
            {
                if (histories.TryGetValue(msg.Player, out var val))
                {
                    val.Add(msg.Year);
                }
                else
                {
                    histories.Add(msg.Player, new List<int> { msg.Year });
                }

            });

            Receive<GetStatistic>(msg =>
            {
                IDictionary<IActorRef, double> avg = new Dictionary<IActorRef, double>();
                int cntRounds = 0;
                foreach (var item in histories)
                {
                    avg.Add(item.Key, item.Value.Average());
                    cntRounds += item.Value.Count;
                }

                Sender.Tell(new GetStatistic { AvgPerPrisoner = avg , RoundPlayed= cntRounds});
            });
        }
    }
}
