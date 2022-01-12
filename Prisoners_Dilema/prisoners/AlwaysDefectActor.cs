using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
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
