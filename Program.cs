using EPAM.WebsitePinger.Services;

if (args.Length == 0)
{
    Console.WriteLine("Please provide one or more website URLs as arguments.");
    return;
}

Console.WriteLine("Starting to ping websites...");

var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
var logger = ExcelLogger.Instance;

var pinger = new WebsitePinger(httpClient, logger);
await pinger.PingWebsitesAsync(args);

Console.WriteLine("Pinging completed! Check the generated Excel file for results.");
