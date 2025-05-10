namespace Net6SampleProject.Core.DTOs
{
    public class CategoryWithProductsDto : CategoryDto
    {
        // private set, AutoMapper map edemez!
        // public set olmalı
        public List<ProductDto> Products { get; set; } = new();
    }

}
