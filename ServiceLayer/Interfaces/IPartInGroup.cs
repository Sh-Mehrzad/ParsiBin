using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IPartInGroup
    {
        IList<PartInGroupsModel> GetItemList(Guid GroupID);
        IList<PartInGroupsModel> GetItemList(Guid GroupID, int? ParticipantTypeID, int? CountryID);
        IList<PartInGroupsModel> GetDeletedList(Guid GroupID);
        IList<PartInGroupsModel> GetDisabledList(Guid GroupID);
        void Add(PartInGroupsModel viewModel);
        void Add(int ParticipantID, Guid GroupID);
        void Remove(int ParticipantID, Guid GroupID);
        //PartInGroupsModel GetDetail(int ID);
        //void Edit(int ID, PartInGroupsModel viewModel);
        void Delete(int ID);
        bool IsExist(int ParticipantID, Guid GroupID);
        //bool IsExist(string Title, int ID);
    }
}
