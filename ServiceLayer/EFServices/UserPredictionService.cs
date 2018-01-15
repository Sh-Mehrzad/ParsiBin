using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsiBin.ViewModel.UserModel;
using ParsiBin.DataLayer;
using System.Data.Entity;
using ParsiBin.DomainClasses.Entities;
using ParsiBin.DomainClasses;

namespace ServiceLayer.EFServices
{
    public class UserPredictionService : IUserPrediction
    {

        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<UserPrediction> _userPrediction;
        private readonly IDbSet<Match> _Match;
        private readonly IDbSet<UserRank> _rank;
        private readonly IDbSet<UserPredictLog> _UserPredictLog;
        #endregion

        #region Constructor
        public UserPredictionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userPrediction = _unitOfWork.Set<UserPrediction>();
            _Match = _unitOfWork.Set<Match>();
            _rank = _unitOfWork.Set<UserRank>();
            _UserPredictLog = _unitOfWork.Set<UserPredictLog>();
        }
        #endregion

        public void Add(UserPredictionModel viewModel)
        {
            var lst = new UserPrediction
            {
                MatchID = viewModel.MatchID,
                Score = viewModel.Score,
                ScoreTitleID = viewModel.ScoreTitleID,
                PUserID = viewModel.UserID,
                Point = 0,
                PredictionTime = DateTime.Now,
                TournamentID = viewModel.TournamentID
            };
            _userPrediction.Add(lst);
        }

        public void Delete(int ID)
        {
            //_userPrediction.Attach(_userPrediction.Find(ID));
            _userPrediction.Remove(_userPrediction.Find(ID));
        }

        public int IsExist(Guid MatchID, int UserID)
        {
            return _userPrediction.Where(x => x.MatchID == MatchID && x.PUserID == UserID).Select(x => x.ID).FirstOrDefault();
        }

        public bool TimeCheck(DateTime PredicTime, Guid MatchID)
        {
            DateTime MatchTime = _Match.Where(x => x.ID == MatchID).Select(x => x.MatchTime).FirstOrDefault();
            if (PredicTime > MatchTime)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdatePoint(IList<int> WinnerList, Guid MatchID)
        {
            var lst = _userPrediction.Where(x => WinnerList.Contains(x.ScoreTitleID) && x.MatchID == MatchID).ToList();
            lst.ForEach(x => x.Point = x.Score);
            _unitOfWork.SaveChanges();
        }

        public double GetUserPoint(int UserID, int TournamentID)
        {
            var sumPoint = (from i in _userPrediction
                            where i.PUserID == UserID && i.TournamentID == TournamentID
                            select i.Point).DefaultIfEmpty(0).Sum();            
            return  sumPoint;
        }

        public void SetUserRanking(int TournamentID)
        {
            var usrpredict = from x in _userPrediction
                             where x.TournamentID == TournamentID
                             group x by new { x.PUserID } into g
                             select new
                             {
                                 UserID = g.Key.PUserID,
                                 Point = g.Sum(x => x.Point)
                             };
            //var z = ;
            //List<UserRank> rankList = new List<UserRank>();
            int Row = 1;
            DateTime rankDate = DateTime.Now;
            foreach ( var ur in usrpredict.OrderByDescending(x => x.Point))
            {                
                var item = new UserRank
                {
                    Rank = Row,
                    RankDate = rankDate,
                    UserID = ur.UserID,
                    UserPoint = ur.Point,
                    TournamentID = TournamentID
                };
                //rankList.Add(item);
                _rank.Add(item);
                Row++;
            }
            
            _unitOfWork.SaveChanges();
        }

        public string GetUserRanking(int UserID, int TournamentID)
        {
            int rank = _rank.Where(x => x.UserID == UserID).Where(x=> x.TournamentID == TournamentID ).OrderByDescending(x => x.RankDate).Select(x => x.Rank).FirstOrDefault();
            if (rank == 0)
                return  "-";
            return rank.ToString();
        }

        public void GetPredictLog(Guid MatchID, int UserID, string ScoreTitleID, string Score, bool Status)
        {
            var item = new UserPredictLog
            {
                MatchID = MatchID,
                PUserID = UserID.ToString(),
                PredictionTime = DateTime.Now,
                Score = Score,
                ScoreTitleID = ScoreTitleID,
                Status = Status
            };
            _UserPredictLog.Add(item);
        }

        public IList<UserRankingModel> TopTen(int TournamentID)
        {
            IList<UserRankingModel> score = new List<UserRankingModel>();
            var scorelist = _rank.OrderByDescending(x => x.RankDate).ThenBy(x => x.Rank).Where(x=>x.TournamentID == TournamentID).Select(x => new { x.UserPoint, x.User.Email, x.Rank } ).Take(10).ToList();
            foreach (var x in scorelist)
            {
                score.Add(new UserRankingModel()
                {
                    Rank = x.Rank,
                    Email = x.Email,
                    Point = x.UserPoint
                });
            }
            return score;
        }
    }
}
