using Prisoners_Dilema.messages;

namespace Prisoners_Dilema.prisoners
{
    public class AlwaysComplyActor : BasePrisoner
    {
        public AlwaysComplyActor():base()
        {
         
        }

        protected override PrisonerOptions GetAnswer()
        {
            return PrisonerOptions.COMPLY;
        }
    }
}
