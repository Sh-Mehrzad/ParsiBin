using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "PBAdmin")]
    public class ScoreTitleController : Controller
    {
        // GET: Admin/ScoreTitle
        public ActionResult Index()
        {
            return View();
        }
    }
}