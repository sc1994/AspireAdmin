// <copyright file="AutoMapperOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.AutoMapper.Provider
{
    using System.Reflection;
    using Aspire.Mapper;
    using global::AutoMapper;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// AutoMapper Options Setup.
    /// </summary>
    public class AutoMapperOptionsSetup : IAspireMapperOptionsSetup
    {
        private readonly Assembly applicationAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperOptionsSetup"/> class.
        /// </summary>
        /// <param name="applicationAssembly">应用程序集，默认为启动入口的程序集.</param>
        public AutoMapperOptionsSetup(Assembly applicationAssembly = null)
        {
            this.applicationAssembly = applicationAssembly ?? Assembly.GetEntryAssembly();
        }

        /// <inheritdoc/>
        public void AddAspireMapper(IServiceCollection services)
        {
            services.AddSingleton(_ =>
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(this.applicationAssembly);
                    this.applicationAssembly
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
