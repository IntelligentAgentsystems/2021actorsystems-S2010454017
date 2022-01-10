using Prisoners_Dilema.messages;

namespace Prisoners_Dilema
{
    public class PunsimentCalculator : IPunishmentCalculator
    {
        public int GetPunishmentInYears(PrisonerOptions answer1, PrisonerOptions answer2)
        {
            if(answer1 == PrisonerOptions.COMPLY)
            {
                return answer2 == PrisonerOptions.COMPLY ? 4 : 7; 
            }

            return answer2 == PrisonerOptions.COMPLY ? 7 : 8;
        }
    }
}
