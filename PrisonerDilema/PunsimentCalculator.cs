using PrisonersDilema.messages;

namespace PrisonersDilema
{
    public class PunsimentCalculator : IPunishmentCalculator
    {
        public int GetPunishmentInYears(PrisonerOptions answer1, PrisonerOptions answer2)
        {
            if (answer1 == PrisonerOptions.COMPLY)
            {
                return answer2 == PrisonerOptions.COMPLY ? 4 : 7;
            }

            return answer2 == PrisonerOptions.COMPLY ? 7 : 8;
        }

        public PrisonerOptions GetOpponentChoiceForYears(PrisonerOptions ownOptions, int years)
        {
            if (years == 7)
            {
                return ownOptions == PrisonerOptions.DEFECT ? PrisonerOptions.COMPLY : PrisonerOptions.DEFECT;
            }
            if (years == 4)
            {
                return PrisonerOptions.COMPLY;
            }
            return PrisonerOptions.DEFECT;
        }

        public (PrisonerOptions, PrisonerOptions) GetOptionsForYear(int year)
        {
            if (year == 7)
            {
                return (PrisonerOptions.COMPLY, PrisonerOptions.DEFECT);
            }
            if (year == 4)
            {
                return (PrisonerOptions.COMPLY, PrisonerOptions.COMPLY);
            }
            return (PrisonerOptions.DEFECT, PrisonerOptions.DEFECT);
        }
    }
}
