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
    public class MatchScoreService : IMatchScore
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<MatchScore> _MatchScore;
        #endregion

        #region Constructor
        public MatchScoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _MatchScore = _unitOfWork.Set<MatchScore>();
        }
        #endregion

        #region List
        public IList<MatchScoreModel> GetItemList(Guid MatchID)
        {
            var TList = _MatchScore.AsQueryable();
            IQueryable<MatchScore> MatchScore = TList.Include(x=> x.ScoreTitle).Where(x=> x.MatchID == MatchID && x.IsDeleted !=true && x.IsEnabled == true).OrderBy(x => x.ScoreTitle.ID);

            return MatchScore.Select(a => new MatchScoreModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                UpdatedOn = a.UpdatedOn.ToString(),
                MatchID = a.Match.ID,
                MatchTitle = a.Match.MatchTime.ToString(),
                Score = a.Score,
                ScoreTitleID = a.ScoreTitleID,
                ScoreTitle = a.ScoreTitle.Title
            }).ToList();
        }


        #endregion

        #region Check

        public bool IsExist(int ScoreTitleID)
        {
            return _MatchScore.Any(x => x.ScoreTitleID == ScoreTitleID);
        }

        public int IsExist(int ScoreTitleID, Guid MatchID)
        {
            //var r = _MatchScore.Where(x => x.ScoreTitleID == ScoreTitleID && x.MatchID == MatchID && x.Score == Score);
            var result = _MatchScore.Where(x => x.ScoreTitleID == ScoreTitleID && x.MatchID == MatchID && x.Match.IsEnabled== true).Select(x=>x.ID).FirstOrDefault();
            if (result != 0)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Add/Delete/Edit/View

        public void Add(List<MatchScoreModel> viewModel)
        {
            foreach (var MSModel in viewModel)
            {
                var MatchScore = new MatchScore
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    MatchID = MSModel.MatchID,
                    Score = MSModel.Score,
                    ScoreTitleID = MSModel.ScoreTitleID
                };
                _MatchScore.Add(MatchScore);
            }
        }

        public void Delete(int ID)
        {
            MatchScore selectedMatchScore = _MatchScore.Find(ID);
            selectedMatchScore.IsDeleted = true;
        }

        public void Delete(Guid MatchID)
        {
            var lst = _MatchScore.Where(x => x.MatchID == MatchID).ToList();
            foreach(var itm in lst)
            {
                itm.IsDeleted = true;
            }
        }

        public void Edit(int ID, MatchScoreModel viewModel)
        {
            MatchScore selectedMatchScore = _MatchScore.Find(ID);
            selectedMatchScore.IsDeleted = viewModel.IsDeleted;
            selectedMatchScore.IsEnabled = viewModel.IsEnabled;
            selectedMatchScore.Score = viewModel.Score;
            selectedMatchScore.ScoreTitleID = viewModel.ScoreTitleID;
        }

        public MatchScoreModel GetDetail(int ID)
        {
            return _MatchScore.Where(x => x.ID == ID).Select(x =>
                new MatchScoreModel
                {
                    ID = x.ID,
                    CreatedOn = x.CreatedOn.ToString(),
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,
                    UpdatedOn = x.UpdatedOn.ToString(),
                    Score= x.Score
                }).FirstOrDefault();
        }

        

        public double MatchScore(Guid MatchID, int ScoreTitleID)
        {
            return _MatchScore.Where(x => x.ScoreTitleID == ScoreTitleID && x.MatchID == MatchID).Select(x => x.Score).FirstOrDefault();
            
        }

        

        #endregion
    }
}
