using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class CityModel
    {
        public int ID { get; set; }
    
        [Display(Name="نام شهر")]
        public string Name { get; set; }
        public Country Country { get; set; }
        public int CountryID { get; set; }

        [Display(Name="کشور")]
        public string CountryName { get; set; }
    }
}
