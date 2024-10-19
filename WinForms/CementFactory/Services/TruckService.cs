using System.Collections.Generic;
using System.Configuration;
using System.Data;
using CementFactory.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CementFactory.Services
{
    public class TruckService
    {
        private readonly string _connectionString;

        public TruckService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TruckDbConnection"].ConnectionString;
        }

        public List<Truck> GetAllTrucks()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM truck ORDER BY date DESC LIMIT 100;";
                return db.Query<Truck>(sql).AsList();
            }
        }
        
        public int AddTruck(Truck truck)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = @"
                    INSERT INTO truck
                    (plate_number, type_cargo, image_path, date, truck_status, c1_status, weight_empty, weight_difference, weight_full, cub_metr)
                    VALUES (@PlateNumber, @TypeCargo, @ImagePath, @Date, @TruckStatus, @C1Status, @WeightEmpty, @WeightDifference, @WeightFull, @CubMetr);
                    SELECT LAST_INSERT_ID();";

                return db.QuerySingle<int>(sql, new
                {
                    PlateNumber = truck.plate_number,
                    TypeCargo = truck?.type_cargo ?? "",
                    ImagePath = truck?.image_path ?? "",
                    Date = truck.Date,
                    TruckStatus = truck.truck_status,
                    C1Status = truck?.c1_status ?? "",
                    WeightEmpty = truck?.weight_empty,
                    WeightDifference = truck?.weight_difference,
                    WeightFull = truck?.weight_full,
                    CubMetr = truck?.cub_metr
                });
            }
        }

        public Truck GetTruckById(int id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM truck WHERE id = @Id";
                return db.QueryFirstOrDefault<Truck>(sql, new { Id = id });
            }
        }
        
        public void UpdateTruck(Truck truck)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = @"UPDATE truck 
                           SET plate_number = @plate_number, 
                               type_cargo = @type_cargo, 
                               image_path = @image_path, 
                               date = @Date, 
                               truck_status = @truck_status, 
                               c1_status = @c1_status, 
                               weight_empty = @weight_empty, 
                               weight_difference = @weight_difference, 
                               weight_full = @weight_full,
                               cub_metr = @cub_metr
                           WHERE id = @Id";
                db.Execute(sql, truck);
            }
        }
        
        public Truck GetLatestTruckByPlateAndStatus(string plateNumber, string status)
        {
            if (!string.IsNullOrEmpty(plateNumber) && plateNumber.Length > 2)
            {
                plateNumber = plateNumber.Substring(2);  // Remove the first 2 characters
            }
            
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = @"SELECT * FROM truck 
                       WHERE plate_number LIKE CONCAT('%', @PlateNumber, '%') 
                       AND truck_status = @Status
                       ORDER BY date DESC
                       LIMIT 1"; 

                return db.QueryFirstOrDefault<Truck>(sql, new 
                { 
                    PlateNumber = plateNumber,
                    Status = status 
                });
            }
        }
    }
}