using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsiBin.DomainClasses;

namespace ParsiBin.ViewModel.AdminModel
{
    public class GroupModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "لطفا نام گروه را وارد کنید"), StringLength(50, ErrorMessage = "حداکثر 50 حرف"), Display(Name = "نام گروه")]
        public string Title { get; set; }
        public int TournamentID { get; set; }

        [Display(Name = "عنوان تورنومنت")]
        public string TournamentTitle { get; set; }
        public virtual Tournament Tournament { get; set; }
        public int? SportTypeID { get; set; }

        [Display(Name = "سبک ورزشی")]
        public string SportTypeTitle { get; set; }
        public virtual SportType SportType { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }

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
        public int FinishMatch { get; set; }
        public int ActiveMatch { get; set; }
        public int InProgressMatch { get; set; }
    }
}
