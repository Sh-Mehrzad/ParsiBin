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
    public class CountryService : ICountry
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Country> _Country;
        #endregion

        #region Constructor
        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Country = _unitOfWork.Set<Country>();
        }
        #endregion

        #region List
        public IList<CountryModel> GetItemList()
        {
            var TList = _Country.AsQueryable();
            IQueryable<Country> lst = TList.OrderBy(x=> x.Name);

            return lst.Select(a => new CountryModel
            {
                ID = a.ID,
                Name = a.Name,
                Flag = a.Flag
            }).ToList(); 
        }

        
        #endregion

        #region Check

        public bool IsExist(string Name)
        {
            return _Country.Any(x => x.Name == Name);
        }

        public bool IsExist(string Name, int id)
        {
            return _Country.Any(x => x.Name == Name);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(CountryModel viewModel)
        {            
            var lst = new Country
            {                
                Name = viewModel.Name,
                Flag = viewModel.Flag
            };
            _Country.Add(lst);
        }

        public void Delete(int ID)
        {
            Country selectedlst = _Country.Remove(_Country.Find(ID));            
        }

        public void Edit(int ID, CountryModel viewModel)
        {
            Country selectedlst = _Country.Find(ID);
            selectedlst.Name = viewModel.Name;
            if (viewModel.Flag != null)
            {
                selectedlst.Flag = viewModel.Flag;
            }            
        }

        public CountryModel GetDetail(int ID)
        {
            return _Country.Where(x => x.ID == ID).Select(x =>
                new CountryModel
                {
                    Name = x.Name,
                    Flag = x.Flag
                }).FirstOrDefault();
        }

        #endregion
    }
}
