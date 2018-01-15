using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ParsiBin.DomainClasses
{
    public class Tournament : General.GeneralClassIntID
    {
        
        [Required(ErrorMessage="لطفا نام سری مسابقات را وارد کنید"),StringLength(50, ErrorMessage = "حداکثر 50 حرف"),Display(Name = "نام سری مسابقات")]
        public string Title { get; set; }

        [MaxLength(30)]
        public string Logo { get; set; }        
        public virtual ICollection<Group> Groups { get; set; }
    }
}
