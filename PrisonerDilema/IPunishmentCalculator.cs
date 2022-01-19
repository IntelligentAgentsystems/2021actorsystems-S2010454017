using PrisonersDilema.messages;


namespace PrisonersDilema
{
    public interface IPunishmentCalculator
    {
        int GetPunishmentInYears(PrisonerOptions answer1, PrisonerOptions answer2);
        PrisonerOptions GetOpponentChoiceForYears(PrisonerOptions ownOptions, int years);
        (PrisonerOptions, PrisonerOptions) GetOptionsForYear(int year);
    }
}
