using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IParticipant
    {
        IList<ParticipantModel> GetItemList(int? SportTypeID, int? ParticipantTypeID, int? CountryID);
        IList<ParticipantModel> GetItemList(int? SportTypeID);
        IList<ParticipantModel> GetDeletedList();
        IList<ParticipantModel> GetDisabledList();
        void Add(ParticipantModel viewModel);
        ParticipantModel GetDetail(int ID);
        void Edit(int ID, ParticipantModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title, int sportType, int CountryID);
        bool IsExist(string Title, int ID, int sportType, int CountryID);
    }
}
