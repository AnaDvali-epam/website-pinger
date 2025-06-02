using EPAM.WebsitePinger.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .Build();

var urls = configuration.GetSection("Urls").Get<string[]>();   
if (urls == null || urls.Length == 0)
{
    Console.WriteLine("No URLs found in the configuration file.");
    return;
}

var batchSize = configuration.GetValue<int>("PingSettings:BatchSize", 100);
var timeoutInSeconds = configuration.GetValue<int>("PingSettings:TimeoutInSeconds", 10);
var pingPeriod = configuration.GetValue<int>("PingSettings:PingPeriod", 60);

Console.WriteLine("Starting to ping websites...");

var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutInSeconds) };
var logger = ExcelLogger.Instance;

var pinger = new WebsitePinger(httpClient, logger, batchSize, pingPeriod);
await pinger.PingWebsitesAsync(urls);

Console.WriteLine("Pinging completed! Check the generated Excel file for results.");
