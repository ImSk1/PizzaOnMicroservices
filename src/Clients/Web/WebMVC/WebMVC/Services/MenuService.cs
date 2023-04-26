using Microsoft.Extensions.Options;
using System.Text.Json;
using WebMVC.Configuration;
using WebMVC.Infrastructure;
using WebMVC.Services.Contracts;
using WebMVC.ViewModels;

namespace WebMVC.Services;

public class MenuService : IMenuService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly HttpClient _apiClient;
    private readonly ILogger<MenuService> _logger;
    private readonly string _menuByPassUrl;

    public MenuService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<MenuService> logger)
    {
        _apiClient = httpClient;
        _settings = settings;
        _logger = logger;

        _menuByPassUrl = $"{_settings.Value.MenuUrl}/api/v1/Pizza/pizzas";
    }

    public async Task<Menu> GetMenu()
    {
        var uri = API.Menu.GetMenu(_menuByPassUrl);
        _logger.LogDebug("[GetMenu] -> Calling {Uri} to get the menu", uri);

        var response = await _apiClient.GetAsync(uri);
        _logger.LogDebug("[GetMenu] -> response code {StatusCode}", response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(responseString))
        {
            return new Menu { Items = new List<MenuItem>()};
        }

        var menuItems = JsonSerializer.Deserialize<MenuItem[]>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (menuItems == null)
        {
            return new Menu { Items = new List<MenuItem>() };
        }

        return new Menu { Items = menuItems.ToList() };
    }
}
