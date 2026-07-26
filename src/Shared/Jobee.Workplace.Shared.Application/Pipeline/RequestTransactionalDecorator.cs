using System.Reflection;
using Jobee.Workplace.Shared.Application.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal sealed class RequestTransactionalDecorator<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<RequestTransactionalDecorator<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RequestTransactionalDecorator(IUnitOfWork unitOfWork, ILogger<RequestTransactionalDecorator<TRequest,
        TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var hasReturnType = typeof(TResponse) != typeof(Unit);

        var type = hasReturnType
            ? typeof(IRequestHandler<,>).MakeGenericType(typeof(TRequest), typeof(TResponse))
            : typeof(IRequestHandler<>).MakeGenericType(typeof(TRequest));

        if (TransactionalTypesDictionary.ShouldUseTransaction(type) == false)
        {
            return await next(cancellationToken);
        }

        var handlerType = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith("Jobee"))
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsAssignableTo(type));

        var hasTransactionalAttribute = handlerType?.GetCustomAttribute<TransactionalAttribute>();

        var shouldUseTransaction = hasTransactionalAttribute is not null;
        TransactionalTypesDictionary.Add(type, shouldUseTransaction);

        if (!shouldUseTransaction)
        {
            return await next(cancellationToken);
        }

        await using var scope = await _unitOfWork.BeginScopeAsync(cancellationToken);
        _logger.TransactionStarted(type.Name);

        var result = await next(cancellationToken);

        await scope.CommitAsync(cancellationToken);
        _logger.TransactionCompleted(type.Name);

        return result;
    }

}