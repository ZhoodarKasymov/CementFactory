namespace CementFactoryAdmin.Models;

public class Truck
{
    public int Id { get; set; }
    public string plate_number { get; set; }
    public string type_cargo { get; set; }
    public string image_path { get; set; }
    public DateTime Date { get; set; }
    public string truck_status { get; set; }
    public string c1_status { get; set; }
    public double weight_empty { get; set; }
    public double weight_difference { get; set; }
    public double weight_full { get; set; }
    public string cub_metr { get; set; }
}