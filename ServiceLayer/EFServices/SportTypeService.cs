using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
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
    public class SportTypeService : ISportType
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;        
        private readonly IDbSet<SportType> _SportType;
        #endregion

        #region Constructor

        public SportTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _SportType = _unitOfWork.Set<SportType>();            
        }
        #endregion

        public void Add(SportTypeModel sporttype)
        {
            var sportT = new SportType
            {
                Title = sporttype.Title,
                IsEnabled = true
            };
            _SportType.Add(sportT);
        }

        public IList<SportTypeModel> GetSportTypeList()
        {
            var list = _SportType.AsQueryable();
            IQueryable<SportType> sportT = list.Where(x => x.IsDeleted != true);
            return sportT.Select(x => new SportTypeModel
                {
                    Title = x.Title,
                    CreatedOn = x.CreatedOn.ToString(),
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,
                    ID = x.ID,
                    UpdatedOn = x.UpdatedOn.ToString()
                }).ToList();
        }

        public void Delete(int ID)
        {
            SportType selectedItem = _SportType.Find(ID);
            selectedItem.IsDeleted = true;
            selectedItem.UpdatedOn = DateTime.Now;
        }

        public bool IsExist(string Name)
        {
            return _SportType.Any(x => x.Title == Name && x.IsEnabled == true && x.IsDeleted == false);
        }
    }
}
