using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParsiBin.UI.Controllers
{
    public class TimeController : Controller
    {
        
        // GET: Site/Time
        public string GetShamsiDate()
        {            
            return ParsiBin.ViewModel.AdminModel.UtilityClass.GetTextDate(DateTime.Now);
        }
    }
}