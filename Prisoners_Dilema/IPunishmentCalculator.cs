using Prisoners_Dilema.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_Dilema
{
    public interface IPunishmentCalculator
    {
        int GetPunishmentInYears(PrisonerOptions answer1, PrisonerOptions answer2);
    }
}
