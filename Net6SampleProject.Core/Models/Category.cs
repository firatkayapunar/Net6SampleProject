namespace Net6SampleProject.Core.Models
{
    public class Category : BaseEntity
    {
        private readonly List<Product> _products;

        public Category()
        {
            _products = new List<Product>();
        }

        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products => _products;
    }
}
