using PrisonersDilema.messages;


namespace PrisonersDilema.prisoners
{
    public class LocalHistoryLearningActor : BasePrisoner
    {
        private IList<PrisonerOptions> OpponentChoices { get; set; }
        private IList<PrisonerOptions> OwnChoices { get; set; }
        private bool TitForTat;
        private bool periodChoice;

        public int period { get; private set; }

        private readonly Random random;
        private readonly int LearnRounds;
        private int currentRound;

        public LocalHistoryLearningActor() : base()
        {
            LearnRounds = 200;
            random = new Random();
            OpponentChoices = new List<PrisonerOptions>();
            OwnChoices = new List<PrisonerOptions>();
        }

        protected override PrisonerOptions GetAnswer()
        {
            ++currentRound;
            if (currentRound < LearnRounds)
            {
                // random behavoiur to get a idee of opponent
                return random.NextDouble() > 0.5? PrisonerOptions.COMPLY: PrisonerOptions.DEFECT;
            }

            //only need to change when PeriodicComply => better result for this round
            if(periodChoice && currentRound % period == 0)
            {
                return PrisonerOptions.COMPLY;
            }

            
            // If only played against stupid opponents => comply
            if (OpponentChoices.All(x => x == PrisonerOptions.COMPLY) ||
                OpponentChoices.All(x => x == PrisonerOptions.DEFECT)
                || TitForTat)
            {
                return PrisonerOptions.COMPLY;
            }

            //check if TitForTat
            if (OpponentChoices.Skip(1).ToList().SequenceEqual(OwnChoices.SkipLast(1)))
            {
                TitForTat = true;
                return PrisonerOptions.COMPLY;
            };

            //check for periodDefect/periodComply
            PrisonerOptions po = OpponentChoices.First();
            int firstPeriod = OpponentChoices.Skip(1).TakeWhile(e => e == po).Count();
            int idxSecondPeriodStart = OpponentChoices.Skip(1).ToList().IndexOf(po);
            int secondPeriod = OpponentChoices.Skip(1 + idxSecondPeriodStart).TakeWhile(e => e == po).Count(); 

            if(firstPeriod == secondPeriod && po == PrisonerOptions.DEFECT)
            {
                periodChoice = true;
                period = secondPeriod+1;
            }

            //in all other cases
            return PrisonerOptions.DEFECT;
        }

        protected override void OnResult(Result result)
        {
            OpponentChoices.Add(calculator.GetOpponentChoiceForYears(LastChoice, result.Years));
            OwnChoices.Add(LastChoice);
        }
        protected override void GameEnded()
        {
            OpponentChoices.Clear();
        }
        protected override void GameStarted()
        {
            periodChoice = false;
            TitForTat = false;
            currentRound = 0;
        }
    }
}
