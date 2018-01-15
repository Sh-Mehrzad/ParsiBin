using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ICountry
    {
        IList<CountryModel> GetItemList();
        //IList<CountryModel> GetDeletedList();
        //IList<CountryModel> GetDisabledList();
        void Add(CountryModel viewModel);
        CountryModel GetDetail(int ID);
        void Edit(int ID, CountryModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);
    }
}
