using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using ParsiBin.DomainClasses.Entities;
using ParsiBin.ViewModel.AdminModel;
using ParsiBin.ViewModel.UserModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.EFServices
{
    public class MatchService : IMatch
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Match> _Match;
        private readonly IDbSet<Group> _Group;
        private readonly IDbSet<Referee> _Referee;
        private readonly IDbSet<Participant> _participant;
        private readonly IDbSet<ParticipantInMatch> _partInMatch;
        private readonly IDbSet<Tournament> _Tournament;
        private readonly IDbSet<MatchScore> _matchScore;
        private readonly IDbSet<UserPrediction> _userPrediction;
        private readonly IDbSet<MatchResult> _matchResult;
        #endregion

        #region Constructor
        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Match = _unitOfWork.Set<Match>();
            _Referee = _unitOfWork.Set<Referee>();
            _Group = _unitOfWork.Set<Group>();
            _participant = _unitOfWork.Set<Participant>();
            _partInMatch = _unitOfWork.Set<ParticipantInMatch>();
            _Tournament = _unitOfWork.Set<Tournament>();
            _matchScore = _unitOfWork.Set<MatchScore>();
            _userPrediction = _unitOfWork.Set<UserPrediction>();
            _matchResult = _unitOfWork.Set<MatchResult>();
        }
        #endregion

        #region List
        public IList<MatchModel> GetItemList(byte? MatchStatus, Guid? GroupIDs)
        {
            //if(MatchStatus == null) { MatchStatus = 1; }
            MatchStatus = MatchStatus == null ? 1 : MatchStatus;
            var MList = _Match.Where(x => x.GroupID == GroupIDs && x.IsDeleted != true).OrderByDescending(x => x.MatchTime).AsQueryable();
            //if (GroupIDs != null) MList.Where(x => x.GroupID == GroupIDs);
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();

            var lst2 = (from M in MList
                        select new { M.ID, M.MatchStatus, M.MatchTime, M.IsEnabled, M.GroupID, p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault() });
            //if (GroupID != null) lst2.Where(x => x.GroupID == GroupID.Value);



            return lst2.Select(a => new MatchModel
            {
                ID = a.ID,
                //CreatedOn = a.Match.CreatedOn.ToString(),
                //Group = a.Match.Group,
                GroupID = a.GroupID,
                //GroupTitle = a.Match.Group.Title,
                //IsDeleted = a.Match.IsDeleted,
                IsEnabled = a.IsEnabled,
                MatchStatus = a.MatchStatus,
                MatchTime = a.MatchTime,
                //Referee = a.Referee,
                //RefereeID = a.Match.RefereeID,
                //RefereeTitle = a.Match.Referee.name,
                //StadiumID = a.Match.StadiumID,                
                //UpdatedOn = a.Match.UpdatedOn.ToString(),
                //TournamentTitle  = a.Match.Group.tournament.Title,
                //homeaway = new homeAway()
                //{
                //    HomeTeam = a.IsHomeTeam == true ? a.Participant.Name : null,
                //    AwayTeam = a.IsHomeTeam == false ? a.Participant.Name : null
                //}
                HomeTeam = a.p1.ToString(),
                AwayTeam = a.P2.ToString()
            }).ToList();

        }

        public IList<MatchModel> GetItemListUserView(int UserID, int TournamentID)
        {
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            DateTime MaxTime = DateTime.Now.AddHours(38);

            var lst = (from M in _Match
                       where M.MatchStatus < 3 && M.IsEnabled == true && M.MatchTime > DateTime.Now && M.MatchTime < MaxTime && M.Group.TournamentID == TournamentID
                       orderby M.MatchTime
                       select new { M.ID, M.MatchStatus, M.MatchTime, M.GroupID, GroupName = (from g in _Group where g.ID == M.GroupID select g.Title).FirstOrDefault(), ScoreTitleID = (from up in _userPrediction where up.MatchID == M.ID && up.PUserID == UserID select up.ScoreTitleID).FirstOrDefault(), p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), p1Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Logo).FirstOrDefault(), P2Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Logo).FirstOrDefault() });

            //foreach (var z in lst)
            //{
            //    lst = (from pp in z
            //           select new { HScore = (from x in _matchScore where x.MatchID == pp.ID && x.ScoreTitleID == 1 select x.Score).FirstOrDefault(), AScore = (from x in _matchScore where x.MatchID == pp.ID && x.ScoreTitleID == 3 select x.Score).FirstOrDefault(), DScore = (from x in _matchScore where x.MatchID == pp.ID && x.ScoreTitleID == 2 select x.Score).FirstOrDefault(), HDScore = (from x in _matchScore where x.MatchID == pp.ID && x.ScoreTitleID == 4 select x.Score).FirstOrDefault(), ADScore = (from x in _matchScore where x.MatchID == pp.ID && x.ScoreTitleID == 5 select x.Score).FirstOrDefault() });
            //} 
            List<MatchModel> mtch = new List<MatchModel>();
            foreach (var a in lst)
            {
                mtch.Add(new MatchModel()
                {
                    ID = a.ID,
                    MatchStatus = a.MatchStatus,
                    MatchTime = a.MatchTime,
                    PersianDate = a.MatchTime.ToString(),
                    PersianTime = a.MatchTime.ToString(),
                    HomeTeam = a.p1.ToString(),
                    AwayTeam = a.P2.ToString(),
                    GroupID = a.GroupID,
                    GroupTitle = a.GroupName.ToString(),
                    AwayLogo = a.P2Logo,
                    HomeLogo = a.p1Logo,
                    ScoreTitleID = a.ScoreTitleID,
                    HScore = GetScore(a.ID, 1),
                    AScore = GetScore(a.ID, 3),
                    XScore = GetScore(a.ID, 2),
                    HXScore = GetScore(a.ID, 4),
                    AXScore = GetScore(a.ID, 5)
                });
            }

            return mtch;
            //return lst.Select(a => new MatchModel
            //{
            //    ID = a.ID,
            //    MatchStatus = a.MatchStatus,
            //    MatchTime = a.MatchTime,
            //    PersianDate = a.MatchTime.ToString(),
            //    PersianTime = a.MatchTime.ToString(),
            //    HomeTeam = a.p1.ToString(),
            //    AwayTeam = a.P2.ToString(),
            //    GroupID = a.GroupID,
            //    GroupTitle = a.GroupName.ToString(),
            //    AwayLogo = a.P2Logo,
            //    HomeLogo = a.p1Logo,
            //    //HScore = GetScore(a.ID, 1),
            //    //AScore = GetScore(a.ID, 3),
            //    //XScore = GetScore(a.ID, 2),
            //    //HXScore = GetScore(a.ID, 4),
            //    //AXScore = GetScore(a.ID, 5),
            //    ScoreTitleID = a.ScoreTitleID
            //}).ToList();
        }

        public IList<MatchModel> GetItemListUserView2(int AddDays, int TournamentID)
        {
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            DateTime MaxTime = DateTime.Now.AddDays(AddDays);

            var lst = (from M in _Match
                       where M.MatchStatus < 3 && M.IsEnabled == true && DbFunctions.TruncateTime(M.MatchTime) == MaxTime.Date  && M.Group.TournamentID == TournamentID
                       orderby M.MatchTime
                       select new { M.ID, M.MatchStatus, M.MatchTime, M.GroupID, GroupName = (from g in _Group where g.ID == M.GroupID select g.Title).FirstOrDefault(),  p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), p1Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Logo).FirstOrDefault(), P2Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Logo).FirstOrDefault() });

            List<MatchModel> mtch = new List<MatchModel>();
            foreach (var a in lst)
            {
                mtch.Add(new MatchModel()
                {
                    ID = a.ID,
                    MatchStatus = a.MatchStatus,
                    MatchTime = a.MatchTime,
                    PersianDate = a.MatchTime.ToString(),
                    PersianTime = a.MatchTime.ToString(),
                    HomeTeam = a.p1.ToString(),
                    AwayTeam = a.P2.ToString(),
                    GroupID = a.GroupID,
                    GroupTitle = a.GroupName.ToString(),
                    AwayLogo = a.P2Logo,
                    HomeLogo = a.p1Logo                    
                });
            }

            return mtch;
        }

        public IList<LastMatchesModel> GetLastMatches(int Number)
        {
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            var Group = _Group.AsQueryable();
            var MatchList = _matchResult.AsQueryable();
            var lst = (from M in _Match
                       where M.MatchStatus == 3 && M.IsEnabled == true && M.MatchTime < DateTime.Now
                       join x in MatchList on M.ID equals x.MatchID
                       join G in Group on M.GroupID equals G.ID
                       orderby M.MatchTime descending
                       select new { x.AwayGoal, x.HomeGoal, G.Title, tournamet = G.tournament.Title, p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault() }).Take(Number);

            List<LastMatchesModel> mtch = new List<LastMatchesModel>();
            foreach (var a in lst)
            {
                mtch.Add(new LastMatchesModel()
                {
                    Goalx = a.HomeGoal.ToString(),
                    Goaly = a.AwayGoal.ToString(),
                    Group = a.Title,
                    Tournament = a.tournamet,
                    Participantx = a.p1,
                    ParticipantY = a.P2
                });
            }
            return mtch;
        }

        public IList<LastMatchesModel> GetLastMatches(int TournamentID, int Number)
        {
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            var Group = _Group.AsQueryable();
            var MatchList = _matchResult.AsQueryable();
            var lst = (from M in _Match
                       where M.MatchStatus == 3 && M.IsEnabled == true && M.MatchTime < DateTime.Now
                       join x in MatchList on M.ID equals x.MatchID
                       join G in Group on M.GroupID equals G.ID
                       where G.TournamentID == TournamentID
                       orderby M.MatchTime descending
                       select new { x.AwayGoal, x.HomeGoal, G.Title, tournamet = G.tournament.Title, p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault() }).Take(Number);

            List<LastMatchesModel> mtch = new List<LastMatchesModel>();
            foreach (var a in lst)
            {
                mtch.Add(new LastMatchesModel()
                {
                    Goalx = a.HomeGoal.ToString(),
                    Goaly = a.AwayGoal.ToString(),
                    Group = a.Title,
                    Tournament = a.tournamet,
                    Participantx = a.p1,
                    ParticipantY = a.P2
                });
            }
            return mtch;
        }

        public IList<FutureMatchesModel> GetFutureMatches(int UserID)
        {
            var Tournament = _Tournament.AsQueryable();
            var lst = (from T in Tournament
                       where T.IsEnabled == true && T.IsDeleted == false
                       select new
                       { TourID = T.ID, TourName = T.Title });
            List<FutureMatchesModel> mtch = new List<FutureMatchesModel>();
            int MatchCount = 0;
            DateTime MaxTime = DateTime.Now.AddHours(38);
            DateTime MinTime = DateTime.Now.AddHours(-60);
            foreach (var a in lst)
            {
                var x = (from UP in _userPrediction
                         where UP.TournamentID == a.TourID
                         where UP.PredictionTime > MinTime
                         where UP.PUserID == UserID
                         select UP.MatchID).ToList();

                var Matches = (from M in _Match
                               where M.Group.TournamentID == a.TourID
                               where M.IsDeleted == false && M.IsEnabled == true && M.MatchStatus == 1 && M.MatchTime > DateTime.Now && M.MatchTime < MaxTime
                               where x.Contains(M.ID)
                               select M.ID).ToList();

                var AllMatches = (from M in _Match
                                  where M.Group.TournamentID == a.TourID
                                  where M.IsDeleted == false && M.IsEnabled == true && M.MatchStatus == 1 && M.MatchTime > DateTime.Now && M.MatchTime < MaxTime
                                  select M.ID).Count();

                

                //var firstNotSecond = Matches.Except(x).ToList();
                //var secondNotFirst = x.Except(Matches).ToList();

                MatchCount = AllMatches - Matches.Count() ;

                mtch.Add(new FutureMatchesModel()
                {
                    TournamentID = a.TourID,
                    Tournament = a.TourName,
                    Matches = MatchCount
                });
            }
            return mtch;
        }

        public IList<UserPredictionHistoryModel> GetUserHistoryListView(int UserID, int TournamentID, int Take)
        {
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            var MatchList = _matchResult.AsQueryable();
            var lst = (from M in _Match
                       where M.MatchStatus == 3 && M.IsEnabled == true && M.MatchTime < DateTime.Now where M.Group.TournamentID == TournamentID
                       join x in MatchList on M.ID equals x.MatchID 
                       from y in _userPrediction 
                       .Where(x=>x.MatchID == M.ID && x.PUserID == UserID).DefaultIfEmpty()
                       //on M.ID equals y.MatchID
                       //where y.PUserID == UserID && y.TournamentID == TournamentID                        
                       orderby M.MatchTime descending
                       select new { M.ID, M.MatchStatus, M.MatchTime, M.GroupID, x.AwayGoal, ScoreTitleID = (y.ScoreTitleID == null ? 0 : y.ScoreTitleID) , Point = (y.Point == null ? 0 : y.Point) , x.HomeGoal, GroupName = (from g in _Group where g.ID == M.GroupID select g.Title).FirstOrDefault(), p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), p1Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Logo).FirstOrDefault(), P2Logo = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Logo).FirstOrDefault() }).Take(Take);

            List<UserPredictionHistoryModel> mtch = new List<UserPredictionHistoryModel>();
            foreach (var a in lst)
            {
                mtch.Add(new UserPredictionHistoryModel()
                {
                    MatchID = a.ID,
                    AwayTeamGoal = a.AwayGoal,
                    HomeTeamGoal = a.HomeGoal,
                    MatchTime = a.MatchTime,
                    PersianDate = a.MatchTime.ToString(),
                    PersianTime = a.MatchTime.ToString(),
                    HomeTeam = a.p1.ToString(),
                    AwayTeam = a.P2.ToString(),
                    GroupID = a.GroupID,
                    GroupName = a.GroupName.ToString(),
                    AwayLogo = a.P2Logo,
                    HomeLogo = a.p1Logo,
                    Score = a.Point,
                    UserPredictScoreTitle = UserpredictionText(a.ScoreTitleID, a.p1, a.P2)
                });
            }

            return mtch;
        }

        private string UserpredictionText(int UserPredictID, string HomeTeam, string AwayTeam)
        {
            switch (UserPredictID)
            {
                case 0:
                    return "پیش بینی نشده";
                case 1:
                    {
                        return "برد " + HomeTeam;
                    }
                case 2:
                    {
                        return "مساوی";
                    }
                case 3:
                    {
                        return "برد " + AwayTeam;
                    }
                case 4:
                    {
                        return "برد مساوی " + HomeTeam;
                    }
                default:
                    {
                        return "برد مساوی " + AwayTeam;
                    }
            }
        }

        private double GetScore(Guid MatchID, int ScoreTitleID)
        {
            return (from x in _matchScore where x.MatchID == MatchID && x.ScoreTitleID == ScoreTitleID select x.Score).FirstOrDefault();
        }

        public IList<MatchModel> GetDeletedList()
        {
            var TList = _Match.AsQueryable();
            IQueryable<Match> lst = TList.Include(x => x.Group).Include(x => x.Referee).Where(x => x.IsDeleted == true).OrderBy(x => x.MatchTime);

            return lst.Select(a => new MatchModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                Group = a.Group,
                GroupID = a.GroupID,
                GroupTitle = a.Group.Title,
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                MatchStatus = a.MatchStatus,
                MatchTime = a.MatchTime,
                Referee = a.Referee,
                RefereeID = a.RefereeID,
                RefereeTitle = a.Referee.name,
                StadiumID = a.StadiumID,
                UpdatedOn = a.UpdatedOn.ToString()
            }).ToList();
        }

        public IList<MatchModel> GetDisabledList()
        {
            var TList = _Match.AsQueryable();
            IQueryable<Match> lst = TList.Include(x => x.Group).Include(x => x.Referee).Where(x => x.IsDeleted == false && x.IsEnabled == false).OrderBy(x => x.MatchTime);

            return lst.Select(a => new MatchModel
            {
                ID = a.ID,
                CreatedOn = a.CreatedOn.ToString(),
                Group = a.Group,
                GroupID = a.GroupID,
                GroupTitle = a.Group.Title,
                IsDeleted = a.IsDeleted,
                IsEnabled = a.IsEnabled,
                MatchStatus = a.MatchStatus,
                MatchTime = a.MatchTime,
                Referee = a.Referee,
                RefereeID = a.RefereeID,
                RefereeTitle = a.Referee.name,
                StadiumID = a.StadiumID,
                UpdatedOn = a.UpdatedOn.ToString()
            }).ToList();
        }


        #endregion


        #region Add/Delete/Edit/View

        public Guid Add(MatchModel viewModel)
        {
            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            var lst = new Match
            {
                ID = g,
                Group = viewModel.Group,
                GroupID = viewModel.GroupID,
                IsDeleted = false,
                IsEnabled = false,
                MatchStatus = 1,
                MatchTime = viewModel.MatchTime,
                Referee = viewModel.Referee,
                RefereeID = viewModel.RefereeID,
                StadiumID = viewModel.StadiumID
            };
            _Match.Add(lst);
            return g;
        }

        public void AddMatchResult(MatchModel viewModel)
        {
            if (!MathResultExist(viewModel.ID))
            {
                var lst = new MatchResult
                {
                    MatchID = viewModel.ID,
                    HomeGoal = viewModel.HomeTeamGoal,
                    AwayGoal = viewModel.AwayTeamGoal
                };
                _matchResult.Add(lst);
                ChangeMatchStatus(viewModel.ID);
                var u = new UserPredictionService(_unitOfWork);
                if (viewModel.HomeTeamGoal > viewModel.AwayTeamGoal)
                {
                    IList<int> lstwin = new List<int>();
                    lstwin.Add(1);
                    lstwin.Add(4);
                    u.UpdatePoint(lstwin, viewModel.ID);
                }
                else if (viewModel.HomeTeamGoal < viewModel.AwayTeamGoal)
                {
                    IList<int> lstwin = new List<int>();
                    lstwin.Add(3);
                    lstwin.Add(5);
                    u.UpdatePoint(lstwin, viewModel.ID);
                }
                else if (viewModel.HomeTeamGoal == viewModel.AwayTeamGoal)
                {
                    IList<int> lstwin = new List<int>();
                    lstwin.Add(2);
                    lstwin.Add(4);
                    lstwin.Add(5);
                    u.UpdatePoint(lstwin, viewModel.ID);
                }

            }
        }

        public bool MathResultExist(Guid MatchID)
        {
            return (_matchResult.Any(x => x.MatchID == MatchID));
        }

        public void Delete(Guid ID)
        {
            Match selectedlst = _Match.Find(ID);
            selectedlst.IsDeleted = true;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public void Edit(Guid ID, MatchModel viewModel)
        {
            Match selectedlst = _Match.Find(ID);
            Group Grouplst = _Group.Find(viewModel.GroupID);
            //Referee Refereelst = _Referee.Find(selectedlst.RefereeID);
            selectedlst.Group = Grouplst;
            selectedlst.GroupID = viewModel.GroupID;
            selectedlst.IsEnabled = viewModel.IsEnabled;
            selectedlst.IsDeleted = viewModel.IsDeleted;
            selectedlst.MatchStatus = viewModel.MatchStatus;
            selectedlst.MatchTime = viewModel.MatchTime;
            //selectedlst.Referee = Refereelst;
            //selectedlst.RefereeID = viewModel.RefereeID;
            //selectedlst.StadiumID = viewModel.StadiumID;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public MatchModel GetMatchGoals(Guid MatchID)
        {
            var MList = _Match.AsQueryable();
            var MRList = _matchResult.AsQueryable();
            var lst2 = (from M in MList
                        join MR in MRList on M.ID equals MR.MatchID
                        select new { MR.AwayGoal, MR.HomeGoal, MR.MatchID });

            return lst2.Where(x => x.MatchID == MatchID).Select(x =>
                new MatchModel
                {
                    ID = x.MatchID,
                    HomeTeamGoal = x.HomeGoal,
                    AwayTeamGoal = x.AwayGoal
                }).FirstOrDefault();
        }

        public MatchModel GetDetail(Guid ID)
        {
            var MList = _Match.AsQueryable();
            var PList = _participant.AsQueryable();
            var PartInMList = _partInMatch.AsQueryable();
            var GList = _Group.AsQueryable();
            var TList = _Tournament.AsQueryable();
            var lst2 = (from M in MList
                        join g in GList on M.GroupID equals g.ID
                        join t in TList on g.TournamentID equals t.ID
                        select new { M.ID, M.IsDeleted, M.IsEnabled, M.MatchStatus, M.MatchTime, M.GroupID, GroupTitle = g.Title, tournametID = t.ID, touranemtTitle = t.Title, p1 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == true && PinM.MatchID == M.ID select P.Name).FirstOrDefault(), P2 = (from PinM in PartInMList join P in PList on PinM.Participant_ID equals P.ID where PinM.IsHomeTeam == false && PinM.MatchID == M.ID select P.Name).FirstOrDefault() });
            return lst2.Where(x => x.ID == ID).Select(x =>
                new MatchModel
                {
                    ID = x.ID,
                    //CreatedOn = x.CreatedOn.ToString(),
                    GroupTitle = x.GroupTitle,
                    //Group = x.Group,
                    GroupID = x.GroupID,
                    IsDeleted = x.IsDeleted,
                    IsEnabled = x.IsEnabled,
                    MatchStatus = x.MatchStatus,
                    MatchTime = x.MatchTime,
                    //Referee = x.Referee,
                    //RefereeID = x.RefereeID ,
                    //RefereeTitle = x.Referee.name,
                    //StadiumID = x.StadiumID ,
                    //UpdatedOn = x.UpdatedOn.ToString(),
                    TournamentTitle = x.touranemtTitle,
                    TournamentID = x.tournametID,
                    HomeTeam = x.p1,
                    AwayTeam = x.P2
                }).FirstOrDefault();
        }

        public void ChangeMatchStatus(Guid MatchID)
        {
            Match selectedlst = _Match.Find(MatchID);
            selectedlst.MatchStatus = 3;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public void GetEnable(Guid ID)
        {
            Match selectedlst = _Match.Find(ID);
            selectedlst.IsEnabled = true;
            selectedlst.UpdatedOn = DateTime.Now;
        }

        public int GetCountFutureMatches()
        {
            DateTime MaxTime = DateTime.Now.AddHours(38);
            int Matches = (from M in _Match
                           where M.IsDeleted == false && M.IsEnabled == true && M.MatchStatus == 1 && M.MatchTime > DateTime.Now && M.MatchTime < MaxTime
                           select M.ID).Count();
            return Matches;
        }

        

        #endregion
    }
}
