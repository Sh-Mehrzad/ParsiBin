using ParsiBin.DataLayer;
using ParsiBin.ViewModel.AdminModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "PBAdmin")]
    public class MatchScoreController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IMatchScore _MatchScore;
        private readonly IMatch _Match;
        #endregion

        #region Constructor
        public MatchScoreController(IUnitOfWork uow, IMatchScore matchscore, IMatch match)
        {
            _uow = uow;
            _MatchScore = matchscore;
            _Match = match;
        }
        #endregion

        [HttpGet]
        // GET: Admin/MatchScore
        public ActionResult Index(Guid? MatchID)
        {
            var dtl = _Match.GetDetail(MatchID.Value);
            ViewBag.TournamentTitle = dtl.TournamentTitle;
            ViewBag.Group = dtl.GroupTitle;
            ViewBag.GroupID = dtl.GroupID;
            ViewBag.HomeTeam = dtl.HomeTeam;
            ViewBag.AwayTeam = dtl.AwayTeam;
            var model = _MatchScore.GetItemList(MatchID.Value);
            return View(model);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(double HomeWin, double HomeWinOrDraws, double Draws, double AwayWin, double AwayWinOrDraws, Guid? MatchID)
        {
            int result = _MatchScore.IsExist(1, MatchID.Value);
            if (result > 0)
            {
                //var viewModel = new MatchScoreModel                 
                //{
                //   MatchID = MatchID.Value,
                //   ScoreTitleID = 1,
                //   Score = HomeWin
                //};
                //_MatchScore.Edit(result, viewModel);
            }
            else
            {
                _MatchScore.Delete(MatchID.Value);
                var mList = new List<MatchScoreModel>() {
                    new MatchScoreModel() { IsEnabled = true, MatchID = MatchID.Value, ScoreTitleID = 1, Score = HomeWin},
                    new MatchScoreModel() { IsEnabled = true, MatchID = MatchID.Value, ScoreTitleID = 2, Score = Draws},
                    new MatchScoreModel() { IsEnabled = true, MatchID = MatchID.Value, ScoreTitleID = 3, Score = AwayWin},
                    new MatchScoreModel() { IsEnabled = true, MatchID = MatchID.Value, ScoreTitleID = 4, Score = HomeWinOrDraws},
                    new MatchScoreModel() { IsEnabled = true, MatchID = MatchID.Value, ScoreTitleID = 5, Score = AwayWinOrDraws},
                };
                _MatchScore.Add(mList);
            }
            
            //HomeWinOrDraws


            await _uow.SaveChangesAsync();
            var dtl = _Match.GetDetail(MatchID.Value);
            ViewBag.TournamentTitle = dtl.TournamentTitle;
            ViewBag.Group = dtl.GroupTitle;
            ViewBag.GroupID = dtl.GroupID;
            ViewBag.HomeTeam = dtl.HomeTeam;
            ViewBag.AwayTeam = dtl.AwayTeam;
            var model = _MatchScore.GetItemList(MatchID.Value);
            return View(model);
        }
    }
}