using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Aspire.Core.Authenticate
{
    internal class UserStore<TUserEntity> : UserStore<TUserEntity, Guid>
        where TUserEntity : class, IUserEntity
    {

    }

    internal class UserStore<TUserEntity, TPrimaryKey> : IUserStore<TUserEntity>
        where TUserEntity : class, IUserEntity<TPrimaryKey>
    {
        private readonly IAuditRepository<TUserEntity, TPrimaryKey> _repository;

        public UserStore()
        {
            _repository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TUserEntity, TPrimaryKey>>();
        }

        public void Dispose()
        {
            // TODO how to dispose
        }

        async public Task<string> GetUserIdAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Id.ToString());
        }

        async public Task<string> GetUserNameAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Account);
        }

        async public Task SetUserNameAsync(TUserEntity user, string userName, CancellationToken cancellationToken)
        {
            user.Account = userName;
            await UpdateAsync(user, cancellationToken);
        }

        async public Task<string> GetNormalizedUserNameAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await GetUserNameAsync(user, cancellationToken);
        }

        async public Task SetNormalizedUserNameAsync(TUserEntity user, string normalizedName, CancellationToken cancellationToken)
        {
            await SetUserNameAsync(user, normalizedName, cancellationToken);
        }

        async public Task<IdentityResult> CreateAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await _repository.InsertAsync(user) ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户插入失败"
                });
        }

        async public Task<IdentityResult> UpdateAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(user) ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户更新失败"
                });
        }

        async public Task<IdentityResult> DeleteAsync(TUserEntity user, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(user.Id) ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {
                    Code = "failed",
                    Description = "用户删除失败"
                });
        }

        async public Task<TUserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _repository.GetBatchAsync(x => x.Id.Equals(userId)).FirstOrDefaultAsync();
        }

        async public Task<TUserEntity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _repository.GetBatchAsync(x => x.Account == normalizedUserName).FirstOrDefaultAsync();
        }
    }
}