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
    public class MatchModel
    {
        public Guid ID { get; set; }

        [Display(Name = "فعال باشد")]
        public bool IsEnabled { get; set; }

        [Display(Name = "حذف شود")]
        public bool IsDeleted { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedOn { get; set; }

        [Display(Name = "زمان بازی")]
        public DateTime MatchTime { get; set; }

        [Display(Name = "وضعیت بازی")]
        public string MatchStatusText { get; set; }
        public byte MatchStatus { get; set; }
        public int StadiumID { get; set; }

        [Display(Name = "نام استادیوم")]
        public string StadiumTitle { get; set; }
        public virtual Referee Referee { get; set; }
        public int RefereeID { get; set; }

        [Display(Name = "نام داور")]
        public string RefereeTitle { get; set; }
        public virtual Group Group { get; set; }
        public Guid GroupID { get; set; }

        [Display(Name = "گروه")]
        public string GroupTitle { get; set; }

        [Display(Name = "تورنومنت")]
        public string TournamentTitle { get; set; }
        public int TournamentID { get; set; }

        public homeAway homeaway { get; set; }
        public int HomeTeamID { get; set; }
        public int AwayTeamID { get; set; }

        [Display(Name = "تیم میزبان")]
        public string HomeTeam { get; set; }

        [Display(Name = "تیم میهمان")]
        public string AwayTeam { get; set; }

        public string _persianDate { get; set; }
        public string PersianDate
        {
            get
            {
                return _persianDate;
            }
            set
            {
                _persianDate = UtilityClass.GetTextDate(MatchTime);
            }
        }
        public string _persiantime { get; set; }
        public string PersianTime
        {
            get
            {
                return _persiantime;
            }
            set {
                _persiantime = UtilityClass.GetTextTime(MatchTime); 
            }
        }
        public string HomeLogo { get; set; }
        public string AwayLogo { get; set; }
        public double HScore { get; set; }
        public double AScore { get; set; }
        public double XScore { get; set; }
        public double AXScore { get; set; }
        public double HXScore { get; set; }
        public int ScoreTitleID { get; set; }
        public int HomeTeamGoal { get; set; }
        public int AwayTeamGoal { get; set; }


    }
    public class homeAway
    {

        public int HomeTeamID { get; set; }
        public int AwayTeamID { get; set; }

        [Display(Name = "تیم میزبان")]
        public string HomeTeam { get; set; }

        [Display(Name = "تیم میهمان")]
        public string AwayTeam { get; set; }
    }
}
