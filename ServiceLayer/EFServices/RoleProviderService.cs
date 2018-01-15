using ParsiBin.DataLayer;
using ParsiBin.DomainClasses.Entities;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.EFServices
{
    public class RoleProviderService : IRoleProvider
    {

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<UserInRole> _userInRole;
        private readonly IDbSet<Role> _role;
        #endregion

        #region Constructor

        public RoleProviderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userInRole = _unitOfWork.Set<UserInRole>();
            _role = _unitOfWork.Set<Role>();
        }
        #endregion

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(rolename))
                {
                    return;
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in roleNames)
                {
                    if (IsUserInRole(username, rolename))
                    {
                        return;
                    }
                }
            }

            var lst = new UserInRole
            {
                userId = int.Parse(usernames[0]),
                roleId = int.Parse(roleNames[0])
            };
            _userInRole.Add(lst);
        }

        public void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public string[] GetRolesForUser(string username)
        {
            int UsrID = int.Parse(username);
            var RoleID = _userInRole.Where(x => x.userId == UsrID).Select(x => x.roleId).ToList();
            List<string> RoleNames = new List<string>();
            foreach (var RoleArr in RoleID)
            {
                RoleNames.Add(_role.Find(RoleArr).RoleName);

            }
            return RoleNames.ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInRole(string username, string roleName)
        {
            int userid = int.Parse(username);
            int roleid = int.Parse(roleName);
            return _userInRole.Any(x => x.userId == userid && x.roleId == roleid);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(string roleName)
        {
            int roleID = int.Parse(roleName);
            return _role.Any(x => x.roleId == roleID);
        }
    }
}
