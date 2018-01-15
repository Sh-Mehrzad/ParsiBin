using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Interfaces;
using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using System.Data.Entity;
using ParsiBin.ViewModel.AdminModel;
using System.Data;
using ParsiBin.DomainClasses.Entities;

namespace ServiceLayer.EFServices
{
    public class TournamentService : ITournament
    {

        #region Fields

        private readonly IUnitOfWork _unitOfWork;        
        private readonly IDbSet<Tournament> _Tournament;
        private readonly IDbSet<Match> _Match;
        private readonly IDbSet<Group> _Group;            
        #endregion

        #region Constructor

        public TournamentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Tournament = _unitOfWork.Set<Tournament>();
            _Match = _unitOfWork.Set<Match>();
            _Group = _unitOfWork.Set<Group>();
        }
        #endregion


        public void Add(TournamentModel tournament)
        {
            //if (ExisByName(viewModel.Name, viewModel.CategoryId)) return false;
            var tournamnet = new Tournament
            {
                Title = tournament.Title,
                Logo = tournament.Logo,
                IsEnabled = true
            };
            _Tournament.Add(tournamnet);
        }

        public int GetMatches(int id)
        {
            return _Match.Select(x => x.Group.TournamentID == id ).Count();
        }

        public IList<TournamentModel> GetTournamnetList()
        {
            var TList = _Tournament.AsQueryable();
            //var MList = _Match.AsQueryable();
            //var lst = (from M in MList join T in TList on M.Group.TournamentID equals T.ID select new { T.Title, T.CreatedOn, T.UpdatedOn, T.Logo, })
            IQueryable<Tournament> tournament = TList.Where(t => t.IsDeleted != true && t.IsEnabled == true);
            List<TournamentModel> TModel = new List<TournamentModel>();

            foreach (var item in tournament)
            {
                var Group = (from G in _Group where G.TournamentID == item.ID && G.IsEnabled == true && G.IsDeleted != true select G.IsEnabled).Count();
                var ActiveMatch = (from M in _Match where M.Group.TournamentID == item.ID && M.IsEnabled == true && M.IsDeleted != true && M.MatchStatus == 1 && M.MatchTime > DateTime.Now select M.IsEnabled).Count();
                var InProgress = (from M in _Match where M.Group.TournamentID == item.ID && M.IsEnabled == true && M.IsDeleted != true && M.MatchStatus == 1 && M.MatchTime <= DateTime.Now select M.IsEnabled).Count();
                TModel.Add(new TournamentModel()
                {
                    Title = item.Title,
                    CreatedOn = item.CreatedOn.ToString(),
                    Logo = item.Logo,
                    UpdatedOn = item.UpdatedOn.ToString(),
                    ID = item.ID,
                    GroupCount = Group,
                    ActiveMatch = ActiveMatch,
                    InProgressMatch = InProgress
                });
            }
            return TModel;

            
        }
        
        public IList<TournamentModel> GetDisabledList()
        {
            var TList = _Tournament.AsQueryable();
            IQueryable<Tournament> tournament = TList.Where(t => t.IsEnabled != true);
            return tournament.Select(t => new TournamentModel
                {
                    Title = t.Title,
                    CreatedOn = t.CreatedOn.ToString(),
                    Logo = t.Logo,
                    UpdatedOn = t.UpdatedOn.ToString(),
                    ID = t.ID
                }).ToList();
        }

        public IList<TournamentModel> GetDeletedList()
        {
            var TList = _Tournament.AsQueryable();
            IQueryable<Tournament> tournament = TList.Where(t => t.IsDeleted == true);
            return tournament.Select(t => new TournamentModel
            {
                Title = t.Title,
                CreatedOn = t.CreatedOn.ToString(),
                Logo = t.Logo,
                UpdatedOn = t.UpdatedOn.ToString(),
                ID = t.ID
            }).ToList();
        }

        public TournamentModel GetDetail(int ID)
        {
            return _Tournament.Where(x => x.ID == ID).Select(x =>
                new TournamentModel
                {
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,
                    Logo = x.Logo,
                    Title = x.Title
                }).FirstOrDefault();
        }

        public void Delete(int ID)
        {
            Tournament selectedTournament = _Tournament.Find(ID);
            selectedTournament.IsDeleted = true;
            selectedTournament.UpdatedOn = DateTime.Now;
        }

        public void Edit(int ID, TournamentModel tournament)
        {
            Tournament selectedTournament = _Tournament.Find(ID);
            selectedTournament.Title = tournament.Title;
            selectedTournament.Logo = tournament.Logo;
            selectedTournament.IsDeleted = tournament.IsDeleted;
            selectedTournament.IsEnabled = tournament.IsEnabled;
            selectedTournament.UpdatedOn = DateTime.Now;
        }

        public bool IsExist(string Name)
        {
            return _Tournament.Any(x => x.Title == Name && x.IsEnabled == true && x.IsDeleted == false);            
        }

        public bool IsExist(string Name, int id)
        {
            return _Tournament.Any(x => x.Title == Name && x.ID != id && x.IsEnabled == true && x.IsDeleted == false);
        }
    }
}
