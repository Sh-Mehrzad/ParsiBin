using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class ParticipantInMatchModel
    {
        public Guid MatchID { get; set; }
        public int HometeamID { get; set; }
        public int AwayTeamID { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
    }
}
