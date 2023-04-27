using System.Text.Json.Serialization;

namespace Web.BFF.Models
{
    public class Pizza
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<string> Ingredients { get; set; } = null!;

        public bool InStock { get; set; }

        public double Cost { get; set; }
    }
}
