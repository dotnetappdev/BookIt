using System.Diagnostics;
using BookIt.Web.Models;
using BookIt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers;

public class HomeController : Controller
{
    private readonly BookItApiClient _apiClient;

    public HomeController(BookItApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Setup()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
