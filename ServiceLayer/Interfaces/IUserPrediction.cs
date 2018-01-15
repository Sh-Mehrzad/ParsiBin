using ParsiBin.ViewModel.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IUserPrediction
    {
        void Add(UserPredictionModel viewModel);
        int IsExist(Guid MatchID, int UserID);
        void Delete(int ID);
        bool TimeCheck(DateTime PredicTime, Guid MatchID);
        void UpdatePoint(IList<int> WinnerList, Guid MatchID);
        double GetUserPoint(int UserID, int TournamentID);
        void SetUserRanking(int TournamentID);
        string GetUserRanking(int UserID, int TournamentID);
        void GetPredictLog(Guid MatchID, int UserID, string ScoreTitleID, string Score, bool Status);
        IList<UserRankingModel> TopTen(int TournamentID);
    }
}
