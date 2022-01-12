using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    public class PeriodicDefect : BasePrisoner
    {
        private readonly int defectPeriod = 10;
        private int CurrentCount { get; set; } = 0;

        public PeriodicDefect():base()
        {

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
    }
}
