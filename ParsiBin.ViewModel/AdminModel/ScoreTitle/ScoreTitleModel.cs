using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.AdminModel
{
    public class ScoreTitleModel
    {
        public int ID { get; set; }

        [MaxLength(50), Display(Name = "عنوان ضریب"), Required(ErrorMessage = "عنوان ضریب را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات مربوط به ضریب")]
        public string Description { get; set; }        
    }
}
