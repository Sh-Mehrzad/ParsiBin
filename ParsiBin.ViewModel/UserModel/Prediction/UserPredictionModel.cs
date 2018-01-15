using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.UserModel
{
    public class UserPredictionModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid MatchID { get; set; }
        public int ScoreTitleID { get; set; }
        public Double Score { get; set; }
        public Double Point { get; set; }
        public DateTime UserPrediction { get; set; }
        public int TournamentID { get; set; }
    }
}
