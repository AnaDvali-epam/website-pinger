using EPAM.WebsitePinger.Models;
using System.Collections.Concurrent;

namespace EPAM.WebsitePinger.Services;

public class WebsitePinger
{
    private readonly HttpClient _httpClient;
    private readonly ExcelLogger _logger;

    public WebsitePinger(HttpClient httpClient, ExcelLogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task PingWebsitesAsync(string[] urls)
    {
        var logQueue = new ConcurrentQueue<LogEntry>();
        int batchSize = 100;

        var pingTasks = urls.Select(url => Task.Run(async () =>
        {
            int requestCount = 0;

            while (true) 
            {
                try
                {
                    var timestamp = DateTime.UtcNow;
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    int statusCode = (int)response.StatusCode;

                    Console.WriteLine($"[{timestamp}] {url} - Ping #{++requestCount}: {statusCode}");
                    logQueue.Enqueue(new LogEntry(url, timestamp, statusCode.ToString()));
                }
                catch (Exception ex)
                {
                    var timestamp = DateTime.UtcNow;
                    Console.WriteLine($"[{timestamp}] {url} - Ping #{++requestCount}: Error - {ex.Message}");
                    logQueue.Enqueue(new LogEntry(url, timestamp, $"Error - {ex.Message}"));
                }

                if (requestCount % batchSize == 0)
                {
                    _logger.LogBatch(logQueue);
                }

                // Delay between requests for each site
                await Task.Delay(1000);
            }
        }));

        await Task.WhenAll(pingTasks);
    }
}
