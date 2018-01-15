using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class SportTypeModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "لطفا نام نوع مسابقات را وارد کنید"), StringLength(50, ErrorMessage = "حداکثر 50 حرف"), Display(Name = "نام نوع مسابقات")]
        public string Title { get; set; }

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
