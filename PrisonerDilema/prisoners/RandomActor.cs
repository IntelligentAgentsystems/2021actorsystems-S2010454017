using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
{
    public class RandomActor : BasePrisoner
    {
        private readonly Random rng;

        public RandomActor() : base()
        {
            rng = new Random();
        }

        protected override PrisonerOptions GetAnswer()
        {
            return rng.NextDouble() > .5 ? PrisonerOptions.COMPLY : PrisonerOptions.DEFECT;
        }
    }
}
