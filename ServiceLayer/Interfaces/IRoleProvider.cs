using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IRoleProvider
    {
        void AddUsersToRoles(string[] usernames, string[] roleNames);
        //
        // Summary:
        //     Adds a new role to the data source for the configured applicationName.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to create.
        void CreateRole(string roleName);
        //
        // Summary:
        //     Removes a role from the data source for the configured applicationName.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to delete.
        //
        //   throwOnPopulatedRole:
        //     If true, throw an exception if roleName has one or more members and do not delete
        //     roleName.
        //
        // Returns:
        //     true if the role was successfully deleted; otherwise, false.
        bool DeleteRole(string roleName, bool throwOnPopulatedRole);
        //
        // Summary:
        //     Gets an array of user names in a role where the user name contains the specified
        //     user name to match.
        //
        // Parameters:
        //   roleName:
        //     The role to search in.
        //
        //   usernameToMatch:
        //     The user name to search for.
        //
        // Returns:
        //     A string array containing the names of all the users where the user name matches
        //     usernameToMatch and the user is a member of the specified role.
        string[] FindUsersInRole(string roleName, string usernameToMatch);
        //
        // Summary:
        //     Gets a list of all the roles for the configured applicationName.
        //
        // Returns:
        //     A string array containing the names of all the roles stored in the data source
        //     for the configured applicationName.
        string[] GetAllRoles();
        //
        // Summary:
        //     Gets a list of the roles that a specified user is in for the configured applicationName.
        //
        // Parameters:
        //   username:
        //     The user to return a list of roles for.
        //
        // Returns:
        //     A string array containing the names of all the roles that the specified user
        //     is in for the configured applicationName.
        string[] GetRolesForUser(string username);
        //
        // Summary:
        //     Gets a list of users in the specified role for the configured applicationName.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to get the list of users for.
        //
        // Returns:
        //     A string array containing the names of all the users who are members of the specified
        //     role for the configured applicationName.
        string[] GetUsersInRole(string roleName);
        //
        // Summary:
        //     Gets a value indicating whether the specified user is in the specified role for
        //     the configured applicationName.
        //
        // Parameters:
        //   username:
        //     The user name to search for.
        //
        //   roleName:
        //     The role to search in.
        //
        // Returns:
        //     true if the specified user is in the specified role for the configured applicationName;
        //     otherwise, false.
        bool IsUserInRole(string username, string roleName);
        //
        // Summary:
        //     Removes the specified user names from the specified roles for the configured
        //     applicationName.
        //
        // Parameters:
        //   usernames:
        //     A string array of user names to be removed from the specified roles.
        //
        //   roleNames:
        //     A string array of role names to remove the specified user names from.
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        //
        // Summary:
        //     Gets a value indicating whether the specified role name already exists in the
        //     role data source for the configured applicationName.
        //
        // Parameters:
        //   roleName:
        //     The name of the role to search for in the data source.
        //
        // Returns:
        //     true if the role name already exists in the data source for the configured applicationName;
        //     otherwise, false.
        bool RoleExists(string roleName);
    }
}
