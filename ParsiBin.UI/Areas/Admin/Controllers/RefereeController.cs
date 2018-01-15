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
    public class RefereeController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IReferee _Referee;
        private readonly ICountry _Country;
        private readonly ISportType _SportType;
        #endregion

        #region Constructor
        public RefereeController(IUnitOfWork uow, IReferee referee, ICountry country, ISportType sporttype)
        {
            _uow = uow;
            _Referee = referee;
            _Country = country;
            _SportType = sporttype;
        }
        #endregion

        // GET: Admin/Referee
        public ActionResult Index(int? SportTypeID, int? CountryID)
        {
            var model = _Referee.GetItemList(SportTypeID, CountryID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.SportType = SPList();
            ViewBag.Country = CList();
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(RefereeModel viewModel)
        {
            ViewBag.SportType = SPList();
            ViewBag.Country = CList();
            if (ModelState.IsValid)
            {
                if (!_Referee.IsExist(viewModel.name))
                {                    
                    var item = new RefereeModel
                    {
                        Country = viewModel.Country,
                        CountryID = viewModel.CountryID,
                        name = viewModel.name,
                        SportType= viewModel.SportType,
                        SportTypeID = viewModel.SportTypeID                        
                    };
                    _Referee.Add(item);
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
            _Referee.Delete(ID);
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
                RefereeModel model = _Referee.GetDetail(ID.Value);
                return View(model);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(int ID, RefereeModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_Referee.IsExist(viewModel.name, ID))
                {
                    _Referee.Edit(ID, viewModel);
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

        private List<SelectListItem> SPList()
        {
            List<SelectListItem> pg = new List<SelectListItem>();
            foreach (var item in _SportType.GetSportTypeList())
            {
                pg.Add(new SelectListItem { Text = item.Title, Value = item.ID.ToString() });
            }
            return pg;
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