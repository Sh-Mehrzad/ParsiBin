using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
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
    public class SportTypeController : Controller
    {
        // GET: Admin/SportType

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly ISportType _sporttype;
        #endregion

        #region Constructor
        public SportTypeController(IUnitOfWork uow, ISportType sporttype)
        {
            _uow = uow;
            _sporttype = sporttype;
        }
        #endregion
        public ActionResult Index()
        {
            var model = _sporttype.GetSportTypeList();
            return View(model);
        }

        
        public virtual async Task<ActionResult> Delete(int ID)
        {
            _sporttype.Delete(ID);
            await _uow.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(SportTypeModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (! _sporttype.IsExist(viewModel.Title))
                {
                    var obj = new SportTypeModel
                    {
                        Title = viewModel.Title,
                        IsEnabled = viewModel.IsEnabled
                    };
                    _sporttype.Add(obj);
                    await _uow.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "عنوان تکراری است.";
                    return View();
                }
            }
            return View();
        }
    }
}