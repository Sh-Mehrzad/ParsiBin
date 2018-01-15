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
    public class CityController : Controller
    {

        
        #region Fields
        private readonly IUnitOfWork _uow;        
        private readonly ICountry _Country;
        private readonly ICity _City;
        #endregion

        #region Constructor
        public CityController(IUnitOfWork uow, ICountry country, ICity city)
        {
            _uow = uow;            
            _Country = country;
            _City = city;
        }
        #endregion

        // GET: Admin/City
        public ActionResult Index(int? CountryID)
        {
            var model = _City.GetItemList(CountryID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {            
            ViewBag.Country = CList();
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(CityModel viewModel)
        {            
            ViewBag.Country = CList();
            if (ModelState.IsValid)
            {
                if (!_City.IsExist(viewModel.Name))
                {
                    var item = new CityModel
                    {
                        Name = viewModel.Name,
                        CountryID = viewModel.CountryID
                    };
                    _City.Add(item);
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
            _City.Delete(ID);
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
                ViewBag.Country = CList();
                CityModel model = _City.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, CityModel viewModel)
        {
            ViewBag.Country = CList();
            if (ModelState.IsValid)
            {
                if (!_City.IsExist(viewModel.Name, ID))
                {
                    _City.Edit(ID, viewModel);
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
            foreach (var item in _Country.GetItemList())
            {
                ls.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            }
            return ls;
        }
    }
}