using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    public class TitForTatActor : BasePrisoner
    {
        private PrisonerOptions lastOpponentOption;

        public TitForTatActor():base()
        {
            lastOpponentOption = PrisonerOptions.COMPLY;
        }

        protected override void OnResult(Result result)
        {   
            lastOpponentOption = calculator.GetOpponentChoiceForYears(LastChoice, result.Years);
        }

        protected override PrisonerOptions GetAnswer()
        {
            if (FirstMove)
            {
                return PrisonerOptions.COMPLY;
            }
            return lastOpponentOption;
        }
    }
}
