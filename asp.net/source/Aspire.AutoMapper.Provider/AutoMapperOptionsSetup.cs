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
            this._applicationAssembly = applicationAssembly ?? Assembly.GetEntryAssembly();
        }

        public void AddAspireMapper(IServiceCollection services)
        {
            services.AddSingleton(_ =>
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(this._applicationAssembly);
                    this._applicationAssembly
                        .GetTypes()
                        .ForEach(type =>
                        {
                            var mapperCase = type.GetCustomAttribute<MapperProfileAttribute>();
                            if (mapperCase is not null)
                            {
                                cfg.CreateProfile($"{type.FullName}_mutually_{mapperCase.Type.FullName}", profileConfig =>
                                {
                                    profileConfig.CreateMap(type, mapperCase.Type).ReverseMap();
                                });
                            }
                        });

                }).CreateMapper();
            });

            services.AddScoped<IAspireMapper, AspireMapper>();
        }
    }
}
