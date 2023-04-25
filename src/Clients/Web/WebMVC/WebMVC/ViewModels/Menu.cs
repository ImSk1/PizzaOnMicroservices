namespace WebMVC.ViewModels
{
    public record Menu
    {
        public List<MenuItem> Items { get; init; } = new List<MenuItem>();
    }
}
