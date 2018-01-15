using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.UserModel
{
    public class UserPointStatusModel
    {
        public int TournamentID { get; set; }
        public string Tournament { get; set; }
        public double Point { get; set; }
        public string Rank { get; set; }
    }
}
