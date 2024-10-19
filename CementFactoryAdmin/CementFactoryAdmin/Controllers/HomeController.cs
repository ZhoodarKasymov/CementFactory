using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CementFactoryAdmin.Models;
using CementFactoryAdmin.Services;
using Microsoft.AspNetCore.Authorization;

namespace CementFactoryAdmin.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TruckService _truckService;

    public HomeController(ILogger<HomeController> logger, TruckService truckService)
    {
        _logger = logger;
        _truckService = truckService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public ActionResult GetTrucks(string from, string to, string truckStatus, string typeCargo, bool isChart, int page = 1, int pageSize = 20)
    {
        List<Truck> trucks;
        var totalTrucks = 0;
        
        if (!isChart)
        {
            trucks = _truckService.GetAllTrucksPagination(from, to, truckStatus, typeCargo, page, pageSize);
            totalTrucks = _truckService.GetTruckCount(from, to, truckStatus,typeCargo);
        }
        else
        {
            trucks = _truckService.GetAllTrucks(from, to, truckStatus,typeCargo);
        }

        return Json(new { trucks, totalTrucks, pageSize });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}