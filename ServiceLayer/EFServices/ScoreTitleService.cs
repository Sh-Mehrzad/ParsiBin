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
    public class ScoreTitleService : IScoreTitle
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<ScoreTitle> _ScoreTitle;
        #endregion

        #region Constructor
        public ScoreTitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ScoreTitle = _unitOfWork.Set<ScoreTitle>();
        }
        #endregion

        #region List
        public IList<ScoreTitleModel> GetItemList()
        {
            var TList = _ScoreTitle.AsQueryable();
            IQueryable<ScoreTitle> ScoreTitle = TList.OrderBy(x => x.ID);

            return ScoreTitle.Select(a => new ScoreTitleModel
            {
                ID = a.ID,
                Description = a.Description,
                Title = a.Title
            }).ToList();
        }
               

        #endregion

        #region Check

        public bool IsExist(string Title)
        {
            return _ScoreTitle.Any(x => x.Title == Title);
        }

        public bool IsExist(string Title, int id)
        {
            return _ScoreTitle.Any(x => x.Title == Title && x.ID != id);
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(ScoreTitleModel viewModel)
        {            
            var ScoreTitle = new ScoreTitle
            {
                Description = viewModel.Description,
                Title = viewModel.Title
            };
            _ScoreTitle.Add(ScoreTitle);
        }

        public void Delete(int ID)
        {
            //ScoreTitle selectedScoreTitle = _ScoreTitle.Find(ID);            
        }

        public void Edit(int ID, ScoreTitleModel viewModel)
        {
            ScoreTitle selectedScoreTitle = _ScoreTitle.Find(ID);
            selectedScoreTitle.Description = viewModel.Description;
            selectedScoreTitle.Title = viewModel.Title;
        }

        public ScoreTitleModel GetDetail(int ID)
        {
            return _ScoreTitle.Where(x => x.ID == ID).Select(x =>
                new ScoreTitleModel
                {
                    ID = x.ID,
                    Title = x.Title,
                    Description = x.Description
                }).FirstOrDefault();
        }

        #endregion
    }
}
