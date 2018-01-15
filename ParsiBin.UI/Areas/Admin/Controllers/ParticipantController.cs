using ParsiBin.DataLayer;
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
    public class ParticipantController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IParticipant _participant;
        private readonly ISportType _sportType;
        private readonly IGroup _Group;
        private readonly ICountry _Country;
        #endregion

        #region Constructor
        public ParticipantController(IUnitOfWork uow, IParticipant participant, ISportType sportType, IGroup group, ICountry country)
        {
            _uow = uow;
            _participant = participant;
            _sportType = sportType;
            _Group = group;
            _Country = country;
        }
        #endregion


        // GET: Admin/Participant

        public ActionResult Index()
        {
            var model = _participant.GetItemList(null);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CountryID = COList();
            ViewBag.ParticipantType = PTList();
            ViewBag.SportType = SPList();
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(ParticipantModel viewModel, HttpPostedFileBase upload)
        {
            ViewBag.CountryID = COList();
            ViewBag.ParticipantType = PTList();
            ViewBag.SportType = SPList();
            if (ModelState.IsValid)
            {
                if (!_participant.IsExist(viewModel.Name, viewModel.SportTypeID, viewModel.CountryID))
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
                        ViewBag.Message = " پسوند تصویر انتخاب شده غیر مجاز است";
                        return View();
                    }
                    const string uploadDir = "~/Uploads/Participant";
                    string fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                    upload.SaveAs(imagePath);

                    var item = new ParticipantModel
                    {
                        Name = viewModel.Name,
                        SportType = viewModel.SportType,
                        SportTypeID = viewModel.SportTypeID,
                        Logo = fileName,
                        CountryID = viewModel.CountryID,
                        ParticipantTypeID = viewModel.ParticipantTypeID
                    };
                    _participant.Add(item);
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

        public virtual async Task<ActionResult> Delete(int ID)
        {
            _participant.Delete(ID);
            await _uow.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SportType = SPList();
                ParticipantModel model = _participant.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, ParticipantModel viewModel, HttpPostedFileBase upload)
        {
            ViewBag.SportType = SPList();
            if (ModelState.IsValid)
            {
                if (!_participant.IsExist(viewModel.Name, ID, viewModel.ParticipantTypeID, viewModel.CountryID))
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
                        const string uploadDir = "~/Uploads/Participant";
                        string fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                        upload.SaveAs(imagePath);
                        viewModel.Logo = fileName;
                    }

                    _participant.Edit(ID, viewModel);
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

        private List<SelectListItem> PTList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _Group.GetParticipantTypeList())
            {
                pg.Add(new SelectListItem { Text = item.ParticipantType, Value = item.ParticipantTypeID.ToString() });
            }
            return pg;
        }
        private List<SelectListItem> SPList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _sportType.GetSportTypeList())
            {
                pg.Add(new SelectListItem { Text = item.Title, Value = item.ID.ToString() });
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