using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
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
