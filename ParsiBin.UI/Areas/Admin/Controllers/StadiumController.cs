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
    public class StadiumController : Controller
    {
        
        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly Istadium _Stadium;
        private readonly ICity _City;        
        #endregion

        #region Constructor
        public StadiumController(IUnitOfWork uow, Istadium Stadium, ICity city)
        {
            _uow = uow;
            _City = city;
            _Stadium = Stadium;
        }
        #endregion

        
        public ActionResult Index(int? CityID)
        {
            var model = _Stadium.GetItemList(CityID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {            
            ViewBag.City = CList();
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(StadiumModel viewModel)
        {            
            ViewBag.City = CList();
            if (ModelState.IsValid)
            {
                if (!_Stadium.IsExist(viewModel.Name))
                {                    
                    var item = new StadiumModel
                    {
                        City = viewModel.City,
                        CityID = viewModel.CityID,
                        IsEnabled = viewModel.IsEnabled,
                        Name = viewModel.Name
                    };
                    _Stadium.Add(viewModel);
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
            _Stadium.Delete(ID);
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
                ViewBag.City = CList();
                StadiumModel model = _Stadium.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, StadiumModel viewModel)
        {
            ViewBag.City = CList();
            if (ModelState.IsValid)
            {
                if (!_Stadium.IsExist(viewModel.Name, ID))
                {
                    _Stadium.Edit(ID, viewModel);
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
                
        private List<SelectListItem> CList()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (var item in _City.GetItemList(null))
            {
                ls.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return ls;
        }
    }
}