using System.Data;
using CementFactoryAdmin.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CementFactoryAdmin.Services;

public class TruckService
{
    private readonly IDbConnection _dbConnection;

    public TruckService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public List<Truck> GetAllTrucksPagination(string from, string to, string? truckStatus, string? typeCargo, int page,
        int pageSize)
    {
        var sql = @"
        SELECT * FROM truck 
        WHERE date >= @From AND date < DATE_ADD(@To, INTERVAL 1 DAY)
        AND (@TruckStatus IS NULL OR truck_status = @TruckStatus)
        AND (@TypeCargo IS NULL OR type_cargo LIKE CONCAT('%', @TypeCargo, '%'))
        LIMIT @PageSize OFFSET @Offset";

        int offset = (page - 1) * pageSize;

        return _dbConnection.Query<Truck>(sql, new
        {
            From = from,
            To = to,
            TruckStatus = truckStatus,
            TypeCargo = typeCargo,
            PageSize = pageSize,
            Offset = offset
        }).AsList();
    }

    public List<Truck> GetAllTrucks(string from, string to, string? truckStatus, string? typeCargo)
    {
        var sql = @"
        SELECT * FROM truck 
        WHERE date >= @From AND date < DATE_ADD(@To, INTERVAL 1 DAY)
        AND (@TruckStatus IS NULL OR truck_status = @TruckStatus)
        AND (@TypeCargo IS NULL OR type_cargo LIKE CONCAT('%', @TypeCargo, '%'))";

        return _dbConnection.Query<Truck>(sql, new
        {
            From = from,
            To = to,
            TruckStatus = truckStatus,
            TypeCargo = typeCargo
        }).AsList();
    }

    public int GetTruckCount(string from, string to, string? truckStatus, string? typeCargo)
    {
        var sql = @"
        SELECT COUNT(*) FROM truck 
        WHERE date >= @From AND date < DATE_ADD(@To, INTERVAL 1 DAY)
        AND (@TruckStatus IS NULL OR truck_status = @TruckStatus)
        AND (@TypeCargo IS NULL OR type_cargo LIKE CONCAT('%', @TypeCargo, '%'))";
        
        return _dbConnection.QuerySingle<int>(sql, new
        {
            From = from,
            To = to,
            TruckStatus = truckStatus,
            TypeCargo = typeCargo
        });
    }
}