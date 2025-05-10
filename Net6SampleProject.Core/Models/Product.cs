namespace Net6SampleProject.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int ProductFeatureId { get; set; }
        public ProductFeature ProductFeature { get; set; } = null!;
    }
}
