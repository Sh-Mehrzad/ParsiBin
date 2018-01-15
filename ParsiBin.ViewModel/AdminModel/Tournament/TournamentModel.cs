using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParsiBin.ViewModel.AdminModel
{
    public class TournamentModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا نام تورنومنت را وارد کنید"), StringLength(50, ErrorMessage = "حداکثر 50 حرف"), Display(Name = "عنوان تورنومنت")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "حداکثر 30 حرف"), Display(Name = "لوگوی تورنومنت")]
        public string Logo { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }        
        public int GroupCount { get; set; }
        public int ActiveMatch { get; set; }
        public int InProgressMatch { get; set; }

    }
}
