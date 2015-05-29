using System;
using System.Collections.Generic;
using Kassandra.Users.Core.Models;

namespace Kassandra.Users.Core
{
    public interface IRoleRepository : IRepository
    {
        void AddUserTo(int roleId, int userId);
        void AddUsersTo(int roleId, IEnumerable<int> userIds);
        void RemoveUserTo(int roleId, int userId);
        void RemoveUsersTo(int roleId, IEnumerable<int> userIds);

        int CreateRole(Role role);

        void DeleteRole(int roleId);

        Role Get(int roleId);
        Role Get(Guid roleUid);
        IEnumerable<Role> GetForUser(int userId);

        bool IsUserInRole(int userId, int roleId);
    }
}