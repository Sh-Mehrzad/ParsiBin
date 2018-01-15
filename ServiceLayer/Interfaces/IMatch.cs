using ParsiBin.ViewModel.AdminModel;
using ParsiBin.ViewModel.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IMatch
    {
        IList<MatchModel> GetItemList(byte? MatchStatus, Guid? GroupID);
        IList<MatchModel> GetItemListUserView(int UserID, int TournamentID);
        IList<MatchModel> GetItemListUserView2(int AddDays, int TournamentID);
        IList<LastMatchesModel> GetLastMatches(int Number);
        IList<LastMatchesModel> GetLastMatches(int TournamentID, int Number);
        IList<FutureMatchesModel> GetFutureMatches(int UserID);
        IList<UserPredictionHistoryModel> GetUserHistoryListView(int UserID, int TournamentID, int Take);
        IList<MatchModel> GetDeletedList();
        IList<MatchModel> GetDisabledList();
        Guid Add(MatchModel viewModel);
        void AddMatchResult(MatchModel viewModel);
        bool MathResultExist(Guid MatchID);
        MatchModel GetDetail(Guid ID);
        MatchModel GetMatchGoals(Guid MatchID);
        void Edit(Guid ID, MatchModel viewModel);
        void Delete(Guid ID);
        void ChangeMatchStatus(Guid MatchID);
        void GetEnable(Guid ID);
        int GetCountFutureMatches();
    }
}
