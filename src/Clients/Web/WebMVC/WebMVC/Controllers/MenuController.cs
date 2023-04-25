using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services.Contracts;

namespace WebMVC.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly IMenuService _menuSvc;

    public MenuController(IMenuService menuSvc)
    {
        _menuSvc = menuSvc;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Menu()
    {
        try
        {
            var vm = await _menuSvc.GetMenu();

            return Ok(vm);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return RedirectToAction(nameof(Index));
    }

    private void HandleException(Exception ex)
    {
        ViewBag.BasketInoperativeMsg = $"Menu Service is inoperative {ex.GetType().Name} - {ex.Message}";
    }
}
