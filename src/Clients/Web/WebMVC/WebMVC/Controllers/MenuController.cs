using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using WebMVC.Services.Contracts;
using WebMVC.ViewModels;

namespace WebMVC.Controllers;

public class MenuController : Controller
{
    private readonly IMenuService _menuSvc;

    public MenuController(IMenuService menuSvc)
    {
        _menuSvc = menuSvc;
    }

    [HttpGet]
    [Route("menu")]
    public async Task<IActionResult> MenuAsync()
    {
        try
        {
            var vm = await _menuSvc.GetMenu();
        
            return View(vm);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        
        return RedirectToAction("Index", "Home");
    }

    private void HandleException(Exception ex)
    {
        ViewBag.BasketInoperativeMsg = $"Menu Service is inoperative {ex.GetType().Name} - {ex.Message}";
    }
}
