using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsiBin.ViewModel.UserModel;
using ParsiBin.DataLayer;
using ParsiBin.DomainClasses;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography;

namespace ServiceLayer.EFServices
{
    public class UserService : IUser
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<User> _User;        
        bool invalid = false;
        #endregion

        #region Constructor
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _User = _unitOfWork.Set<User>();            
        }

        
        #endregion

        public void Add(RegisterModel viewModel)
        {            
            var itm = new User
            {
                Email = viewModel.Email.ToLower(),
                Password = sha256_hash(viewModel.Password),
                IsEnabled = false,
                IsDeleted = false
            };
            _User.Add(itm);
            _unitOfWork.SaveChanges();
            var u =new  verifyUserService(_unitOfWork);
            u.SendVerifyEmail(itm.ID, itm.Email, "VerifyEmail");
        }


        public static String sha256_hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public bool CheckFormat(string input, string Type)
        {
            if (Type == "Email")
            {
                invalid = false;
                if (String.IsNullOrEmpty(input))
                    return false;

                // Use IdnMapping class to convert Unicode domain names.
                try
                {
                    input = Regex.Replace(input, @"(@)(.+)$", this.DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }

                if (invalid)
                    return false;

                // Return true if strIn is in valid e-mail format.
                try
                {
                    return Regex.IsMatch(input,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
            if (Type == "Password")
            {
                invalid = false;
                if (String.IsNullOrEmpty(input))
                    return false;

                if (input.Length < 6)
                    return false;

                if (invalid)
                    return false;

                // Return true if strIn is in valid e-mail format.
                try
                {
                    return Regex.IsMatch(input, @"^[a-zA-Z0-9,!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]*$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
                
        public bool Login(string Email, string Password)
        {
            string hashpass = sha256_hash(Password);
            return _User.Any(x => x.Email == Email.ToLower() && x.Password == hashpass && x.IsDeleted != true && x.IsEnabled == true);
        }

        public int GetUserID(string Email)
        {
            string UsrID = _User.FirstOrDefault(x => x.Email == Email).ID.ToString();
            if (UsrID == null){
                return 0;
            }
            else { return int.Parse(UsrID); }
        }

        public bool CheckEmail(string Email)
        {
            return _User.Any(x => x.Email.ToLower() == Email.ToLower());
        }

        public bool CheckActiveEmail(string Email)
        {
            return _User.Any(x => x.Email.ToLower() == Email.ToLower() && x.IsEnabled == true && x.IsDeleted == false);
        }

        public string GetHash(string text)
        {
            return sha256_hash(text);
        }
    }
}
