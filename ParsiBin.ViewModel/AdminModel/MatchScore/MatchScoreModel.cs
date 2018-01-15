using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class MatchScoreModel
    {
        public int ID { get; set; }

        public Guid MatchID { get; set; }

        [Display(Name ="مسابقه")]
        public string MatchTitle { get; set; }

        public int ScoreTitleID { get; set; }

        [Display(Name ="ضریب")]        
        public double Score { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }              

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }
        public string ScoreTitle { get; set; }
    }
}
