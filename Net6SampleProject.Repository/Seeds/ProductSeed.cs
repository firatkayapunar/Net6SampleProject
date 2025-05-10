using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Repository.Configurations
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product { Id = 1, CategoryId = 1, Name = "Kalem 1", Price = 100, Stock = 20, CreatedDate = DateTime.Now },
                new Product { Id = 2, CategoryId = 2, Name = "Kitap 1", Price = 120, Stock = 22, CreatedDate = DateTime.Now },
                new Product { Id = 3, CategoryId = 3, Name = "Defter 1", Price = 140, Stock = 24, CreatedDate = DateTime.Now }
                );
        }
    }
}
