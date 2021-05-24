using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aliyun.Acs.Alidns.Model.V20150109;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using DNSR.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DNSR.Jobs
{
    public class DnsJob : IBaseJob
    {
        private readonly ILogger<DnsJob> _logger;
        private readonly IConfiguration _configuration;

        public DnsJob(ILogger<DnsJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var currentIp = GetPublicIp();
            var client = new DefaultAcsClient(DefaultProfile.GetProfile("", _configuration["AliYun:AccessKeyId"],
                _configuration["AliYun:Secret"]));
            var domainRecords = client.GetAcsResponse(new DescribeDomainRecordsRequest
            {
                DomainName = _configuration["Dns:DomainName"],
                RRKeyWord = _configuration["Dns:RRKeyWord"]
            }).DomainRecords;

            DescribeDomainRecordsResponse.DescribeDomainRecords_Record homeRecord =
                domainRecords.First(x => x.RR == _configuration["Dns:RRKeyWord"]);
            _logger.LogInformation($"解析IP:{currentIp},当前IP:{homeRecord._Value}");
            if (homeRecord._Value != currentIp)
            {
                client.GetAcsResponse(new UpdateDomainRecordRequest
                {
                    RecordId = homeRecord.RecordId,
                    RR = homeRecord.RR,
                    Type = homeRecord.Type,
                    _Value = currentIp,
                });
                _logger.LogInformation("动态解析成功");
            }
            return Task.CompletedTask;
        }

        private string GetPublicIp()
        {
            var str = new WebClient().DownloadString("https://pv.sohu.com/cityjson");
            Console.WriteLine(str);
            var regex = new Regex(@"(\d+\.\d+\.\d+\.\d+)");
            var match = regex.Match(str);
            if (match.Success)
            {
                var ip = match.Groups[0].Value;
                return ip;
            }
            return null;
        }
    }
}
