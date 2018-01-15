using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IGroup
    {
        IList<GroupModel> GetItemList(int TournamentID);
        IList<GroupModel> GetDeletedList(int TournamentID);
        IList<GroupModel> GetDisabledList(int TournamentID);
        IList<GroupModel> GetParticipantTypeList();
        void Add(GroupModel viewModel, int TournamentID);
        GroupModel GetDetail(Guid ID);
        void Edit(Guid ID, GroupModel viewModel);
        void Delete(Guid ID);
        bool IsExist(string Title, int TournamentID, int SportTypeID);
        bool IsExist(string Title, Guid ID, int TournamentID, int SportTypeID);
        int ActiveGroupCount(int TournamentID);
    }
}
