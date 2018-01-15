using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.UserModel
{
    public class UserPredictionHistoryModel
    {
        public Guid MatchID { get; set; }
        public string GroupName { get; set; }
        public string _persianDate { get; set; }
        public Guid GroupID { get; set; }
        public DateTime MatchTime { get; set; }
        public string PersianDate
        {
            get
            {
                return _persianDate;
            }
            set
            {
                _persianDate = UtilityClass.GetTextDate(MatchTime);
            }
        }
        public string _persiantime { get; set; }
        public string PersianTime
        {
            get
            {
                return _persiantime;
            }
            set
            {
                _persiantime = UtilityClass.GetTextTime(MatchTime);
            }
        }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeTeamGoal { get; set; }
        public int AwayTeamGoal { get; set; }
        public double Score { get; set; }
        public string HomeLogo { get; set; }
        public string AwayLogo { get; set; }        
        public string UserPredictScoreTitle {get; set;}            
    }
}

