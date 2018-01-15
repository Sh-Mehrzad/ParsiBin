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
    public class GroupController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IGroup _Group;
        private readonly ITournament _Tournament;
        private readonly ISportType _SportType;
        private readonly IPartInGroup _partInGroup;
        private readonly IParticipant _participant;
        private readonly ICountry _Country;
        #endregion

        #region Constructor
        public GroupController(IUnitOfWork uow, IGroup group, ITournament tournament, ISportType sporttype, IPartInGroup partingroup, IParticipant participant, ICountry country)
        {
            _uow = uow;
            _Group = group;
            _Tournament = tournament;
            _SportType = sporttype;
            _partInGroup = partingroup;
            _participant = participant;
            _Country = country;
        }
        #endregion        

        // GET: Admin/Group
        public ActionResult Index(int? TournamentID)
        {
            if (TournamentID == null)
            {
                return RedirectToAction("Index", "Tournament");
            }
            var model = _Group.GetItemList(TournamentID.Value);
            ViewBag.TournamentTitle = TournamentTitle(TournamentID.Value);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? TournamentID)
        {
            if (TournamentID == null)
            {
                return RedirectToAction("Index", "Tournament");
            }

            ViewBag.SportType = SPList();
            ViewBag.ParticipantType = PTList();
            ViewBag.CountryID = COList();
            ViewBag.TournamentTitle = TournamentTitle(TournamentID.Value);
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(GroupModel viewModel, int? TournamentID)
        {
            ViewBag.SportType = SPList();
            ViewBag.ParticipantType = PTList();
            ViewBag.CountryID = COList();
            if (ModelState.IsValid)
            {
                if (!_Group.IsExist(viewModel.Title, TournamentID.Value, viewModel.SportTypeID.Value))
                {
                    var grp = new GroupModel
                    {
                        Title = viewModel.Title,
                        SportType = viewModel.SportType,
                        SportTypeID = viewModel.SportTypeID,
                        SportTypeTitle = viewModel.SportTypeTitle,
                        Tournament = viewModel.Tournament,
                        TournamentID = viewModel.TournamentID,
                        TournamentTitle = viewModel.TournamentTitle,
                        ParticipantTypeID = viewModel.ParticipantTypeID,
                        CountryID = viewModel.CountryID
                    };
                    _Group.Add(grp, TournamentID.Value);
                    await _uow.SaveChangesAsync();
                    ViewBag.Status = true;
                    return View();
                }
                else
                {
                    ViewBag.Message = "عنوان وارد شده تکراری است.";
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult Edit(Guid ID)
        {
            try
            {
                var model = _Group.GetDetail(ID);
                ViewBag.SportType = new SelectList(SPList(), "Value", "Text", model.SportTypeID);
                ViewBag.ParticipantType = new SelectList(PTList(), "Value", "Text", model.ParticipantTypeID);
                ViewBag.CountryList = new SelectList(COList(), "Value", "Text", model.CountryID);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(GroupModel viewModel, Guid ID)
        {
            var model = _Group.GetDetail(ID);
            ViewBag.SportType = new SelectList(SPList(), "Value", "Text", model.SportTypeID);
            ViewBag.ParticipantType = new SelectList(PTList(), "Value", "Text", model.ParticipantTypeID);
            ViewBag.CountryID = new SelectList(COList(), "Value", "Text", model.CountryID);
            if (!_Group.IsExist(viewModel.Title, ID, viewModel.TournamentID, viewModel.SportTypeID.Value))
            {
                _Group.Edit(ID, viewModel);
                await _uow.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "عنوان وارد شده در سیستم موجود است.";
                return View();
            }
            return View(model);
        }


        private List<SelectListItem> SPList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _SportType.GetSportTypeList())
            {
                pg.Add(new SelectListItem { Text = item.Title, Value = item.ID.ToString() });
            }
            return pg;
        }

        private List<SelectListItem> PTList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _Group.GetParticipantTypeList())
            {
                pg.Add(new SelectListItem { Text = item.ParticipantType, Value = item.ParticipantTypeID.ToString() });
            }
            return pg;
        }


        public string TournamentTitle(int TournamentID)
        {
            var T = _Tournament.GetDetail(TournamentID);
            return T.Title;
        }

        public virtual async Task<ActionResult> Delete(Guid ID, int? TournamentID)
        {
            _Group.Delete(ID);
            await _uow.SaveChangesAsync();
            return RedirectToAction("Index?TournamentID=" + TournamentID.Value);
        }

        [HttpGet]
        public ActionResult PartList(Guid? GroupID)
        {
            try
            {   
                var g = _Group.GetDetail(GroupID.Value);
                ViewBag.TourTitle = g.TournamentTitle;
                ViewBag.TournamID = g.TournamentID;
                ViewBag.GroupTitle = g.Title;
                //ViewBag.ParticipantList = ParticipantList(g.SportTypeID, g.ParticipantTypeID, g.CountryID);
                ViewBag.SportTypeID = g.SportTypeID;
                var PING = _partInGroup.GetItemList(GroupID.Value);
                var model = _participant.GetItemList(g.SportTypeID, g.ParticipantTypeID, g.CountryID);
                foreach(var item in model)
                {
                    foreach(var item2 in PING)
                    {
                        if(item.ID == item2.ParticipantID)
                        {
                            item.IsEnabled = true;
                            break;
                        }
                    }
                }               
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Tournament");
            }
        }

        public ActionResult AddParticipant(Guid GroupID, int ParticipantID)
        {
            _partInGroup.Add(ParticipantID, GroupID);
            _uow.SaveChanges();            
            return this.Json("True", JsonRequestBehavior.AllowGet);
            //Response.Redirect()
        }

        public ActionResult RemoveParticipant(Guid GroupID, int ParticipantID)
        {
            _partInGroup.Remove(ParticipantID, GroupID);
            _uow.SaveChanges();
            return this.Json("True", JsonRequestBehavior.AllowGet);
            //Response.Redirect()
        }

        [HttpPost]
        public virtual async Task<ActionResult> PartList(PartInGroupsModel viewModel, int sportTypeID, Guid? GroupID)
        {
            var test = _Group.GetDetail(GroupID.Value);
            ViewBag.ParticipantList = ParticipantList(sportTypeID, viewModel.ParticipantID, test.CountryID);
            if (ModelState.IsValid)
            {
                if (!_partInGroup.IsExist(viewModel.ParticipantID, viewModel.GroupID))
                {
                    var grp = new PartInGroupsModel
                    {
                        GroupID = viewModel.GroupID,
                        ParticipantID = viewModel.ParticipantID
                    };
                    _partInGroup.Add(grp);
                    await _uow.SaveChangesAsync();
                    ViewBag.Status = true;
                    try
                    {
                        var model = _partInGroup.GetItemList(GroupID.Value);
                        var g = _Group.GetDetail(GroupID.Value);
                        ViewBag.TourTitle = g.TournamentTitle;
                        ViewBag.GroupTitle = g.Title;
                        ViewBag.TournamID = g.TournamentID;
                        ViewBag.ParticipantList = ParticipantList(g.SportTypeID, test.ParticipantTypeID, test.CountryID);
                        ViewBag.SportTypeID = g.SportTypeID;
                        return View(model);
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Tournament");
                    }
                }
                else
                {
                    ViewBag.Message = "عنوان وارد شده تکراری است.";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public virtual async Task<ActionResult> DeletePart(int? PartID, Guid? GroupID)
        {
            _partInGroup.Delete(PartID.Value);
            await _uow.SaveChangesAsync();
            return RedirectToAction("PartList", "Group", new { GroupID = GroupID });
        }

        private List<SelectListItem> ParticipantList(int? SportTypeID, int? ParticipantTypeID, int? CountryID)
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _participant.GetItemList(SportTypeID.Value, ParticipantTypeID, CountryID))
            {
                pg.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return pg;
        }

        private List<SelectListItem> COList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _Country.GetItemList())
            {
                pg.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return pg;
        }
    }
}