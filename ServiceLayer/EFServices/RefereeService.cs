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
    public class RefereeService : IReferee
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Referee> _Referee;
        #endregion

        #region Constructor
        public RefereeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Referee = _unitOfWork.Set<Referee>();
        }
        #endregion

        #region List
        public IList<RefereeModel> GetItemList(int? SportTypeID, int? CountryID)
        {
            var TList = _Referee.AsQueryable();            
            IQueryable<Referee> lst = TList.Include(x => x.Country).Include(x=> x.SportType).OrderBy(x=> x.name);

            if (SportTypeID != null) lst.Where(x => x.SportTypeID == SportTypeID.Value);
            if (CountryID != null) lst.Where(x => x.CountryID == CountryID.Value);

            return lst.Select(a => new RefereeModel
            {
                ID = a.ID,
                name = a.name,
                CountryName = a.Country.Name,
                SportTypeTitle = a.SportType.Title
            }).ToList(); 
        }

        
        #endregion

        #region Check

        public bool IsExist(string Name)
        {
            return _Referee.Any(x => x.name == Name);
        }

        public bool IsExist(string Name, int id)
        {
            return _Referee.Any(x => x.name == Name);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(RefereeModel viewModel)
        {            
            var lst = new Referee
            {                
                name = viewModel.name,
                SportTypeID = viewModel.SportTypeID,
                CountryID = viewModel.CountryID,
                Country = viewModel.Country,
                SportType = viewModel.SportType
            };
            _Referee.Add(lst);
        }

        public void Delete(int ID)
        {
            Referee selectedlst = _Referee.Remove(_Referee.Find(ID));            
        }

        public void Edit(int ID, RefereeModel viewModel)
        {
            Referee selectedlst = _Referee.Find(ID);
            selectedlst.CountryID = viewModel.CountryID;
            selectedlst.Country = viewModel.Country;
            selectedlst.name = viewModel.name;
            selectedlst.SportType = viewModel.SportType;
            selectedlst.SportTypeID = viewModel.SportTypeID;            
        }

        public RefereeModel GetDetail(int ID)
        {
            return _Referee.Include(x=> x.Country).Include(x=> x.SportType).Where(x => x.ID == ID).Select(x =>
                new RefereeModel
                {
                    Country = x.Country,
                    CountryID = x.CountryID,
                    CountryName = x.Country.Name,
                    name = x.name,
                    SportType = x.SportType,
                    SportTypeID = x.SportTypeID,
                    SportTypeTitle = x.SportType.Title
                }).FirstOrDefault();
        }

        #endregion
    }
}
