using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Aspire
{
    /// <summary>
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    public abstract class CrudApplication<
        TAuditEntity> : CrudAppService<
        TAuditEntity,
        Guid>
        where TAuditEntity : IAuditEntity
    {

    }

    /// <summary>
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
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
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入</typeparam>
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
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入</typeparam>
    /// <typeparam name="TDto">数据传输对象</typeparam>
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
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入</typeparam>
    /// <typeparam name="TOutputDto">数据传输 输出对象</typeparam>
    /// <typeparam name="TCreateOrUpdateDto">数据传输 创建或者更新 对象</typeparam>
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
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键</typeparam>
    /// <typeparam name="TPageInputDto">数据传输对象 分页输入</typeparam>
    /// <typeparam name="TOutputDto">数据传输 输出对象</typeparam>
    /// <typeparam name="TCreateDto">数据传输 创建对象</typeparam>
    /// <typeparam name="TUpdateDto">数据传输 更新对象</typeparam>
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
        /// 当前服务仓储
        /// </summary>
        readonly protected IAuditRepository<TAuditEntity, TPrimaryKey> CurrentRepository;

        /// <summary>
        /// 默认 构造 ，实例CRUD必须的服务
        /// </summary>
        public CrudAppService()
        {
            CurrentRepository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TAuditEntity, TPrimaryKey>>();
        }

        //protected virtual  PageFilter(TPageInputDto input)
        //{
        // 多ORM似乎无法实现这一点，特别是 ORM 对 IQueryable 实现不一致时
        //}

        /// <summary>
        /// 映射到 数据传输对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TOutputDto MapToDto(TAuditEntity entity)
        {
            return MapTo<TAuditEntity, TOutputDto>(entity);
        }

        /// <summary>
        /// 映射到 数据传输对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TTargetDto MapToDto<TTargetDto>(TAuditEntity entity)
        {
            return MapTo<TAuditEntity, TTargetDto>(entity);
        }

        /// <summary>
        /// 映射到 实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TAuditEntity MapToEntity<TSourceDto>(TSourceDto dto)
        {
            return MapTo<TSourceDto, TAuditEntity>(dto);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<TOutputDto> CreateAsync(TCreateDto dto)
        {
            var entity = MapToEntity(dto);
            return MapToDto(await CurrentRepository.InsertThenEntityAsync(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await CurrentRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<TOutputDto> UpdateAsync(TUpdateDto dto)
        {
            var entity = MapToEntity(dto);
            var success = await CurrentRepository.UpdateAsync(entity);
            if (success) {
                return await GetAsync(dto.Id);
            }
            return Failure<TOutputDto>(ResponseCode.InternalServerDatabaseError, $"执行 {nameof(TAuditEntity)} 实体更新失败");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<TOutputDto> GetAsync(TPrimaryKey id)
        {
            return MapToDto<TOutputDto>(await CurrentRepository.GetAsync(id));
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<PagedResultDto<TOutputDto>> PagingAsync(TPageInputDto dto)
        {
            var filer = FilterPage(dto);
            var (items, totalCount) = await CurrentRepository.PagingAsync(filer, dto);
            return new PagedResultDto<TOutputDto>(items.Select(MapToDto), totalCount);

        }

        /// <summary>
        /// 过滤分页
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected abstract object FilterPage(TPageInputDto dto);
    }
}
