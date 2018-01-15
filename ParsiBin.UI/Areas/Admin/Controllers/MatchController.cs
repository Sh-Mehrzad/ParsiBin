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
    public class MatchController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IMatch _match;
        private readonly IGroup _Group;
        private readonly IReferee _referee;
        private readonly Istadium _stadium;
        private readonly IPartInGroup _partInGroup;
        private readonly IpartInMatch _partInMatch;
        private readonly IUserPrediction _userPrediction;        
        #endregion

        #region Constructor
        public MatchController(IUnitOfWork uow, IMatch match, IReferee referee, IGroup group, Istadium stadium, IPartInGroup partGroup, IUserPrediction userprediction, IpartInMatch partInMatch)
        {
            _uow = uow;
            _match = match;
            _referee = referee;
            _Group = group;
            _stadium = stadium;
            _partInGroup = partGroup;
            _partInMatch = partInMatch;
            _userPrediction = userprediction;            
        }
        #endregion

        // GET: Admin/Match
        public ActionResult Index(Guid? GroupID)
        {
            var model = _match.GetItemList(null, GroupID.Value);
            var g = _Group.GetDetail(GroupID.Value);
            ViewBag.TourTitle = g.TournamentTitle;
            ViewBag.TournamID = g.TournamentID;
            ViewBag.GroupTitle = g.Title;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(Guid? GroupID)
        {
            ViewBag.StadiumList = STList();
            ViewBag.Referee = RList();
            ViewBag.ParticipantList = PList(GroupID);
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(MatchModel viewModel, Guid? GroupID)
        {
            ViewBag.StadiumList = STList();
            ViewBag.Referee = RList();
            ViewBag.ParticipantList = PList(GroupID);
            if (ModelState.IsValid)
            {
                var item = new MatchModel
                {
                    MatchTime = viewModel.MatchTime,
                    MatchStatus = 0,
                    StadiumID = viewModel.StadiumID,
                    //Referee = _referee.GetDetail(viewModel.RefereeID),
                    RefereeID = viewModel.RefereeID,
                    GroupID = viewModel.GroupID
                };
                var matchID = _match.Add(item);
                var part1 = new PartInMatchModel
                {
                    IsHomeTeam = true,
                    MatchID = matchID,
                    Participant_ID = viewModel.HomeTeamID
                };
                var part2 = new PartInMatchModel
                {
                    IsHomeTeam = false,
                    MatchID = matchID,
                    Participant_ID = viewModel.AwayTeamID
                };
                _partInMatch.Add(part1);
                _partInMatch.Add(part2);
                await _uow.SaveChangesAsync();
                return View();

            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Edit(Guid ID)
        {            
            MatchModel model = _match.GetDetail(ID);
            var PartList = PList(model.GroupID);            
            ViewBag.ParticipantList1 = new SelectList(PartList, "Value", "Text", _partInMatch.GetMatchHomeParticipant(ID));
            ViewBag.ParticipantList2 = new SelectList(PartList, "Value", "Text", _partInMatch.GetMatchAwayParticipant(ID));
            model.MatchTime.ToString("yyyy/MM/dd hh:mm");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MatchModel viewModel, Guid ID, int cmbHomeTeamID, int cmbAwayTeamID)
        {
            var model = _match.GetDetail(ID);
            var PartList = PList(model.GroupID);
            viewModel.GroupID = model.GroupID;
            viewModel.MatchStatus = 1;
            ViewBag.ParticipantList1 = new SelectList(PartList, "Value", "Text", _partInMatch.GetMatchHomeParticipant(ID));
            ViewBag.ParticipantList2 = new SelectList(PartList, "Value", "Text", _partInMatch.GetMatchAwayParticipant(ID));
            _match.Edit(ID, viewModel);
            _partInMatch.Delete(model.ID);            
            var part1 = new PartInMatchModel
            {
                IsHomeTeam = true,
                MatchID = model.ID,
                Participant_ID = cmbHomeTeamID
            };
            var part2 = new PartInMatchModel
            {
                IsHomeTeam = false,
                MatchID = model.ID,
                Participant_ID = cmbAwayTeamID
            };
            _partInMatch.Add(part1);
            _partInMatch.Add(part2);
            _uow.SaveChanges();
            return View(model);
        }

        private List<SelectListItem> STList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _stadium.GetItemList(null))
            {
                pg.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return pg;
        }

        private List<SelectListItem> RList()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (var item in _referee.GetItemList(null, null))
            {
                ls.Add(new SelectListItem { Text = item.name, Value = item.ID.ToString() });
            }
            return ls;
        }

        private List<SelectListItem> PList(Guid? GroupID)
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (var item in _partInGroup.GetItemList(GroupID.Value))
            {
                ls.Add(new SelectListItem { Text = item.Team, Value = item.ParticipantID.ToString() });
            }
            return ls;
        }

        public ActionResult EnableMatch(Guid? MatchID)
        {
            _match.GetEnable(MatchID.Value);
            _uow.SaveChanges();
            return this.Json("True", JsonRequestBehavior.AllowGet);
            //Response.Redirect()
        }

        public ActionResult MatchResult(Guid? MatchID)
        {
            var dtl = _match.GetDetail(MatchID.Value);
            var mScore = _match.GetMatchGoals(MatchID.Value);            
            ViewBag.TournamentTitle = dtl.TournamentTitle;
            ViewBag.Group = dtl.GroupTitle;
            ViewBag.GroupID = dtl.GroupID;
            ViewBag.HomeTeam = dtl.HomeTeam;
            ViewBag.AwayTeam = dtl.AwayTeam;
            try
            {
                var item = new MatchModel
                {
                    ID = mScore.ID,
                    HomeTeamGoal = mScore.HomeTeamGoal,
                    AwayTeamGoal = mScore.AwayTeamGoal
                };
                return View(item);
            }
            catch
            {
                return View();
            }            
            
        }

        [HttpPost]
        public ActionResult MatchResult(Guid? MatchID, MatchModel viewModel)
        {
            var dtl = _match.GetDetail(MatchID.Value);
            ViewBag.TournamentTitle = dtl.TournamentTitle;            
            ViewBag.Group = dtl.GroupTitle;
            ViewBag.GroupID = dtl.GroupID;
            ViewBag.HomeTeam = dtl.HomeTeam;
            ViewBag.AwayTeam = dtl.AwayTeam;
            //if (ModelState.IsValid)
            //{
                //if (viewModel.ID == null)
                //{
                    var item = new MatchModel
                    {
                        ID = MatchID.Value,
                        HomeTeamGoal = viewModel.HomeTeamGoal,
                        AwayTeamGoal = viewModel.AwayTeamGoal
                    };
                    _match.AddMatchResult(item);
                    _uow.SaveChanges();
                //}
                
                
                _userPrediction.SetUserRanking(dtl.TournamentID);
                _uow.SaveChanges();
                return View();
            //}
            //else
            //{
            //    return View();
            //}
        }
    }
}