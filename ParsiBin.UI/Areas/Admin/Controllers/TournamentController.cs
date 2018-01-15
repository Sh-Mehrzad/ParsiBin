using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using ParsiBin.ViewModel.AdminModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "PBAdmin")]
    public class TournamentController : Controller
    {
        // GET: Admin/Tournament

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly ITournament _tournament;
        private readonly IGroup _group;
        #endregion

        #region Constructor
        public TournamentController(IUnitOfWork uow, ITournament tournament, IGroup group)
        {
            _uow = uow;
            _tournament = tournament;
            _group = group;
        }
        #endregion

        public ActionResult Index()
        {
            return RedirectToAction("TournamentList");
        }

        public ActionResult TournamentList()
        {            
            var model = _tournament.GetTournamnetList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(TournamentModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (! _tournament.IsExist(viewModel.Title))
                {
                    var validImageTypes = new string[]
                        {
                            "image/gif",
                            "image/jpeg",
                            "image/jpg",
                            "image/png"
                        };
                    var httpPostedFileBases = upload as HttpPostedFileBase;
                    if (upload != null && !validImageTypes.Contains(upload.ContentType))
                    {
                        ModelState.AddModelError("", " پسوند تصویر انتخاب شده غیر مجاز است");
                        return View();
                    }

                    if (upload == null)
                    {
                        ModelState.AddModelError("", " بارگزاری تصویر اجباری است");
                        return View();
                    }

                    const string uploadDir = "~/Uploads/Tournament";
                    string fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                    upload.SaveAs(imagePath);

                    var tournam = new TournamentModel
                    {
                        Title = viewModel.Title,
                        Logo = fileName,                        
                    };
                    _tournament.Add(tournam);
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

        public ActionResult DisabledList()
        {
            var model = _tournament.GetDisabledList();
            return View(model);
        }

        public ActionResult DeletedList()
        {
            var model = _tournament.GetDeletedList();
            return View(model);
        }

        public ActionResult Details(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("TournamentList");
            }
            else
            {
                TournamentModel model = _tournament.GetDetail(ID.Value);
                return View(model);
            }
        }

        public virtual async Task<ActionResult> Delete(int ID)
        {
            _tournament.Delete(ID);
            await _uow.SaveChangesAsync();
            return RedirectToAction("TournamentList");
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("TournamentList");
            }
            else
            {
                TournamentModel model = _tournament.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, TournamentModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (! _tournament.IsExist(viewModel.Title, ID))
                {
                    var validImageTypes = new string[]
                        {
                            "image/gif",
                            "image/jpeg",
                            "image/jpg",
                            "image/png"
                        };
                    var httpPostedFileBases = upload as HttpPostedFileBase;
                    if (upload != null && !validImageTypes.Contains(upload.ContentType))
                    {
                        ModelState.AddModelError("", " پسوند تصویر انتخاب شده غیر مجاز است");
                        return View();
                    }
                    if (upload != null)
                    {
                        const string uploadDir = "~/Uploads/Tournament";
                        string fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                        upload.SaveAs(imagePath);
                        viewModel.Logo = fileName;
                    }
                    
                    _tournament.Edit(ID, viewModel);
                    await _uow.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "عنوان وارد شده در سیستم موجود است.";
                    return View();
                }
                
            }
            return View();
        }
    }
}