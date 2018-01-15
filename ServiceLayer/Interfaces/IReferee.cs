using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IReferee
    {
        IList<RefereeModel> GetItemList(int? SportTypeID, int? CountryID);
        //IList<RefereeModel> GetDeletedList();
        //IList<RefereeModel> GetDisabledList();
        void Add(RefereeModel viewModel);
        RefereeModel GetDetail(int ID);
        void Edit(int ID, RefereeModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);
    }
}
