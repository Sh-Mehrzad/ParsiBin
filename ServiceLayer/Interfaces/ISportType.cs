using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ISportType
    {
        void Add(SportTypeModel viewModel);
        IList<SportTypeModel> GetSportTypeList();
        void Delete(int ID);
        bool IsExist(string Title);
    }
}
