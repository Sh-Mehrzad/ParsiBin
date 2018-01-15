using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class StadiumModel
    {
        public int ID { get; set; }

        [MaxLength(50), Display(Name="نام استادیوم")]
        public string Name { get; set; }
        public City City { get; set; }
        public int CityID { get; set; }

        [Display(Name="شهر")]
        public string CityName { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }
    }
}
