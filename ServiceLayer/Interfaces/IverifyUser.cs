using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IverifyUser
    {
        void SendVerifyEmail(int UserID, string Email, string Explain);
        void VerifyUser(int UserID);
        bool IsVerifyValid(Guid VCode, int UserID, string Explain);
        void VerifyUpdate(Guid VCode, int UserID);
        void VerifyUpdate(int UserID, string Explain);
        void SendForgotPassEmail(int UserID, string Email, string Explain);
        void ChangePassword(int UserID, string Password);
    }
}
