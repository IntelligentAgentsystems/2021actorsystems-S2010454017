using Prisoners_Dilema;
using Prisoners_Dilema.messages;
using Xunit;
using Xunit.Sdk;

namespace Prisoners_DilemaTest
{
    public class TestCalculator
    {
       
        [Theory]
        [InlineData(PrisonerOptions.COMPLY, PrisonerOptions.COMPLY)]
        [InlineData(PrisonerOptions.DEFECT, PrisonerOptions.COMPLY)]
        [InlineData(PrisonerOptions.DEFECT, PrisonerOptions.DEFECT)]
        [InlineData(PrisonerOptions.COMPLY, PrisonerOptions.DEFECT)]
        public void TestGetYearsForAnswers(PrisonerOptions a1, PrisonerOptions a2)
        {
            IPunishmentCalculator calc = new PunsimentCalculator();
            var years = calc.GetPunishmentInYears(a1, a2);
            
            if(years == 4)
            {
                Assert.True(a1 == PrisonerOptions.COMPLY && a2 == PrisonerOptions.COMPLY);
                return;
            }
            if(years == 7)
            {
                Assert.True((a1 == PrisonerOptions.COMPLY && a2 == PrisonerOptions.DEFECT) ||
                            (a1 == PrisonerOptions.DEFECT && a2 == PrisonerOptions.COMPLY));
                return;
            }
            if (years == 8)
            {
                Assert.True(a1 == PrisonerOptions.DEFECT && a2 == PrisonerOptions.DEFECT);
                return;
            }
            throw new XunitException("Calculated Years not 4,7 or 8");



        }

        [Theory]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(7)]
        [InlineData(10)]

        public void TestGetAnswersForYears(int years)
        {
            IPunishmentCalculator calc = new PunsimentCalculator();
            var answers = calc.GetOptionsForYear(years);

            if(years == 4)
            {
                Assert.True(answers.Item1 == PrisonerOptions.COMPLY);
                Assert.True(answers.Item2 == PrisonerOptions.COMPLY);
                return;
            }

            if(years == 7)
            {
                Assert.True(answers.Item1 == PrisonerOptions.COMPLY);
                Assert.True(answers.Item2 == PrisonerOptions.DEFECT);
                return;
            }

            if(years == 8)
            {
                Assert.True(answers.Item1 == PrisonerOptions.DEFECT);
                Assert.True(answers.Item2 == PrisonerOptions.DEFECT);
                return;
            }

            Assert.True(answers.Item1 == PrisonerOptions.DEFECT);
            Assert.True(answers.Item2 == PrisonerOptions.DEFECT);
        }

        [Theory]
        [InlineData(PrisonerOptions.COMPLY, 4)]
        [InlineData(PrisonerOptions.DEFECT, 8)]
        [InlineData(PrisonerOptions.DEFECT, 7)]
        [InlineData(PrisonerOptions.COMPLY, 7)]
        [InlineData(PrisonerOptions.COMPLY, 10)]
        [InlineData(PrisonerOptions.COMPLY, -45)]
        public void TestGetOpponentAnswer(PrisonerOptions own, int years)
        {
            IPunishmentCalculator calc = new PunsimentCalculator();
            var answer = calc.GetOpponentChoiceForYears(own, years);

            switch (own)
            {
                case PrisonerOptions.COMPLY:
                    Assert.True(years == 4 ? answer == PrisonerOptions.COMPLY : answer == PrisonerOptions.DEFECT);
                    break;
                case PrisonerOptions.DEFECT:
                    Assert.True(years == 7 ? answer == PrisonerOptions.COMPLY : answer == PrisonerOptions.DEFECT);
                    break;
            }

        }

    }
}
