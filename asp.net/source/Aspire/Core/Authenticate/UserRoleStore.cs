using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Aspire.Core.Authenticate
{
    internal class UserRoleStore<TUserRoleEntity> : RoleStore<TUserRoleEntity, Guid>
        where TUserRoleEntity : class, IUserRoleEntity
    {

    }

    internal class RoleStore<TUserRoleEntity, TPrimaryKey> : IRoleStore<TUserRoleEntity>
        where TUserRoleEntity : class, IUserRoleEntity<TPrimaryKey>
    {
        private readonly IAuditRepository<TUserRoleEntity, TPrimaryKey> _repository;

        public RoleStore()
        {
            _repository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserRoleEntity, TPrimaryKey>>();
        }

        public void Dispose()
        {
            // TODO how to dispose
        }

        async public Task<IdentityResult> CreateAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await _repository.InsertAsync(role)
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户角色插入失败"
                });
        }

        async public Task<IdentityResult> UpdateAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(role)
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户角色更新失败"
                });
        }

        async public Task<IdentityResult> DeleteAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(role.Id)
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户角色删除失败"
                });
        }

        async public Task<string> GetRoleIdAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Id.ToString());
        }

        async public Task<string> GetRoleNameAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.RoleName);
        }

        async public Task SetRoleNameAsync(TUserRoleEntity role, string roleName, CancellationToken cancellationToken)
        {
            role.RoleName = roleName;
            await UpdateAsync(role, cancellationToken);
        }

        async public Task<string> GetNormalizedRoleNameAsync(TUserRoleEntity role, CancellationToken cancellationToken)
        {
            return await GetRoleNameAsync(role, cancellationToken);
        }

        async public Task SetNormalizedRoleNameAsync(TUserRoleEntity role, string normalizedName, CancellationToken cancellationToken)
        {
            await SetRoleNameAsync(role, normalizedName, cancellationToken);
        }

        async public Task<TUserRoleEntity> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _repository.GetBatchAsync(x => x.Id.Equals(roleId), 1).FirstOrDefaultAsync();
        }

        async public Task<TUserRoleEntity> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _repository.GetBatchAsync(x => x.RoleName == normalizedRoleName).FirstOrDefaultAsync();
        }
    }
}