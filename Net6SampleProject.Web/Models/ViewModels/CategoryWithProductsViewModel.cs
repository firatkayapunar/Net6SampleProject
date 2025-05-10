namespace Net6SampleProject.MVC.Models.ViewModels
{
    public class CategoryWithProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<ProductViewModel> Products { get; set; } = new();
    }
}
