using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;
public class ValidationBehavior<Trequest, Tresponse>
    (IEnumerable<IValidator<Trequest>> validators)
    : IPipelineBehavior<Trequest, Tresponse>
    where Trequest : ICommand<Tresponse>, new()
{
    public async Task<Tresponse> Handle(Trequest request, RequestHandlerDelegate<Tresponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<Trequest>(request);

        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }
        return await next();
    }
}
