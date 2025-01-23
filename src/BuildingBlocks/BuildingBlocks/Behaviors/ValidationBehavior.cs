using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
    //sadece command içeren crud işlemlerini kapsıyor yani delete update create gibi
    public class ValidationBehavior<TRequest, TResponse>
         (IEnumerable<IValidator<TRequest>> validators) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //fluentvalidation doğrulama işlemi sırasında ihtiyaç duyduğu bir nesnedir. Gelen isteği(TRequest) doğrulamak için kullanılır.
            var context = new ValidationContext<TRequest>(request);

            var validationResults= await Task.WhenAll(validators.Select(v=>v.ValidateAsync(context, cancellationToken)));

            var failures= validationResults
                .Where(r=>r.Errors.Any())
                .SelectMany(r=>r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next();
        }
    }
}
