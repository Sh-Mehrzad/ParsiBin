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
    public class StadiumService : Istadium
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Stadium> _Stadium;
        #endregion

        #region Constructor
        public StadiumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Stadium = _unitOfWork.Set<Stadium>();
        }
        #endregion

        #region List
        public IList<StadiumModel> GetItemList(int? CityID)
        {
            var TList = _Stadium.AsQueryable();
            IQueryable<Stadium> lst = TList.Include(x => x.City).OrderBy(x => x.Name);
                        
            if (CityID != null) lst.Where(x => x.CityID  == CityID.Value);

            return lst.Select(a => new StadiumModel
            {
                ID = a.ID,
                Name = a.Name,
                CityName = a.City.Name,
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                UpdatedOn  = a.UpdatedOn.ToString(),
                CreatedOn = a.CreatedOn.ToString()
            }).ToList();
        }

        public IList<StadiumModel> GetDeletedList()
        {
            var TList = _Stadium.AsQueryable();
            IQueryable<Stadium> lst = TList.Include(x => x.City).Where(x => x.IsDeleted == true).OrderBy(x => x.Name);
            return lst.Select(a => new StadiumModel
            {
                ID = a.ID,
                Name = a.Name,
                CityName = a.City.Name,
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                UpdatedOn = a.UpdatedOn.ToString(),
                CreatedOn = a.CreatedOn.ToString()
            }).ToList();
        }

        public IList<StadiumModel> GetDisabledList()
        {
            var TList = _Stadium.AsQueryable();
            IQueryable<Stadium> lst = TList.Include(x => x.City).Where(x => x.IsDeleted == false && x.IsEnabled == false).OrderBy(x => x.Name);
            return lst.Select(a => new StadiumModel
            {
                ID = a.ID,
                Name = a.Name,
                CityName = a.City.Name,
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                UpdatedOn = a.UpdatedOn.ToString(),
                CreatedOn = a.CreatedOn.ToString()
            }).ToList();
        }


        #endregion

        #region Check

        public bool IsExist(string Name)
        {
            return _Stadium.Any(x => x.Name == Name);
        }

        public bool IsExist(string Name, int id)
        {
            return _Stadium.Any(x => x.Name == Name);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(StadiumModel viewModel)
        {
            var lst = new Stadium
            {
                Name = viewModel.Name,
                City = viewModel.City,
                CityID = viewModel.CityID,
                IsDeleted = false,
                IsEnabled = true                
            };
            _Stadium.Add(lst);
        }

        public void Delete(int ID)
        {
            Stadium selectedlst = _Stadium.Remove(_Stadium.Find(ID));
        }

        public void Edit(int ID, StadiumModel viewModel)
        {
            Stadium selectedlst = _Stadium.Find(ID);
            selectedlst.City = viewModel.City;
            selectedlst.CityID = viewModel.CityID;
            selectedlst.Name = viewModel.Name;
            selectedlst.IsEnabled = viewModel.IsEnabled;
            selectedlst.UpdatedOn = DateTime.Now;
            }

        public StadiumModel GetDetail(int ID)
        {
            return _Stadium.Include(x => x.City).Where(x => x.ID == ID).Select(x =>
                new StadiumModel
                {
                    Name = x.Name,
                    CityName = x.City.Name,
                    CreatedOn = x.CreatedOn.ToString(),
                    UpdatedOn = x.UpdatedOn.ToString(),
                    IsEnabled = x.IsEnabled,
                    IsDeleted = x.IsDeleted                    
                }).FirstOrDefault();
        }

        #endregion
    }
}
