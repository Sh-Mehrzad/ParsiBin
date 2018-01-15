using ParsiBin.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class ParticipantModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا نام شرکت کننده را وارد کنید"), StringLength(50, ErrorMessage = "حداکثر 50 حرف"), Display(Name = "تیم")]
        public string Name { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }

        [StringLength(50, ErrorMessage = "حداکثر 30 حرف"), Display(Name = "لوگوی شرکت کننده")]
        public string Logo { get; set; }

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }

        [Display(Name = "نوع مسابقه")]
        public string SportTypeTitle { get; set; }
        public SportType SportType { get; set; }

        [Display(Name = "نوع ورزش")]
        public int SportTypeID { get; set; }
        public int _ParticipantTypeID { get; set; }
        [Display(Name = "نوع تیم")]
        public int ParticipantTypeID /*{ get; set; }*/
        {
            get
            {
                return _ParticipantTypeID;
            }
            set
            {
                _ParticipantTypeID = value;
                if (value == 2)
                {
                    ParticipantType = "باشگاهی";
                }
                else
                {
                    ParticipantType = "ملی ";
                }
            }
        }

        public string ParticipantType { get; set; }
        public int CountryID { get; set; }

        [Display(Name = "کشور")]
        public string Country { get; set; }
    }
}
