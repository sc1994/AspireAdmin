using System.Reflection;

using Aspire.Mapper;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

namespace Aspire.AutoMapper.Provider
{
    public class AutoMapperOptionsSetup : IAspireMapperOptionsSetup
    {
        private readonly Assembly _applicationAssembly;

        /// <summary>
        /// 初始化 auto mapper 的启动选项
        /// </summary>
        /// <param name="applicationAssembly">应用程序集，默认为启动入口的程序集</param>
        public AutoMapperOptionsSetup(Assembly applicationAssembly = null)
        {
            _applicationAssembly = applicationAssembly ?? Assembly.GetEntryAssembly();
        }

        public void AddAspireMapper(IServiceCollection services)
        {
            services.AddScoped<IAspireMapper, AspireMapper>();

            services.AddScoped(typeof(IMapper), _ => {
                return new MapperConfiguration(cfg => {
                    cfg.AddMaps(_applicationAssembly);
                }).CreateMapper();
            });
        }
    }
}
