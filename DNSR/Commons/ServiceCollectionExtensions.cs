using DNSR.Jobs;
using LBON.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DNSR.Commons
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new JobSchedule(typeof(DnsJob), configuration[$"{nameof(DnsJob)}:Cron"], configuration[$"{nameof(DnsJob)}:IsEnable"].IsNullOrEmpty() || bool.Parse(configuration[$"{nameof(DnsJob)}:IsEnable"])));
        }
    }
}
