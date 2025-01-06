using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductCommandHandler(IDocumentSession documentSession, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProduct {@Command}", command);
            var product = await documentSession.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            documentSession.Update(product);
            await documentSession.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
