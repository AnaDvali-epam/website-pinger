using EPAM.WebsitePinger.Models;
using System.Collections.Concurrent;

namespace EPAM.WebsitePinger.Services;

public class WebsitePinger
{
    private readonly HttpClient _httpClient;
    private readonly ExcelLogger _logger;
    private readonly int _batchSize;
    private readonly int _pingPeriod;
    private int _totalPings;

    public WebsitePinger(HttpClient httpClient, ExcelLogger logger, int batchSize, int pingPeriod)
    {
        _httpClient = httpClient;
        _logger = logger;
        _batchSize = batchSize;
        _pingPeriod = pingPeriod;
        _totalPings = 0;
    }

    public async Task PingWebsitesAsync(string[] urls)
    {
        var logQueue = new ConcurrentQueue<LogEntry>();

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
                    _logger.LogRow(url, timestamp, statusCode.ToString()); 
                }
                catch (Exception ex)
                {
                    var timestamp = DateTime.UtcNow;
                    Console.WriteLine($"[{timestamp}] {url} - Ping #{++requestCount}: Error - {ex.Message}");
                    _logger.LogRow(url, timestamp, $"Error - {ex.Message}");
                }

                Interlocked.Increment(ref _totalPings);

                if (_totalPings >= _batchSize)
                {
                    Console.WriteLine("Batch size reached. Exiting program...");
                    Environment.Exit(0); 
                }

                // Wait for the ping period (default: 1 minute)
                await Task.Delay(_pingPeriod * 1000);
            }
        }));

        await Task.WhenAll(pingTasks);
    }
}
