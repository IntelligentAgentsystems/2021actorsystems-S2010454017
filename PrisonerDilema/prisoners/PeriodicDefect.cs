using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
{
    public class PeriodicDefect : BasePrisoner
    {
        private readonly int defectPeriod;
        private int CurrentCount { get; set; }

        public PeriodicDefect(int period = 10):base()
        {
            defectPeriod = period;
            CurrentCount = 0;
        }

        protected override PrisonerOptions GetAnswer()
        {
            if (CurrentCount < defectPeriod)
            {
                ++CurrentCount;
                return PrisonerOptions.COMPLY;
            }
            else
            {
                CurrentCount = 0;
               return PrisonerOptions.DEFECT;
            }
        }

        protected override void GameStarted()
        {
            CurrentCount = 0;
        }
    }
}
