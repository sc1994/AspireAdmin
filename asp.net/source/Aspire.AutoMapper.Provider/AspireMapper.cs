using Aspire.Mapper;

using AutoMapper;

namespace Aspire.AutoMapper.Provider
{
    public class AspireMapper : IAspireMapper
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// 空参构造，由 ServiceLocator 创建 AutoMapper 实例
        /// </summary>
        public AspireMapper()
        {
            _mapper = ServiceLocator.ServiceProvider.GetService<IMapper>();
        }

        /// <summary>
        /// 由 di 实例此构造函数
        /// </summary>
        /// <param name="mapper"></param>
        public AspireMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TTarget MapTo<TTarget>(object source)
        {
            return _mapper.Map<TTarget>(source);
        }

        public TTarget MapTo<TSource, TTarget>(TSource source)
        {
            return _mapper.Map<TSource, TTarget>(source);
        }
    }
}
