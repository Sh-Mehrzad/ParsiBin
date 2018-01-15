using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.EFServices
{
    public class verifyUserService : IverifyUser
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<verifyUser> _verifyUser;
        private readonly IDbSet<User> _User;
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        #endregion

        #region Constructor
        public verifyUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _verifyUser = _unitOfWork.Set<verifyUser>();
            _User = _unitOfWork.Set<User>();
        }

        #endregion

        public void SendVerifyEmail(int UserID, string Email, string Explain)
        {
            Guid registercode = Guid.NewGuid();
            DateTime dateValue = DateTime.Now;
            var item = new verifyUser
            {
                UserID = UserID,
                VerifyCode = registercode,
                ExpiredDate = dateValue.AddHours(6),
                Explain = Explain
            };
            _verifyUser.Add(item);
            _unitOfWork.SaveChanges();

            verifyUserService.GmailUsername = "parsiBin@gmail.com";
            verifyUserService.GmailPassword = "PBP@ssw0rd";

            verifyUserService mailer = new verifyUserService();
            mailer.ToEmail = Email;
            mailer.Subject = "Verify your email - تایید شما در سایت پارسی بین";
            string htmlBody = @"<html lang=""fa"">       <body style='direction:rtl; float:right; font-family:tahoma;'><span style='direction:rtl;'><img alt='پارسی بین' src='http://www.parsibin.com/Handlers/ImageHandler/ImageHandler.ashx?h=211&w=324&file=~/Content/img/parsibinfarsi.png' title='پارسی بین'/><br/>این ایمیل توسط تیم پارسی بین ، جهت فعال سازی اکانت شما ارسال شده است.<br> با کلیک بر روی لینک زیر اکانت خود را فعال کنید  <br/> <a href='http://www.parsiBin.com/Account/register/VerifyEmail?VCode=" + registercode + "&UID=" + UserID + "'>لینک فعال سازی</a><br/><br/><br/>  اگر ثبت نام از طرف شما نبوده است، کافی است آن را نادیده بگیرید.   <br/><br/><br/> این ایمیل به صورت خودکار ارسال شده است و لطفا به آن پاسخ ندهید.<br/>سیستم تایید کاربران پارسی بین</span></body></html>";
            mailer.Body = htmlBody;//' "<span style='direction:rtl;'><img alt='پارسی بین' src='http://www.parsibin.com/Handlers/ImageHandler/ImageHandler.ashx?h=211&w=324&file=~/Content/img/parsibinfarsi.png' title='پارسی بین'/><br/>این ایمیل توسط تیم پارسی بین ، جهت فعال سازی اکانت شما ارسال شده است.<br> با کلیک بر روی لینک زیر اکانت خود را فعال کنید  <br/> <a href='http://www.parsiBin.com'>verify</a><br/><br/><br/>  اگر ثبت نام از طرف شما نبوده است، با نادیده گرفتن این ایمیل از بابت حفظ حقوق خود مطمئن باشید.   <br/><br/><br/> این ایمیل به صورت خودکار ارسال شده است و لطفا به آن پاسخ ندهید.<br/>سیستم تایید کاربران پارسی بین</span>";
            mailer.IsHtml = true;
            mailer.Send();

        }

        public verifyUserService()
        {
            GmailHost = "smtp.gmail.com";
            GmailPort = 25; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;            
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            using (var message = new MailMessage(GmailUsername, ToEmail))
            {
                message.From = new MailAddress("parsiBin@gmail.com", "ParsiBin Team");
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                smtp.Send(message);
            }
        }

        public void VerifyUser(int UserID)
        {
            User selectedUser = _User.Find(UserID);
            selectedUser.IsEnabled = true;
            selectedUser.UpdatedOn = DateTime.Now;
        }

        public bool IsVerifyValid(Guid VCode, int UserID, string Explain)
        {
            return _verifyUser.Any(x => x.UserID == UserID && x.VerifyCode == VCode && x.Explain == Explain && x.ExpiredDate > DateTime.Now);            
        }

        public void VerifyUpdate(Guid VCode, int UserID)
        {
            var V = _verifyUser.Where(x => x.UserID == UserID && x.VerifyCode == VCode).FirstOrDefault();
            V.ExpiredDate = DateTime.Now;            
        }

        public void VerifyUpdate(int UserID, string Explain)
        {
            DateTime V = _verifyUser.Where(x => x.UserID == UserID && x.Explain == Explain).Select(x => x.ExpiredDate).FirstOrDefault();
            V = DateTime.Now;
        }

        public void SendForgotPassEmail(int UserID, string Email, string Explain)
        {
            Guid registercode = Guid.NewGuid();
            DateTime dateValue = DateTime.Now;
            var item = new verifyUser
            {
                UserID = UserID,
                VerifyCode = registercode,
                ExpiredDate = dateValue.AddHours(2),
                Explain = Explain
            };
            _verifyUser.Add(item);
            _unitOfWork.SaveChanges();

            verifyUserService.GmailUsername = "parsiBin@gmail.com";
            verifyUserService.GmailPassword = "PBP@ssw0rd";

            verifyUserService mailer = new verifyUserService();
            mailer.ToEmail = Email;
            mailer.Subject = "Change your Password - تایید تغییر رمز ";
            string htmlBody = @"<html lang=""fa"">       <body style='direction:rtl; float:right; font-family:tahoma;'><span style='direction:rtl;'><img alt='پارسی بین' src='http://www.parsibin.com/Handlers/ImageHandler/ImageHandler.ashx?h=211&w=324&file=~/Content/img/parsibinfarsi.png' title='پارسی بین'/><br/>این ایمیل توسط تیم پارسی بین ، جهت تغییر رمز اکانت شما ارسال شده است.<br> با کلیک بر روی لینک زیر مراحل تغییر رمز را دنبال کنید  <br/> <a href='http://www.parsiBin.com/Account/register/ChangePassword?VCode=" + registercode + "&UID=" + UserID + "'>لینک تغییر رمز</a><br/><br/><br/>  اگر درخواست از طرف شما نبوده است، لطفا با ما با ایمیل info@parsibin.com در میان بگذارید.   <br/><br/><br/> این ایمیل به صورت خودکار ارسال شده است و لطفا به آن پاسخ ندهید.<br/>سیستم تایید کاربران پارسی بین</span></body></html>";
            mailer.Body = htmlBody;//' "<span style='direction:rtl;'><img alt='پارسی بین' src='http://www.parsibin.com/Handlers/ImageHandler/ImageHandler.ashx?h=211&w=324&file=~/Content/img/parsibinfarsi.png' title='پارسی بین'/><br/>این ایمیل توسط تیم پارسی بین ، جهت فعال سازی اکانت شما ارسال شده است.<br> با کلیک بر روی لینک زیر اکانت خود را فعال کنید  <br/> <a href='http://www.parsiBin.com'>verify</a><br/><br/><br/>  اگر ثبت نام از طرف شما نبوده است، با نادیده گرفتن این ایمیل از بابت حفظ حقوق خود مطمئن باشید.   <br/><br/><br/> این ایمیل به صورت خودکار ارسال شده است و لطفا به آن پاسخ ندهید.<br/>سیستم تایید کاربران پارسی بین</span>";
            mailer.IsHtml = true;
            mailer.Send();
        }

        public void ChangePassword(int UserID, string Password)
        {
            var user = _User.Find(UserID);
            user.Password = Password;
            user.UpdatedOn = DateTime.Now;            
        }
    }
}
