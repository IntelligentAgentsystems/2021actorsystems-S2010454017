using PrisonersDilema.messages;

namespace PrisonersDilema.prisoners
{
    public class StatisticLearningActor : BasePrisoner
    {
        private long countComply;
        private long countDefect;

        public StatisticLearningActor() : base()
        {
            countDefect = 0;
            countComply = 0;
        }

        protected override PrisonerOptions GetAnswer()
        {
            if (countDefect == countComply)
            {
                return PrisonerOptions.COMPLY;
            }

            return countComply > countDefect ? PrisonerOptions.COMPLY : PrisonerOptions.DEFECT;

        }

        protected override void OnResult(Result result)
        {
            var answers = calculator.GetOptionsForYear(result.Years);
            if (answers.Item1 == PrisonerOptions.COMPLY)
            {
                if (answers.Item2 == PrisonerOptions.COMPLY)
                {
                    countComply += 2;
                }
                else
                {
                    ++countDefect;
                    ++countComply;
                }
            }
            else
            {
                ++countDefect;
                if (answers.Item2 == PrisonerOptions.COMPLY)
                {
                    ++countComply;
                }
                else
                {
                    ++countDefect;
                }
            }
        }
    }
}
