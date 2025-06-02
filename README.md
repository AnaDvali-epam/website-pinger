# Website Pinger Tool

A .NET console application to monitor website availability by sending HTTP requests (pings) to specified URLs and logging the results into an Excel file. This tool is designed to help you monitor the uptime and response codes of websites in real-time.

---

## Features

- **Ping Multiple Websites**: Concurrently ping multiple URLs.
- **Real-Time Logging**: Logs each ping immediately to an Excel file.
- **Crash Resilience**: Saves logs after each ping to prevent data loss in case of crashes.
- **Configurable Settings**: Adjust settings such as ping interval, batch size, and timeout via `appsettings.json`.
- **Excel File Output**: Creates a timestamped Excel file with detailed logs.

---

## How to Run

### Prerequisites
- .NET 6.0 or later installed on your system.
- ClosedXML NuGet package is already included in the project.

### Steps to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/AnaDvali-epam/website-pinger.git
   cd website-pinger
2. Build and Run the project
3. The application will start pinging websites specified in the appsettings.json file.

### Configuration
All settings for the application are defined in the appsettings.json file. You can customize:

- urls: Specify the websites to be pinged in the Urls section.
- BatchSize: Controls how many pings are processed before exiting the program (default: 100).
- TimeoutInSeconds: Specifies the timeout for each HTTP request.
- PingPeriod: Sets the interval (in seconds) between consecutive pings for each website (default: 60 seconds).

### Excel File Output
The Excel file is saved in the ExcelFiles folder inside the base directory of the application.

- **Debug Mode**: <ProjectDirectory>\bin\Debug\netX.X\ExcelFiles
- **Release Mode**: <ProjectDirectory>\bin\Release\netX.X\ExcelFiles
  
