using ParsiBin.ViewModel.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IUser
    {
        void Add(RegisterModel viewModel);
        bool CheckEmail(string Email);
        bool CheckActiveEmail(string Email);
        bool CheckFormat(string input, string Type);
        bool Login(string Email, string Password);
        int GetUserID(string Email);
        string GetHash(string text);
    }
}
