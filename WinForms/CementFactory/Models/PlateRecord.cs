using System;

namespace CementFactory.Models
{
    public class PlateRecord
    {
        public string PlateNumber { get; set; }
        public string Time { get; set; }
        public string Error { get; set; }  // Optional: Handle errors in the data
        
        // Parsed DateTime property based on the Time string
        public DateTime ParsedTime => DateTime.TryParse(Time, out var result) ? result : DateTime.MinValue;
    }
}