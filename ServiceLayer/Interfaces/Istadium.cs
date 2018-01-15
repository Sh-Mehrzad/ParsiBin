using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface Istadium
    {
        IList<StadiumModel> GetItemList(int? CityID);
        IList<StadiumModel> GetDeletedList();
        IList<StadiumModel> GetDisabledList();
        void Add(StadiumModel viewModel);
        StadiumModel GetDetail(int ID);
        void Edit(int ID, StadiumModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);
    }
}
