using Net6SampleProject.Core.DTOs;

public class ProductsWithCategoryDto : ProductDto
{
    public CategoryDto Category { get; set; } = new();
}
