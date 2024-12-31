namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //daha sonra mapster uygulayacağım

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
