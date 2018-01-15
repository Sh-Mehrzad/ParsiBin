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
    public class CityService : ICity
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<City> _City;
        #endregion

        #region Constructor
        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _City = _unitOfWork.Set<City>();
        }
        #endregion

        #region List
        public IList<CityModel> GetItemList(int? CountryID)
        {
            var TList = _City.AsQueryable();
            IQueryable<City> lst = TList.Include(x => x.Country).OrderBy(x => x.Name);
                        
            if (CountryID != null) lst.Where(x => x.Country.ID  == CountryID.Value);

            return lst.Select(a => new CityModel
            {
                ID = a.ID,
                Name = a.Name,
                CountryName = a.Country.Name                
            }).ToList();
        }


        #endregion

        #region Check

        public bool IsExist(string Name)
        {
            return _City.Any(x => x.Name == Name);
        }

        public bool IsExist(string Name, int id)
        {
            return _City.Any(x => x.Name == Name);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(CityModel viewModel)
        {
            var lst = new City
            {
                Name = viewModel.Name,
                CountryID = viewModel.CountryID,
                Country = viewModel.Country
            };
            _City.Add(lst);
        }

        public void Delete(int ID)
        {
            City selectedlst = _City.Remove(_City.Find(ID));
        }

        public void Edit(int ID, CityModel viewModel)
        {
            City selectedlst = _City.Find(ID);
            selectedlst.Country = viewModel.Country;
            selectedlst.Name = viewModel.Name;
        }

        public CityModel GetDetail(int ID)
        {
            return _City.Include(x => x.Country).Where(x => x.ID == ID).Select(x =>
                new CityModel
                {
                    Name = x.Name,
                    CountryName = x.Country.Name
                }).FirstOrDefault();
        }

        #endregion

    }
}
