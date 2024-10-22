using System.Linq;
using System.Windows.Forms;
using CementFactory.Services;

namespace CementFactory
{
    public partial class Trucks : Form
    {
        private readonly TruckService _truckService = new TruckService();

        public Trucks()
        {
            InitializeComponent();
            var trucks = _truckService.GetAllTrucks().ToList();

            // Bind the truck data to the DataGridView
            dataGridView1.DataSource = trucks;

            // Set the AutoSizeColumnsMode property to Fill
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Optionally, customize the DataGridView columns
            dataGridView1.Columns["plate_number"].HeaderText = "Номер машины";
            dataGridView1.Columns["type_cargo"].HeaderText = "Вид груза";
            dataGridView1.Columns["truck_status"].HeaderText = "Статус машины";
            dataGridView1.Columns["weight_empty"].HeaderText = "Вес пустой машины";
            dataGridView1.Columns["weight_difference"].HeaderText = "Разница веса";
            dataGridView1.Columns["cub_metr"].HeaderText = "Куб.м";
            dataGridView1.Columns["weight_full"].HeaderText = "Вес полной машины";
            dataGridView1.Columns["Date"].HeaderText = "Дата";
            dataGridView1.Columns["c1_status"].HeaderText = "Статус 1C";

            // Format the DateCreated column
            dataGridView1.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["image_path"].Visible = false;
        }
    }
}