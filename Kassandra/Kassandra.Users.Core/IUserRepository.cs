using System;
using System.Collections.Generic;
using Kassandra.Users.Core.Models;

namespace Kassandra.Users.Core
{
    public interface IUserRepository : IRepository
    {
        bool ChangePassword(string username, string oldPassword, string newPassword);

        Guid CreateUser(User user);

        void DeleteUser(int userId);
        //void DeleteUser(Guid userUid);

        User Get(int id);
        User Get(Guid uid);
        User GetByUsername(string username);
        User GetByEmail(string email);
        IEnumerable<User> GetByRole(int roleId);
        IEnumerable<User> GetAll();

        void LockUser(int userId);
        //void LockUser(Guid userUid);
        void UnlockUser(int userId);
        //void UnlockUser(Guid userUid);

        User AuthenticateUsername(string username, string password);
        User AuthenticateEmail(string email, string password);
    }
}