namespace WebMVC.ViewModels
{
    public record MenuItem
    {
        public int Version { get; init; }

        public Guid Id { get; init; }

        public string Name { get; init; } = null!;

        public List<string> Ingredients { get; init; } = null!;

        public bool InStock { get; init; }

        public double Cost { get; init; }
    }
}
