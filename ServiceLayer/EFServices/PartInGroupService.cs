using ParsiBin.DataLayer;
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
    public class PartInGroupService : IPartInGroup
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;        
        private readonly IDbSet<ParticipantInGroups> _partInGroups;
        private readonly IDbSet<Participant> _participant;
        #endregion

        #region Constructor

        public PartInGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _partInGroups = _unitOfWork.Set<ParticipantInGroups>();
            _participant = _unitOfWork.Set<Participant>();
        }
        #endregion

        #region List
        public IList<PartInGroupsModel> GetItemList(Guid GroupID)
        {
            var TList = _partInGroups.AsQueryable();
            IQueryable<ParticipantInGroups> lst = TList.Include(x => x.participant).Where(x=>x.IsDeleted != true && x.IsEnabled == true && x.GroupID == GroupID).OrderBy(x => x.participant.Name);
                        
            return lst.Select(a => new PartInGroupsModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ParticipantID = a.participant.ID,
                Team = a.participant.Name
            }).ToList();
        }

        public IList<PartInGroupsModel> GetItemList(Guid GroupID, int? ParticipantTypeID, int? CountryID)
        {
            var TList = _partInGroups.AsQueryable();
            IQueryable<ParticipantInGroups> lst = TList.Include(x => x.participant).Where(x => x.IsDeleted != true && x.IsEnabled == true && x.GroupID == GroupID).OrderBy(x => x.participant.Name);
            if (ParticipantTypeID != null)
            {
                lst.Where(x => x.participant.ParticipantTypeID == ParticipantTypeID.Value);
            }

            if (CountryID != null)
            {
                lst.Where(x => x.participant.CountryID == CountryID);
            }

            return lst.Select(a => new PartInGroupsModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ParticipantID = a.participant.ID,
                Team = a.participant.Name
            }).ToList();
        }

        public IList<PartInGroupsModel> GetDeletedList(Guid GroupID)
        {
            var TList = _partInGroups.AsQueryable();
            IQueryable<ParticipantInGroups> lst = TList.Include(x => x.participant).Where(x => x.IsDeleted == true && x.GroupID == GroupID).OrderBy(x => x.participant.Name);

            return lst.Select(a => new PartInGroupsModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ParticipantID = a.ParticipantID,
                Team = a.participant.Name
            }).ToList();
        }

        public IList<PartInGroupsModel> GetDisabledList(Guid GroupID)
        {
            var TList = _partInGroups.AsQueryable();
            IQueryable<ParticipantInGroups> lst = TList.Include(x => x.participant).Where(x => x.IsDeleted == true && x.GroupID == GroupID).OrderBy(x => x.participant.Name);

            return lst.Select(a => new PartInGroupsModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                UpdatedOn = a.UpdatedOn.ToString(),
                ParticipantID = a.ParticipantID,
                Team = a.participant.Name
            }).ToList();
        }


        #endregion

        #region Check

        public bool IsExist(int ParticipantID, Guid GroupID)
        {
            return _partInGroups.Any(x => x.ParticipantID == ParticipantID && x.GroupID == GroupID);
        }
                

        #endregion

        #region Add/Delete/Edit/View

        public void Add(PartInGroupsModel viewModel)
        {
            var lst = new ParticipantInGroups
            {
                IsDeleted = false,
                IsEnabled = true,
                GroupID = viewModel.GroupID,
                participant = _participant.Find(viewModel.ParticipantID),
                ParticipantID = viewModel.ParticipantID 
            };
            _partInGroups.Add(lst);
        }

        public void Delete(int ID)
        {            
            _partInGroups.Remove(_partInGroups.Find(ID));
        }

        public void Add(int ParticipantID, Guid GroupID)
        {
            var lst = new ParticipantInGroups
            {
                IsDeleted = false,
                IsEnabled = true,
                GroupID = GroupID,
                participant = _participant.Find(ParticipantID),
                ParticipantID = ParticipantID
            };
            _partInGroups.Add(lst);
        }

        public void Remove(int ParticipantID, Guid GroupID)
        {
            _partInGroups.Remove(_partInGroups.Where(x => x.GroupID == GroupID && x.ParticipantID == ParticipantID).FirstOrDefault());
        }




        #endregion
    }
}
