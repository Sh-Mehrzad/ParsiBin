using ParsiBin.DataLayer;
using ParsiBin.ViewModel.UserModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Areas.Admin.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserPrediction _userPrediction;
        private readonly IUser _User;
        private readonly IMatchScore _matchscore;
        public PredictionController(IUnitOfWork uow, IUserPrediction userprediction, IUser user, IMatchScore matchscore)
        {
            _uow = uow;            
            _userPrediction = userprediction;
            _User = user;
            _matchscore = matchscore;
        }


        public ActionResult AddPredict()
        {
            return View();
        }


        // GET: Site/Prediction
        [HttpPost]
        public virtual async Task<ActionResult> AddPredict(Guid MatchID, int STID, double Score, int TournamentID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return this.Json("UnAuthorize");
                }
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
                                Score = Score,
                                TournamentID = TournamentID
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
                                Score = Score,
                                TournamentID = TournamentID
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
            catch
            {
                return this.Json("False");
            }

        }
    }
}