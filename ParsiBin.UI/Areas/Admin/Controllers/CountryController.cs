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
    public class CountryController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly ICountry _country;
        private string fileName;
        #endregion

        #region Constructor
        public CountryController(IUnitOfWork uow, ICountry country)
        {
            _uow = uow;
            _country = country;
        }
        #endregion

        // GET: Admin/Country
        public ActionResult Index()
        {
            var model = _country.GetItemList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(CountryModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (!_country.IsExist(viewModel.Name))
                {
                    if (upload == null)
                    {
                        fileName = "Unknown.png";
                    }
                    else
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
                        const string uploadDir = "~/Uploads/Country";
                        fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                        upload.SaveAs(imagePath);
                    }
                    

                    var item = new CountryModel
                    {
                        Name = viewModel.Name,
                        Flag = fileName
                    };
                    _country.Add(item);
                    await _uow.SaveChangesAsync();
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
            _country.Delete(ID);
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
                CountryModel model = _country.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, CountryModel viewModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (!_country.IsExist(viewModel.Name, ID))
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
                        const string uploadDir = "~/Uploads/Country";
                        string fileName = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + Path.GetExtension(upload.FileName);
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                        upload.SaveAs(imagePath);
                        viewModel.Flag = fileName;
                    }

                    _country.Edit(ID, viewModel);
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