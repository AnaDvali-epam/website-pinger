using ClosedXML.Excel;
using EPAM.WebsitePinger.Models;
using System.Collections.Concurrent;

namespace EPAM.WebsitePinger.Services;

public class ExcelLogger
{
    private static readonly Lazy<ExcelLogger> _instance = new(() => new ExcelLogger());
    private readonly string _folderPath;
    private readonly string _filePath;
    private readonly object _lock = new();

    private XLWorkbook _workbook;
    private IXLWorksheet _worksheet;
    private int _currentRow;

    private ExcelLogger()
    {
        _folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFiles");

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        _filePath = Path.Combine(_folderPath, $"PingLog_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx");
        _workbook = new XLWorkbook();
        _worksheet = _workbook.Worksheets.Add("Logs");
        _currentRow = 1;

        _worksheet.Cell(_currentRow, 1).Value = "URL";
        _worksheet.Cell(_currentRow, 2).Value = "Timestamp";
        _worksheet.Cell(_currentRow, 3).Value = "Response Code";
        _currentRow++;
    }

    public static ExcelLogger Instance => _instance.Value;

    public void LogBatch(ConcurrentQueue<LogEntry> logQueue)
    {
        lock (_lock)
        {
            while (logQueue.TryDequeue(out var entry))
            {
                _worksheet.Cell(_currentRow, 1).Value = entry.Url;
                _worksheet.Cell(_currentRow, 2).Value = entry.Timestamp.ToString("o"); 
                _worksheet.Cell(_currentRow, 3).Value = entry.ResponseCode;
                _currentRow++;
            }

            _workbook.SaveAs(_filePath);
        }
    }
}
