using Aspire.Mapper;

using AutoMapper;

namespace Aspire.AutoMapper.Provider
{
    internal class AspireMapper : IAspireMapper
    {
        private readonly IMapper _mapper;

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
