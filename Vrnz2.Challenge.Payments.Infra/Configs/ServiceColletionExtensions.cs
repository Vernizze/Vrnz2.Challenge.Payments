using Microsoft.Extensions.DependencyInjection;

namespace Vrnz2.Challenge.Payments.Infra.Configs
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddIServiceColletion(this IServiceCollection services)
            => services.AddSingleton(services);
    }
}
