using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession documentSession) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);
            
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,

            };
            //veritabanı işlemleri documentssession kullanarak
            documentSession.Store(product);
            await documentSession.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}
