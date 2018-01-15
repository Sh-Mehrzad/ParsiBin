using ParsiBin.DataLayer;
using ParsiBin.ViewModel.UserModel;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParsiBin.UI.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly IUser _User;
        private readonly IverifyUser _VerifyUser;
        private readonly IRoleProvider _roleProvider;
        #endregion

        #region Constructor
        public RegisterController(IUnitOfWork uow, IUser user, IverifyUser verifyUser, IRoleProvider roleprovider)
        {
            _uow = uow;
            _User = user;
            _VerifyUser = verifyUser;
            _roleProvider = roleprovider;
        }
        #endregion


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(RegisterModel viewModel, string RePassword)
        {
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != viewModel.Captcha)
            {
                ModelState.AddModelError("Captcha", "مجموع اشتباه است");
            }
            if (ModelState.IsValid)
            {
                if (_User.CheckFormat(viewModel.Email, "Email"))
                {
                    if (_User.CheckFormat(viewModel.Password, "Password"))
                    {
                        if (RePassword == viewModel.Password)
                        {
                            if (_User.CheckEmail(viewModel.Email))
                            {
                                if (!_User.CheckActiveEmail(viewModel.Email))
                                {
                                    _VerifyUser.SendVerifyEmail(_User.GetUserID(viewModel.Email), viewModel.Email, "VerifyEmail");
                                    return RedirectToAction("SuccessMessage", "Home", new { area = "" });
                                }
                                else
                                {
                                    ViewBag.Message = "این ایمیل در سیستم پارسی بین قبلا به ثبت رسیده است. ";
                                    return View();
                                }
                            }
                            else
                            {
                                var item = new RegisterModel
                                {
                                    Email = viewModel.Email,
                                    Password = viewModel.Password
                                };
                                _User.Add(item);
                                await _uow.SaveChangesAsync();
                                return RedirectToAction("SuccessMessage", "Home", new { area = "" });
                            }
                        }
                        else
                        {
                            ViewBag.Message = "کلمه عبور به درستی تکرار نشده است.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "انتخاب این رمز عبور مجاز نمیباشد.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "ایمیل وارد شده در سیستم به عنوان ایمیل معتبر شناخته نشد.";
                    return View();
                }
            }
            else
                return View();
        }

        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
                bmp.Dispose();
            }
            
            
            return img;
        }

        [HttpGet]
        public ActionResult VerifyEmail(string UID, Guid? VCode)
        {
            try
            {
                if (_VerifyUser.IsVerifyValid(VCode.Value, int.Parse(UID), "VerifyEmail"))
                {
                    _VerifyUser.VerifyUpdate(VCode.Value, int.Parse(UID));
                    _VerifyUser.VerifyUser(int.Parse(UID));
                    string[] UIDs = new string[] { UID };
                    string[] RoleIDs = new string[] { "1" };
                    _roleProvider.AddUsersToRoles(UIDs, RoleIDs);
                    _uow.SaveChanges();
                    return RedirectToAction("VerifyDone");
                }
                else
                {
                    return RedirectToAction("VerifyProblem");
                }
            }

            catch
            {
                return RedirectToAction("VerifyProblem");
            }
        }

        public ActionResult VerifyDone()
        {
            return View();
        }

        public ActionResult VerifyProblem()
        {
            return View();
        }

        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            //SendSms("09120254408");
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "ClientArea");
            }
        }

        [HttpPost]
        public ActionResult Login(RegisterModel viewModel)
        {
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != viewModel.Captcha)
            {
                ViewBag.Message = "مجموع اشتباه است.";
                return View();
            }
            if (ModelState.IsValid)
            {
                if (_User.CheckFormat(viewModel.Email, "Email"))
                {
                    if (_User.CheckFormat(viewModel.Password, "Password"))
                    {
                        if (_User.Login(viewModel.Email, viewModel.Password))
                        {
                            int UserID = _User.GetUserID(viewModel.Email);
                            var roleID = _roleProvider.GetRolesForUser(UserID.ToString());

                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, viewModel.Email, DateTime.Now, DateTime.Now.AddHours(3), true, roleID.ToString());
                            string encrypt = FormsAuthentication.Encrypt(ticket);
                            HttpCookie Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                            Response.Cookies.Add(Authcookie);

                            return Redirect("~/account/clientarea/index");
                        }
                        else
                        {
                            ViewBag.Message = "ایمیل و یا رمز عبور به درستی وارد نشده است.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "کاراکتر غیر مجاز در رمز عبور.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "ایمیل با فرمت نادرست.";
                    return View();
                }
            }
            else { return View(); }
        }

        [HttpPost]
        public String ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return "لطفا ایمیل خود را وارد نمایید.";
            }
            else if (!_User.CheckFormat(Email, "Email"))
            {
                return "ایمیل وارد شده معتبر نیست";
            }
            else
            {
                if (_User.CheckActiveEmail(Email))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    _VerifyUser.SendForgotPassEmail(_User.GetUserID(Email), Email, "ChangePass");
                    return "ایمیل تغییر رمز برای شما ارسال شد.";
                    //Do somthing
                }
                else if (!_User.CheckEmail(Email))
                {
                    return "ایمیل وارد شده در سیستم پارسی‌بین ثبت نشده است.";
                }
                else
                {
                    _VerifyUser.VerifyUpdate(_User.GetUserID(Email), "VerifyEmail");
                    _uow.SaveChanges();             
                    _VerifyUser.SendVerifyEmail(_User.GetUserID(Email), Email, "VerifyEmail");
                    return "کاربر گرامی شما مشکل تایید حساب دارید. مجددا برای شما ایمیل تایید ارسال شد.";
                }
            }
        }

        public ActionResult ChangePassword(string UID, Guid? VCode)
        {
            try
            {
                if (_VerifyUser.IsVerifyValid(VCode.Value, int.Parse(UID), "ChangePass"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("VerifyProblem");
                }
            }
            catch
            {
                return RedirectToAction("VerifyProblem");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string UID, string Pass, string RePass, Guid? VCode)
        {
            try
            {
                if (Pass == RePass)
                {
                    if (_VerifyUser.IsVerifyValid(VCode.Value, int.Parse(UID), "ChangePass"))
                    {
                        _VerifyUser.ChangePassword(int.Parse(UID), _User.GetHash(Pass));
                        _VerifyUser.VerifyUpdate(VCode.Value, int.Parse(UID));
                        _uow.SaveChanges();
                        return RedirectToAction("VerifyDone");
                    }
                    else
                    {
                        return RedirectToAction("VerifyProblem");
                    }
                }
                else
                {
                    ViewBag.Message = "کلمه عبور به درستی تکرار نشده است.";
                    return View();
                }
                
            }
            catch
            {
                return RedirectToAction("VerifyProblem");
            }
        }

        public void SendSms(string Mobile)
        {
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data.Add("from", "10000100001010");
                data.Add("to", Mobile);
                data.Add("message", "پارسی بین\r\nسامانه رایگان پیش بینی مسابقات ورزشی\r\nکد فعالسازی شما:\r\n652510");
                data.Add("number", "10000100001010");
                data.Add("farsi","1");
                data.Add("api", "d3be430514e5e226b98a9a6a07c6d2c9");
                var result = client.UploadValues("http://www.ehost.ir/billing/usersms.php", data);
            }
        }
    }
}