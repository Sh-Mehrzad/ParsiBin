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
    public class ParticipantService : IParticipant
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;        
        private readonly IDbSet<Participant> _Participant;
        private readonly IDbSet<SportType> _sportType;
        private readonly IDbSet<ParticipantInGroups> _partInGroup;
        #endregion

        #region Constructor

        public ParticipantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Participant = _unitOfWork.Set<Participant>();
            _sportType = _unitOfWork.Set<SportType>();
            _partInGroup = _unitOfWork.Set<ParticipantInGroups>();
        }
        #endregion


        #region List
        public IList<ParticipantModel> GetItemList(int? SportTypeID, int? ParticipantTypeID, int? CountryID)
        {
            //var PING = _partInGroup.Include(x => x.participant);

            var TList = _Participant.AsQueryable();
            var lst = TList.Include(x => x.SportType);


            if (SportTypeID != null) {
                lst = lst.Where(x => x.SportTypeID == SportTypeID.Value);
            };

            if (ParticipantTypeID != null)
            {
                lst = lst.Where(x => x.ParticipantTypeID == ParticipantTypeID.Value);
            };

            if (CountryID != null)
            {
                lst = lst.Where(x => x.CountryID == CountryID);
            };

            //lst = lst.Where(x => x.Groups.Select(xy => xy.ID).FirstOrDefault() == GroupID);
            lst = lst.OrderBy(x => x.Name);
                        
            return lst.Select(a => new ParticipantModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                Logo = a.Logo,
                Name = a.Name,
                SportType = a.SportType,
                SportTypeTitle = a.SportType.Title,
                IsEnabled = false
            }).ToList();
        }

        public IList<ParticipantModel> GetItemList(int? SportTypeID)
        {
            var TList = _Participant.AsQueryable();
            IQueryable<Participant> lst = TList.Include(x => x.SportType).OrderBy(x => x.Name);

            if (SportTypeID != null)
            {
                lst.Where(x => x.SportTypeID == SportTypeID.Value);
            };

            return lst.Select(a => new ParticipantModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                Logo = a.Logo,
                Name = a.Name,
                SportType = a.SportType,
                SportTypeTitle = a.SportType.Title
            }).Take(10).ToList();
        }

        public IList<ParticipantModel> GetDeletedList()
        {
            var TList = _Participant.AsQueryable();
            IQueryable<Participant> lst = TList.Include(x => x.SportType).Where(x=>x.IsDeleted == true).OrderBy(x => x.Name);

            return lst.Select(a => new ParticipantModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                Logo = a.Logo,
                Name = a.Name,
                SportType = a.SportType,
                SportTypeTitle = a.SportType.Title
            }).ToList();
        }

        public IList<ParticipantModel> GetDisabledList()
        {
            var TList = _Participant.AsQueryable();
            IQueryable<Participant> lst = TList.Include(x => x.SportType).Where(x=>x.IsDeleted !=true && x.IsEnabled !=true).OrderBy(x => x.Name);

            return lst.Select(a => new ParticipantModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                Logo = a.Logo,
                Name = a.Name,
                SportType = a.SportType,
                SportTypeTitle = a.SportType.Title
            }).ToList();
        }


        #endregion

        #region Check

        public bool IsExist(string Name, int sportType, int CountryID)
        {
            return _Participant.Any(x => x.Name == Name && x.SportTypeID == sportType && x.CountryID == CountryID);
        }

        public bool IsExist(string Name, int id, int sportType, int CountryID)
        {
            return _Participant.Any(x => x.Name == Name && x.ID != id && x.SportTypeID == sportType && x.CountryID == CountryID);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(ParticipantModel viewModel)
        {
            var lst = new Participant
            {
                IsDeleted = false,
                IsEnabled = true,
                Logo = viewModel.Logo,
                Name = viewModel.Name,
                SportType = viewModel.SportType,
                SportTypeID = viewModel.SportTypeID,
                ParticipantTypeID = viewModel.ParticipantTypeID,
                CountryID = viewModel.CountryID
            };
            _Participant.Add(lst);
        }

        public void Delete(int ID)
        {            
            Participant selectedlst = _Participant.Find(ID);
            selectedlst.IsDeleted = true;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public void Edit(int ID, ParticipantModel viewModel)
        {
            Participant selectedlst = _Participant.Find(ID);
            SportType spt = _sportType.Find(viewModel.SportTypeID);
            selectedlst.IsDeleted = viewModel.IsDeleted;
            selectedlst.IsEnabled = viewModel.IsEnabled;
            if (viewModel.Logo != null)
            {
                selectedlst.Logo = viewModel.Logo;
            }            
            selectedlst.Name = viewModel.Name;
            selectedlst.SportType = spt;
            selectedlst.SportTypeID = viewModel.SportTypeID;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public ParticipantModel GetDetail(int ID)
        {
            return _Participant.Include(x => x.SportType).Where(x => x.ID == ID).Select(x =>
                new ParticipantModel
                {
                    CreatedOn = x.CreatedOn.ToString() ,
                    UpdatedOn = x.UpdatedOn.ToString(),
                    ID = x.ID,
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,
                    Logo = x.Logo,
                    Name = x.Name,
                    SportType = x.SportType,
                    SportTypeTitle = x.SportType.Title
                }).FirstOrDefault();
        }

        

        #endregion
    }
}
