using WebMVC.ViewModels;

namespace WebMVC.Services.Contracts;

public interface IMenuService
{
    public Task<Menu> GetMenu();
}
