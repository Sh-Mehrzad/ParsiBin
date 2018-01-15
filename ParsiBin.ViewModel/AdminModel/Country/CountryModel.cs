using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class CountryModel
    {
        public int ID { get; set; }

        [MaxLength(50), Display(Name="نام کشور"), Required(ErrorMessage="نام کشور را وارد نمایید")]
        public string Name { get; set; }

        [MaxLength(50), Display(Name="پرچم")]
        public string Flag { get; set; }

        //[Display(Name="لیست شهرها")]
        //public ICollection<City> CityList { get; set; }
    }
}
