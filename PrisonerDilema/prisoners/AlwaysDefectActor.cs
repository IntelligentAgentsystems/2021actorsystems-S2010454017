using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
{
    public class AlwaysDefectActor : BasePrisoner
    {
        public AlwaysDefectActor():base()
        {

        }
        protected override PrisonerOptions GetAnswer()
        {
            return PrisonerOptions.DEFECT;
        }
    }
}
