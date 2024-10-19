using System;
using System.Globalization;
using System.Windows.Forms;
using Dapper;
using Serilog;

namespace CementFactory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Global exception handling
            Application.ThreadException += (sender, args) => HandleGlobalException(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => HandleGlobalException(args.ExceptionObject as Exception);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
            // Close and flush logs on exit
            Log.CloseAndFlush();
        }
        
        private static void HandleGlobalException(Exception exception)
        {
            if (exception == null) return;
            Log.Fatal(exception, "An unhandled exception occurred");
            MessageBox.Show("Произошла непредвиденная ошибка. Подробности смотрите в логгах.", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        // Method to convert "field_field" to "FieldField"
        private static string ConvertToPascalCase(string columnName)
        {
            // Split the column name by underscores
            var words = columnName.Split('_');

            // Capitalize the first letter of each word
            for (var i = 0; i < words.Length; i++)
            {
                words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
            }

            // Join the words back together without underscores
            return string.Join("", words);
        }
    }
}