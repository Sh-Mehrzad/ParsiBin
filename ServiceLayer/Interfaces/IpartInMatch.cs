using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IpartInMatch
    {
        //IList<PartInMatchModel> GetItemList();
        //IList<PartInMatchModel> GetDeletedList();
        //IList<PartInMatchModel> GetDisabledList();
        void Add(PartInMatchModel viewModel);
        //PartInMatchModel GetDetail(int ID);
        //void Edit(int ID, PartInMatchModel viewModel);
        //void Delete(int ID);
        //bool IsExist(string Title);
        //bool IsExist(string Title, int ID);
        int GetMatchHomeParticipant(Guid MatchID);
        int GetMatchAwayParticipant(Guid MatchID);
        void Delete(Guid MatchID);
    }
}
