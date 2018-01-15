using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.UserModel.Prediction
{
    public class ComingMatchModel
    {
        public int ID { get; set; }
        public string TournamentTitle { get; set; }
        public string TournamentLogo { get; set; }
        public IList<MatchModel> MatchList { get; set; }
    }
}
