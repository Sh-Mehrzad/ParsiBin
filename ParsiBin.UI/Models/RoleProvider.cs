using ParsiBin.DataLayer;
using ParsiBin.DomainClasses.Entities;
using ServiceLayer.EFServices;
using ServiceLayer.Interfaces;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;


namespace ParsiBin.UI.Models
{
    public class ParsRoleProvider : RoleProvider
    {        
        public ParsRoleProvider()
        {

        }
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var role = new Container(x => {
                x.For<IUnitOfWork>().Use(new ParsContext());
                x.For<IRoleProvider>().Use<RoleProviderService>();
                x.For<IUser>().Use<UserService>();
            });
            int userid = role.GetInstance<UserService>().GetUserID(username);
            return role.GetInstance<RoleProviderService>().GetRolesForUser(userid.ToString());
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            //int roleID = int.Parse(roleName);
            var role = new Container(x => {
                x.For<IUnitOfWork>().Use(new ParsContext());
                x.For<IRoleProvider>().Use<RoleProviderService>();
            });
            return role.GetInstance<RoleProviderService>().RoleExists(roleName);
            //throw new NotImplementedException();
        }
    }
}
