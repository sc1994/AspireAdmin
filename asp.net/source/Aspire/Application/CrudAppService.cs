using System;
using System.Linq;
using System.Threading.Tasks;

using Aspire.Dto;
using Aspire.Mapper;

using Microsoft.AspNetCore.Mvc;

using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace Aspire
{
    /// <summary>
    /// CRUD 服务
    /// </summary>
    /// <typeparam name="TAuditEntity">数据库审计实体</typeparam>
    public abstract class CrudAppService<
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
        where TDto : IEntityDto<TPrimaryKey>
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
        where TCreateOrUpdateDto : IEntityDto<TPrimaryKey>
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
    [DynamicWebApi]
    public abstract class CrudAppService<
        TAuditEntity,
        TPrimaryKey,
        TPageInputDto,
        TOutputDto,
        TCreateDto,
        TUpdateDto> : IDynamicWebApi
        where TAuditEntity : IAuditEntity<TPrimaryKey>
        where TPageInputDto : PageInputDto
        where TUpdateDto : IEntityDto<TPrimaryKey>
    {
        private readonly IAuditRepository<TAuditEntity, TPrimaryKey> _repository;
        private readonly IAspireMapper _mapper;

        /// <summary>
        /// 默认 构造 ，实例CRUD必须的服务
        /// </summary>
        public CrudAppService()
        {
            _repository = ServiceLocator.ServiceProvider.GetService<IAuditRepository<TAuditEntity, TPrimaryKey>>();
            _mapper = ServiceLocator.ServiceProvider.GetService<IAspireMapper>();
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
            return _mapper.MapTo<TAuditEntity, TOutputDto>(entity);
        }

        /// <summary>
        /// 映射到 数据传输对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TTargetDto MapToDto<TTargetDto>(TAuditEntity entity)
        {
            return _mapper.MapTo<TAuditEntity, TTargetDto>(entity);
        }

        /// <summary>
        /// 映射到 实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TAuditEntity MapToEntity<TSourceDto>(TSourceDto dto)
        {
            return _mapper.MapTo<TSourceDto, TAuditEntity>(dto);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<TOutputDto> CreateAsync(TCreateDto dto)
        {
            var entity = MapToEntity(dto);
            return MapToDto(await _repository.InsertThenEntityAsync(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<TOutputDto> UpdateAsync(TUpdateDto dto)
        {
            var entity = MapToEntity(dto);
            var success = await _repository.UpdateAsync(entity);
            if (success) {
                return await GetAsync(dto.Id);
            }
            throw new Exception(""); // TODO 有好的异常信息
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<TOutputDto> GetAsync(TPrimaryKey id)
        {
            return MapToDto<TOutputDto>(await _repository.GetAsync(id));
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
            var (items, totalCount) = await _repository.PagingAsync(filer, dto);
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
