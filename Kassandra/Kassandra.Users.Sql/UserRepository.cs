using System;
using System.Collections.Generic;
using Common.Logging;
using Kassandra.Connector.Sql.Factories;
using Kassandra.Core;
using Kassandra.Core.Components;
using Kassandra.Users.Core;
using Kassandra.Users.Core.Models;

namespace Kassandra.Users.Sql
{
    internal class UserRepository : IUserRepository
    {
        private readonly IContext _context;
        private readonly ILog _logger;

        public UserRepository(string connectionString)
        {
            _context = SqlContextFactory.Instance.GetContext(connectionString);
            _logger = LogManager.GetLogger<IUserRepository>();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                _context.BuildQuery("pr_Users_ChangePassword")
                    .Parameter("@Username", username)
                    .Parameter("@OldPassword", oldPassword)
                    .Parameter("@NewPassword", newPassword)
                    .ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(string.Format("An error occured in ChangePassword method: {0}", e.Message));
            }

            return false;
        }

        public Guid CreateUser(User user)
        {
            return _context.BuildQuery<Guid>("pr_Users_Create")
                .Parameter("@Email", user.Email)
                .Parameter("@Username", user.Username)
                .Parameter("@Password", user.Password)
                .QueryScalar();
        }

        public void DeleteUser(int userId)
        {
            _context.BuildQuery("pr_Users_Delete")
                .Parameter("@UserID", userId)
                .ExecuteNonQuery();
        }

        public User Get(int id)
        {
            return _context.BuildQuery<User>("pr_Users_GetByID")
                .Parameter("@UserID", id)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }

        public User Get(Guid uid)
        {
            return _context.BuildQuery<User>("pr_Users_GetByUID")
                .Parameter("@UserUID", uid)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }

        public User GetByUsername(string username)
        {
            return _context.BuildQuery<User>("pr_Users_GetByUsername")
                .Parameter("@Username", username)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }

        public User GetByEmail(string email)
        {
            return _context.BuildQuery<User>("pr_Users_GetByEmail")
                .Parameter("@Email", email)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }

        public IEnumerable<User> GetByRole(int roleId)
        {
            return _context.BuildQuery<User>("pr_Users_GetByRole")
                .Parameter("@RoleID", roleId)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QueryMany();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.BuildQuery<User>("pr_Users_GetAll")
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QueryMany();
        }

        public void LockUser(int userId)
        {
            _context.BuildQuery("pr_Users_Lock")
                .Parameter("@UserID", userId)
                .ExecuteNonQuery();
        }

        public void UnlockUser(int userId)
        {
            _context.BuildQuery("pr_Users_Unlock")
                .Parameter("@UserID", userId)
                .ExecuteNonQuery();
        }

        public User AuthenticateUsername(string username, string password)
        {
            return _context.BuildQuery<User>("pr_Users_GetByUsernameByPassword")
                .Parameter("@Username", username)
                .Parameter("@Password", password)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }

        public User AuthenticateEmail(string email, string password)
        {
            return _context.BuildQuery<User>("pr_Users_GetByEmailByPassword")
                .Parameter("@Email", email)
                .Parameter("@Password", password)
                .Mapper(new ExpressionMapper<User>()
                    .Bind(x => x.Active, "Active")
                    .Bind(x => x.Email, "Email")
                    .Bind(x => x.Id, "ID")
                    .Bind(x => x.Password, "Password")
                    .Bind(x => x.Uid, "UID")
                    .Bind(x => x.Username, "Username")
                )
                .QuerySingle();
        }
    }
}