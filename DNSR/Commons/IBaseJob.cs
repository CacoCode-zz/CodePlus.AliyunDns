using System.Threading.Tasks;
using Quartz;

namespace DNSR.Commons
{
    public interface IBaseJob : IJob
    {
        Task Execute(IJobExecutionContext context);
    }
}
