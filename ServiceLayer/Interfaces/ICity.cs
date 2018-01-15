using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ICity
    {
        IList<CityModel> GetItemList(int? CountryID);
        //IList<CityModel> GetDeletedList();
        //IList<CityModel> GetDisabledList();
        void Add(CityModel viewModel);
        CityModel GetDetail(int ID);
        void Edit(int ID, CityModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);
    }
}
