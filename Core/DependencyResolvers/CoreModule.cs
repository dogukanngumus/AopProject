using System.Diagnostics;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.Utilities.IoC;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers;

public class CoreModule:ICoreModule
{
    public void Load(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddSingleton<ICacheManager, RedisCacheManager>();
        services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
        services.AddSingleton<Stopwatch>();
    }
}