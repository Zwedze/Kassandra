using System;
using System.Collections.Generic;
using Common.Logging;
using Kassandra.Connector.Sql.Extensions;
using Kassandra.Connector.Sql.Factories;
using Kassandra.Core;
using Kassandra.Core.Components;
using Kassandra.Users.Core;
using Kassandra.Users.Core.Models;

namespace Kassandra.Users.Sql
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly IContext _context;
        private readonly ILog _logger;

        public RoleRepository(string connectionString)
        {
            _context = SqlContextFactory.Instance.GetContext(connectionString);
            _logger = LogManager.GetLogger<IUserRepository>();
        }

        public void AddUserTo(int roleId, int userId)
        {
            AddUsersTo(roleId, new List<int> {userId});
        }

        public void AddUsersTo(int roleId, IEnumerable<int> userIds)
        {
            _context.BuildQuery("pr_Roles_AddUsersTo")
                .Parameter("@UserIDTable", userIds.ToIdTable("ID"))
                .Parameter("@RoleID", roleId)
                .ExecuteNonQuery();
        }

        public void RemoveUserTo(int roleId, int userId)
        {
            RemoveUsersTo(roleId, new List<int> {userId});
        }

        public void RemoveUsersTo(int roleId, IEnumerable<int> userIds)
        {
            _context.BuildQuery("pr_Roles_RemoveUsersTo")
                .Parameter("@UserIDTable", userIds.ToIdTable("ID"))
                .Parameter("@RoleID", roleId)
                .ExecuteNonQuery();
        }

        public int CreateRole(Role role)
        {
            return _context.BuildQuery<int>("pr_Roles_Create")
                .Parameter("@Name", role.Name)
                .QueryScalar();
        }

        public void DeleteRole(int roleId)
        {
            _context.BuildQuery("pr_Roles_Delete")
                .Parameter("@ID", roleId)
                .ExecuteNonQuery();
        }

        public Role Get(int roleId)
        {
            return _context.BuildQuery<Role>("pr_Roles_GetByID")
                .Parameter("@ID", roleId)
                .Mapper(new ExpressionMapper<Role>(
                    new MappingItem<Role>(x => x.Id, "ID"),
                    new MappingItem<Role>(x => x.Uid, "UID"),
                    new MappingItem<Role>(x => x.Name, "Name")
                ))
                .QuerySingle();
        }

        public Role Get(Guid roleUid)
        {
            return _context.BuildQuery<Role>("pr_Roles_GetByUID")
                .Parameter("@Uid", roleUid)
                .Mapper(new ExpressionMapper<Role>(
                    new MappingItem<Role>(x => x.Id, "ID"),
                    new MappingItem<Role>(x => x.Uid, "UID"),
                    new MappingItem<Role>(x => x.Name, "Name")
                ))
                .QuerySingle();
        }

        public IEnumerable<Role> GetForUser(int userId)
        {
            return _context.BuildQuery<Role>("pr_Roles_GetByUserID")
                .Parameter("@UserID", userId)
                .Mapper(new ExpressionMapper<Role>(
                    new MappingItem<Role>(x => x.Id, "ID"),
                    new MappingItem<Role>(x => x.Uid, "UID"),
                    new MappingItem<Role>(x => x.Name, "Name")
                ))
                .QueryMany();
        }

        public bool IsUserInRole(int userId, int roleId)
        {
            return _context.BuildQuery<bool>("pr_Roles_IsUserInRole")
                .Parameter("@UserID", userId)
                .Parameter("@RoleID", roleId)
                .QueryScalar();
        }
    }
}