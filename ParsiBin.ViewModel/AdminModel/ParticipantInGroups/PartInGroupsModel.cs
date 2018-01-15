using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class PartInGroupsModel
    {
        public int ID { get; set; }

        [Display(Name = "تورنومنت")]
        public string TournamentTitle { get; set; }

        //[Display(Name = "گروه")]
        //public string GroupTitle { get; set; }

        [Display(Name = "تیم")]
        public string Team { get; set; }
        public Guid GroupID { get; set; }
        public int ParticipantID { get; set; }

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
