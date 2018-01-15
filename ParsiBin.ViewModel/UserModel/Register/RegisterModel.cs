using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.ViewModel.UserModel
{
    public class RegisterModel
    {
        public int ID { get; set; }

        
        [Display(Name = "ایمیل")]
        [EmailAddress]        
        [Required(ErrorMessage ="وارد کردن {0} اجباری است")]
        public string Email { get; set; }

        
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "وارد کردن رمز عبور اجباری است")]
        [DataType(DataType.Password)]
        //[StringLength(14,"کاراکتر بیش از حد", "",null,6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حاصل جمع دو عدد")]
        public string Captcha { get; set; }
    }
}
