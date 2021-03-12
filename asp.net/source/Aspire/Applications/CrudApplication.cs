// <copyright file="CrudApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Aspire.AuditEntity;
    using Aspire.AuditRepository;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    public abstract class CrudApplication<
        TAuditEntity> : CrudAppService<
        TAuditEntity,
        Guid>
        where TAuditEntity : IAuditEntity
    {
    }

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey> : CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        PageInputDto>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
    }

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto> : CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TAuditEntity>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
        where TPageInputDto : PageInputDto
    {
    }

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入.</typeparam>
    /// <typeparam name="TDto">数据传输对象.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TDto> : CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TDto,
        TDto>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
        where TPageInputDto : PageInputDto
        where TDto : IDto<TPrimaryKey>
    {
    }

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入.</typeparam>
    /// <typeparam name="TOutputDto">数据传输 输出对象.</typeparam>
    /// <typeparam name="TCreateOrUpdateDto">数据传输 创建或者更新 对象.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TOutputDto,
        TCreateOrUpdateDto> : CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TOutputDto,
        TCreateOrUpdateDto,
        TCreateOrUpdateDto>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
        where TPageInputDto : PageInputDto
        where TCreateOrUpdateDto : IDto<TPrimaryKey>
    {
    }

    /// <summary>
    /// CRUD 服务.
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体.</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入.</typeparam>
    /// <typeparam name="TOutputDto">数据传输 输出对象.</typeparam>
    /// <typeparam name="TCreateDto">数据传输 创建对象.</typeparam>
    /// <typeparam name="TUpdateDto">数据传输 更新对象.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TOutputDto,
        TCreateDto,
        TUpdateDto> : Application
        where TAuditEntity : IAuditEntity<TPrimaryKey>
        where TPageInputDto : PageInputDto
        where TUpdateDto : IDto<TPrimaryKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppService{        TAuditEntity,         TPrimaryKey,         TPageInputDto,         TOutputDto,         TCreateDto,         TUpdateDto}"/> class.
        /// </summary>
        protected CrudAppService()
        {
            this.CurrentRepository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TAuditEntity, TPrimaryKey>>();
        }

        /// <summary>
        /// Gets 当前服务仓储.
        /// </summary>
        protected IAuditRepository<TAuditEntity, TPrimaryKey> CurrentRepository { get; }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="input">Input Dto.</param>
        /// <returns>Output Dto.</returns>
        public virtual async Task<TOutputDto> CreateAsync(TCreateDto input)
        {
            var entity = this.MapToEntity(input);
            return this.MapToDto(await this.CurrentRepository.InsertThenEntityAsync(entity));
        }

        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Is Success.</returns>
        [HttpDelete("{id}")]
        public virtual async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await this.CurrentRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <returns>Output.</returns>
        public virtual async Task<TOutputDto> UpdateAsync(TUpdateDto input)
        {
            var entity = this.MapToEntity(input);
            var success = await this.CurrentRepository.UpdateAsync(entity);
            if (success)
            {
                return await this.GetAsync(input.Id);
            }

            return Failure<TOutputDto>(ResponseCode.InternalServerDatabaseError, $"执行 {nameof(TAuditEntity)} 实体更新失败");
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Output.</returns>
        [HttpGet("{id}")]
        public virtual async Task<TOutputDto> GetAsync(TPrimaryKey id)
        {
            return this.MapToDto<TOutputDto>(await this.CurrentRepository.GetAsync(id));
        }

        /// <summary>
        /// Paging.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <returns>Page Output.</returns>
        [HttpPost]
        public virtual async Task<PagedResultDto<TOutputDto>> PagingAsync(TPageInputDto input)
        {
            var filer = this.FilterPage(input);
            var (items, totalCount) = await this.CurrentRepository.PagingAsync(filer, input);
            return new PagedResultDto<TOutputDto>(items.Select(this.MapToDto), totalCount);
        }

        /// <summary>
        /// Filter Page.
        /// </summary>
        /// <param name="input">Input.</param>
        /// <returns>Queryable.</returns>
        protected abstract object FilterPage(TPageInputDto input);

        /// <summary>
        /// 映射到 数据传输对象.
        /// </summary>
        /// <param name="entity">Audit Entity.</param>
        /// <returns>Output Dto.</returns>
        protected virtual TOutputDto MapToDto(TAuditEntity entity)
        {
            return this.MapTo<TAuditEntity, TOutputDto>(entity);
        }

        /// <summary>
        /// 映射到 数据传输对象.
        /// </summary>
        /// <typeparam name="TTargetDto">Target Dto.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <returns>Target.</returns>
        protected virtual TTargetDto MapToDto<TTargetDto>(TAuditEntity entity)
        {
            return this.MapTo<TAuditEntity, TTargetDto>(entity);
        }

        /// <summary>
        /// 映射到 实体.
        /// </summary>
        /// <typeparam name="TSourceDto">Source Dto.</typeparam>
        /// <param name="dto">Dto.</param>
        /// <returns>Audit Entity.</returns>
        protected virtual TAuditEntity MapToEntity<TSourceDto>(TSourceDto dto)
        {
            return this.MapTo<TSourceDto, TAuditEntity>(dto);
        }
    }
}
