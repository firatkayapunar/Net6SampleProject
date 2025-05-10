namespace Net6SampleProject.MVC.Models.ViewModels
{
    public class ProductsWithCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
