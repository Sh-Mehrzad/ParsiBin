using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IScoreTitle
    {
        IList<ScoreTitleModel> GetItemList();
        //IList<ScoreTitleModel> GetDeletedList();
        //IList<ScoreTitleModel> GetDisabledList();
        void Add(ScoreTitleModel viewModel);
        ScoreTitleModel GetDetail(int ID);
        void Edit(int ID, ScoreTitleModel viewModel);
        void Delete(int ID);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);
    }
}
