using FluentValidation;
using Net6SampleProject.Core.DTOs;

namespace Net6SampleProject.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            // Bir alan hatalıysa diğer validasyonları boşuna çalıştırmasın diye bunu yazdık.
            CascadeMode = CascadeMode.Stop;

            // FluentValidation'da NotEmpty zaten null kontrolü de yapıyor.
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            // GreaterThan(0) veya.GreaterThanOrEqualTo(0) gibi bir kural yazdığımızda, buradaki 0 değeri FluentValidation için bir ComparisonValue olur.
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than {ComparisonValue}.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be greater than or equal to {ComparisonValue}.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}.");
        }
    }
}
