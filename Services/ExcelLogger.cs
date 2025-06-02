using ClosedXML.Excel;

namespace EPAM.WebsitePinger.Services;

public class ExcelLogger : IDisposable
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

        // Save the initial Excel file
        Save();
    }

    public static ExcelLogger Instance => _instance.Value;

    /// <summary>
    /// Logs a single row to the Excel file.
    /// </summary>
    public void LogRow(string url, DateTime timestamp, string responseCode)
    {
        lock (_lock)
        {
            _worksheet.Cell(_currentRow, 1).Value = url;
            _worksheet.Cell(_currentRow, 2).Value = timestamp.ToString("o"); // ISO 8601 format
            _worksheet.Cell(_currentRow, 3).Value = responseCode;
            _currentRow++;

            // Save the file after each row is added
            Save();
        }
    }

    /// <summary>
    /// Saves the Excel file.
    /// </summary>
    private void Save()
    {
        try
        {
            _workbook.SaveAs(_filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save Excel file: {ex.Message}");
        }
    }

    /// <summary>
    /// Disposes of the workbook resources.
    /// </summary>
    public void Dispose()
    {
        _workbook.Dispose();
    }
}
