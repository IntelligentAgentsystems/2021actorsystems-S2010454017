using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
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
