using ParsiBin.DataLayer;
using ParsiBin.ViewModel.AdminModel;
using ParsiBin.ViewModel.UserModel;
using ParsiBin.ViewModel.UserModel.Prediction;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Areas.Admin.Controllers
{    
    [Authorize(Roles = "PBUsers")]
    public class ClientAreaController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IMatch _match;
        private readonly IGroup _Group;
        private readonly IPartInGroup _partInGroup;
        private readonly IpartInMatch _partInMatch;
        private readonly IUserPrediction _userPrediction;
        private readonly IUser _User;
        private readonly IMatchScore _matchscore;
        private readonly ITournament _tournament;
        #endregion

        #region Constructor
        public ClientAreaController(IUnitOfWork uow, IMatch match, IGroup group, IPartInGroup partGroup, IpartInMatch partInMatch, IUserPrediction userprediction, IUser user, IMatchScore matchscore, ITournament tournament)
        {
            _uow = uow;
            _match = match;
            _Group = group;
            _partInGroup = partGroup;
            _partInMatch = partInMatch;
            _userPrediction = userprediction;
            _User = user;
            _matchscore = matchscore;
            _tournament = tournament;
        }
        #endregion

        [AllowAnonymous]
        public ActionResult testNL()
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());            
            var model = _match.GetItemListUserView(UserID,2);
            ViewBag.UserPoint = _userPrediction.GetUserPoint(UserID, 2);
            ViewBag.UserRank = _userPrediction.GetUserRanking(UserID, 2);
            ViewBag.UsrID = UserID;
            return View(model);
            
        }

        public ActionResult _LastGames()
        {
            return PartialView(_match.GetLastMatches(10));
        } 

        public ActionResult _FutureMatches()
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            var result = _match.GetFutureMatches(UserID);
            ViewBag.AllMatches = result.Sum(x=>x.Matches);
            return PartialView(result);
        }

        public ActionResult _FutureMatches2()
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            ViewBag.AllMatches = _match.GetCountFutureMatches();
            return PartialView(_match.GetFutureMatches(UserID));
        }

        public ActionResult _PointList()
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            var TList = _tournament.GetTournamnetList();
            List<UserPointStatusModel> iList = new List<UserPointStatusModel>();
            foreach (var item in TList)
            {
                iList.Add(new UserPointStatusModel { TournamentID = item.ID, Tournament = item.Title, Point = _userPrediction.GetUserPoint(UserID, item.ID), Rank = _userPrediction.GetUserRanking(UserID, item.ID) });
            }
                       
            return PartialView(iList);
        }

        public ActionResult _Top10()
        {
            var TList = _tournament.GetTournamnetList();
            List<UserPointStatusModel> iList = new List<UserPointStatusModel>();
            foreach (var item in TList)
            {
                iList.Add(new UserPointStatusModel { TournamentID = item.ID, Tournament = item.Title});
            }

            return PartialView(iList);
        }

        public ActionResult _History()
        {
            var TList = _tournament.GetTournamnetList();
            List<UserPointStatusModel> iList = new List<UserPointStatusModel>();
            foreach (var item in TList)
            {
                iList.Add(new UserPointStatusModel { TournamentID = item.ID, Tournament = item.Title });
            }

            return PartialView(iList);
        }

        public ActionResult Predict(int? ID)
        {
            try
            {
                int UserID = _User.GetUserID(User.Identity.Name.ToLower());
                var model = _match.GetItemListUserView(UserID,ID.Value);
                var TournamentDetail = _tournament.GetDetail(ID.Value);
                ViewBag.TournamentName = TournamentDetail.Title;
                ViewBag.Point = _userPrediction.GetUserPoint(UserID, ID.Value);
                ViewBag.Rank = _userPrediction.GetUserRanking(UserID, ID.Value);                                
                return View(model);
            }
            catch
            {
                ViewBag.status = "false";
                return View();
            }            
        }

        public ActionResult Top10(int? ID)
        {
            try
            {
                int UserID = _User.GetUserID(User.Identity.Name.ToLower());
                var TournamentDetail = _tournament.GetDetail(ID.Value);
                ViewBag.TournamentName = TournamentDetail.Title;
                ViewBag.Point = _userPrediction.GetUserPoint(UserID, ID.Value);
                ViewBag.Rank = _userPrediction.GetUserRanking(UserID, ID.Value);
                var model = _userPrediction.TopTen(ID.Value);
                if (UserID < 4 && UserID > 0)
                {
                    ViewBag.Admin = "Admin";
                }
                return View(model);
            }
            catch
            {
                ViewBag.status = "false";
                return View();
            }
            
        }

        public ActionResult History(int? ID, int? Get=0, int? Take=15)
        {
            try
            {
                int UserID = _User.GetUserID(User.Identity.Name.ToLower());
                var model = _match.GetUserHistoryListView(UserID, ID.Value, Take.Value).Skip(Get.Value);
                var TournamentDetail = _tournament.GetDetail(ID.Value);
                ViewBag.TournamentName = TournamentDetail.Title;
                ViewBag.Point = _userPrediction.GetUserPoint(UserID, ID.Value);
                ViewBag.Rank = _userPrediction.GetUserRanking(UserID, ID.Value);
                ViewBag.UsrID = UserID;
                return View(model);
            }
            catch
            {
                ViewBag.status = "false";
                return View();
            }
            
        }

        public ActionResult CommingMatchList(int? ID)
        {
            ID = (ID == null) ? ID = 0 : ID=ID;
            
            //int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            var TList = _tournament.GetTournamnetList();
            List<ComingMatchModel> mtch = new List<ComingMatchModel>();
            foreach (var item in TList)
            {
                var x = _match.GetItemListUserView2(ID.Value, item.ID);
                mtch.Add(new ComingMatchModel
                {
                    ID = item.ID,
                    TournamentLogo = item.Logo,
                    TournamentTitle = item.Title,
                    MatchList = x
                });                             
            };
            
            return View(mtch);
        }


        // GET: Site/ClientArea
        public ActionResult Index()
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            //var model = _match.GetItemListUserView(UserID);

            var TList = _tournament.GetTournamnetList();
            List<UserPointStatusModel> iList = new List<UserPointStatusModel>();
            foreach (var item in TList)
            {
                iList.Add(new UserPointStatusModel { TournamentID = item.ID, Tournament = item.Title, Point = _userPrediction.GetUserPoint(UserID, item.ID), Rank = _userPrediction.GetUserRanking(UserID, item.ID) });
            }
            
            ViewBag.UsrID = UserID;
            return View(iList);
        }

        [HttpPost]
        public ActionResult PredictPage(int TournamentID)
        {
            return View();
        }

        
        public ActionResult AddPredict()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> AddPredict(Guid MatchID, int STID, double Score)
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());

            if (_userPrediction.TimeCheck(DateTime.Now, MatchID))
            {
                if (_matchscore.MatchScore(MatchID, STID) != Score)
                {
                    return this.Json("False");
                }
                else {
                    int MatchIDold = _userPrediction.IsExist(MatchID, UserID);
                    if (MatchIDold > 0)
                    {
                        _userPrediction.Delete(MatchIDold);
                        var item = new UserPredictionModel
                        {
                            MatchID = MatchID,
                            Point = 0,
                            UserID = UserID,
                            ScoreTitleID = STID,
                            Score = Score
                        };

                        _userPrediction.Add(item);
                        await _uow.SaveChangesAsync();
                        return this.Json("True");
                    }
                    else
                    {
                        var item = new UserPredictionModel
                        {
                            MatchID = MatchID,
                            Point = 0,
                            UserID = UserID,
                            ScoreTitleID = STID,
                            Score = Score
                        };

                        _userPrediction.Add(item);
                        await _uow.SaveChangesAsync();
                        // TempData["Success"]="با موفقیت ثبت شد";
                    }

                    return this.Json("True");

                }
            }
            else
            {
                return this.Json("False");
            }

        }

        public ActionResult SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        

        [HttpPost]
        public virtual async Task<string> PredictLog(Guid MatchID, string ScoreTitleID, string Score, bool Status)
        {
            int UserID = _User.GetUserID(User.Identity.Name.ToLower());
            _userPrediction.GetPredictLog(MatchID, UserID, ScoreTitleID, Score, Status);
            await _uow.SaveChangesAsync();
            return "Done";
        }

        
    }
}