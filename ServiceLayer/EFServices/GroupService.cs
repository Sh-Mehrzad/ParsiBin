using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using ParsiBin.DomainClasses.Entities;
using ParsiBin.ViewModel.AdminModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.EFServices
{
    public class GroupService : IGroup
    {

        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Group> _Group;
        private readonly IDbSet<Match> _Match;
        #endregion

        #region Constructor
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Group = _unitOfWork.Set<Group>();
            _Match = _unitOfWork.Set<Match>();
        }
        #endregion

        #region List
        public IList<GroupModel> GetItemList(int TournamentID)
        {
            var TList = _Group.AsQueryable();
            IQueryable<Group> group = TList.Include(x=> x.SportType).Where(t => t.IsDeleted != true && t.IsEnabled == true && t.TournamentID == TournamentID).OrderBy(x=> x.Title);
            List<GroupModel> GModel = new List<GroupModel>();

            foreach(var item in group)
            {
                var FinishMatches = (from M in _Match where M.GroupID == item.ID && M.IsEnabled == true && M.IsDeleted != true && M.MatchStatus == 3 select M.IsEnabled).Count();
                var ActiveMatch = (from M in _Match where M.GroupID == item.ID && M.IsEnabled == true && M.IsDeleted != true && M.MatchStatus == 1 && M.MatchTime > DateTime.Now select M.IsEnabled).Count();
                var InProgress = (from M in _Match where M.GroupID == item.ID && M.IsEnabled == true && M.IsDeleted != true && M.MatchStatus == 1 && M.MatchTime <= DateTime.Now select M.IsEnabled).Count();

                GModel.Add(new GroupModel()
                {
                    Title = item.Title,
                    CreatedOn = item.CreatedOn.ToString(),
                    UpdatedOn = item.UpdatedOn.ToString(),
                    ID = item.ID,
                    TournamentID = item.TournamentID,
                    SportTypeID = item.SportTypeID,
                    SportTypeTitle = item.SportType.Title,
                    ParticipantTypeID = item.ParticipantTypeID.Value,
                    InProgressMatch = InProgress,
                    FinishMatch = FinishMatches,
                    ActiveMatch = ActiveMatch
                });
            }
            return GModel;
        }

        public IList<GroupModel> GetDeletedList(int TournamentID)
        {
            var TList = _Group.AsQueryable();
            IQueryable<Group> group = TList.Where(t => t.IsDeleted == true && t.TournamentID == TournamentID);
            return group.Select(a => new GroupModel {
                Title = a.Title,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ID = a.ID,
                TournamentID = a.TournamentID,
                SportTypeID = a.SportTypeID,
                ParticipantTypeID = a.ParticipantTypeID.Value
            }).ToList();
        }

        public IList<GroupModel> GetDisabledList(int TournamentID)
        {
            var TList = _Group.AsQueryable();
            IQueryable<Group> group = TList.Where(t => t.IsDeleted != true && t.IsEnabled == false && t.TournamentID == TournamentID);
            return group.Select(a => new GroupModel
            {
                Title = a.Title,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ID = a.ID,
                TournamentID = a.TournamentID,
                SportTypeID = a.SportTypeID,
                ParticipantTypeID = a.ParticipantTypeID.Value
            }).ToList();
        }

        public IList<GroupModel> GetParticipantTypeList()
        {
            List<GroupModel> PartTypeList = new List<GroupModel>()
            {
                new GroupModel() { ParticipantTypeID = 1 , ParticipantType = "ملی" },
                new GroupModel() { ParticipantTypeID = 2 , ParticipantType = "باشگاهی" }
            };
            return PartTypeList.ToList();
        }
        #endregion

        #region Check

        public bool IsExist(string Name, int TournamentID, int SportTypeID)
        {
            return _Group.Any(x => x.Title == Name && x.IsEnabled == true && x.IsDeleted == false && x.TournamentID == TournamentID && x.SportTypeID == SportTypeID);
        }

        public bool IsExist(string Name, Guid id, int TournamentID, int SportTypeID)
        {
            return _Group.Any(x => x.Title == Name && x.ID != id && x.IsEnabled == true && x.IsDeleted == false && x.TournamentID == TournamentID && x.SportTypeID == SportTypeID);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(GroupModel viewModel, int tournamentID)
        {
            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            var group = new Group
            {
                ID = g,
                IsEnabled = true,
                SportTypeID = viewModel.SportTypeID,
                Title = viewModel.Title,
                TournamentID = tournamentID,
                tournament = viewModel.Tournament,
                ParticipantTypeID = viewModel.ParticipantTypeID,
                CountryID = viewModel.CountryID
            };
            _Group.Add(group);
        }

        public void Delete(Guid ID)
        {
            Group selectedGroup = _Group.Find(ID);
            selectedGroup.IsDeleted = true;
            selectedGroup.UpdatedOn = DateTime.Now;
        }

        public void Edit(Guid ID, GroupModel viewModel)
        {
            Group selectedGroup = _Group.Find(ID);
            selectedGroup.Title = viewModel.Title;            
            selectedGroup.IsDeleted = viewModel.IsDeleted;
            selectedGroup.IsEnabled = viewModel.IsEnabled;
            selectedGroup.UpdatedOn = DateTime.Now;
            selectedGroup.SportTypeID = viewModel.SportTypeID;
            selectedGroup.ParticipantTypeID = viewModel.ParticipantTypeID;
            selectedGroup.CountryID = viewModel.CountryID;         
        }

        public GroupModel GetDetail(Guid ID)
        {
            return _Group.Where(x => x.ID == ID).Select(x =>
                new GroupModel
                {
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,                    
                    Title = x.Title,
                    SportTypeID = x.SportTypeID,
                    TournamentTitle = x.tournament.Title,
                    TournamentID = x.TournamentID,
                    ParticipantTypeID = x.ParticipantTypeID.Value,
                    CountryID = x.CountryID.Value
                }).FirstOrDefault();
        }

        public int ActiveGroupCount(int TournamentID)
        {
            return _Group.Count(x => x.TournamentID == TournamentID && x.IsEnabled == true && x.IsDeleted != true);
        }

        #endregion


    }
}
