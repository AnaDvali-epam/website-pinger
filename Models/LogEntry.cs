namespace EPAM.WebsitePinger.Models;

public class LogEntry
{
    public string Url { get; }
    public DateTime Timestamp { get; }
    public string ResponseCode { get; }

    public LogEntry(string url, DateTime timestamp, string responseCode)
    {
        Url = url;
        Timestamp = timestamp;
        ResponseCode = responseCode;
    }
}
