using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Repository.Configurations
{
    public class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasData(
                new ProductFeature { Id = 1, Color = "Kırmızı", Height = 100, Width = 30, ProductId = 1 },
                new ProductFeature { Id = 2, Color = "Mavi", Height = 120, Width = 40, ProductId = 2 }
                );
        }
    }
}
