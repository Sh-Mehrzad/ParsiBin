using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IMatchScore
    {
        IList<MatchScoreModel> GetItemList(Guid MatchID);
        //IList<MatchScoreModel> GetDeletedList();
        //IList<MatchScoreModel> GetDisabledList();
        void Add(List<MatchScoreModel> viewModel);
        MatchScoreModel GetDetail(int ID);
        void Edit(int ID, MatchScoreModel viewModel);
        void Delete(int ID);
        void Delete(Guid MatchID);
        bool IsExist(int ScoreTitleID);
        int IsExist(int ScoreTitleID, Guid MatchID);
        double MatchScore(Guid MatchID, int ScoreTitleID);

    }
}
