using Microsoft.Extensions.DependencyInjection;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

namespace OIdentNetLib.Infrastructure.EntityFramework;

public static class ServiceRegistration
{
    public static IServiceCollection AddOIdentNetEntityFramework(this IServiceCollection services)
    {
        services.AddScoped<IAudienceReader, AudienceReader>();
        services.AddScoped<IClientReader, ClientReader>();
        services.AddScoped<ITenantReader, TenantReader>();
        services.AddScoped<IUserReader, UserReader>();
        
        return services;
    }
}