using System.Collections.Generic;

namespace CementFactory.Models
{
    public class ApiResponse
    {
        public List<Item> Results { get; set; }
    }
    
    public class Item
    {
        public string Name { get; set; }
        public string Guid { get; set; }
    }
}