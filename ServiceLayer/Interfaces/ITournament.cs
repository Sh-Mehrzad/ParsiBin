using ParsiBin.DomainClasses;
using ParsiBin.ViewModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ITournament
    {                
        IList<TournamentModel> GetTournamnetList();
        IList<TournamentModel> GetDeletedList();
        IList<TournamentModel> GetDisabledList();
        void Add(TournamentModel viewModel);
        TournamentModel GetDetail(int ID);
        void Delete(int ID);
        void Edit(int ID, TournamentModel viewModel);
        bool IsExist(string Title);
        bool IsExist(string Title, int ID);       

    }
}
