using ParsiBin.DomainClasses;
using ParsiBin.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class RefereeModel
    {
        public int ID { get; set; }

        [MaxLength(50), Display(Name="نام داور"), Required(ErrorMessage="نام داور را وارد نمایید")]
        public string name { get; set; }
        public int SportTypeID { get; set; }
        public virtual SportType SportType { get; set; }

        [Display(Name="سبک ورزشی")]
        public string SportTypeTitle { get; set; }
        public virtual Country Country { get; set; }
        public int CountryID { get; set; }

        [Display(Name="کشور")]
        public string CountryName { get; set; }
    }
}
