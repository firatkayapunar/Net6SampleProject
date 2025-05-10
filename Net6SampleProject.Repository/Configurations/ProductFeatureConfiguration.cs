using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Repository.Configurations
{
    internal class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.ToTable("ProductFeatures");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.HasOne(pf => pf.Product)
                .WithOne(p => p.ProductFeature)
                .HasForeignKey<ProductFeature>(p => p.ProductId);
        }
    }
}
