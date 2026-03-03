using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using FluentValidation;
using MediatR;

namespace CbsAp.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse : BaseResult
    {
        private readonly IValidator<TRequest> _validators;

        public ValidationBehavior(IValidator<TRequest> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validators.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition()
                    == typeof(ResponseResult<>))
                {
                    var resultType = typeof(TResponse).GetGenericArguments()[0];
                    var resultClass = typeof(ResponseResult<>).MakeGenericType(resultType);

                    var ctorResponseResult = resultClass.GetConstructor(
                            new[] {
                                typeof(int),
                                typeof(bool),
                                resultType ,
                                typeof(List<string>)
                            }
                        );

                    if (ctorResponseResult == null)
                    {
                        throw new InvalidOperationException("Constructor not found");
                    }

                    var responseResult = ctorResponseResult.Invoke(
                        new object[] {
                             400, // bad request
                             false,
                             default!,
                             errors
                        });

                    //Result<string>Notfound()
                    return (TResponse)responseResult;
                }

                throw new InvalidOperationException("Invalid response type");
            }
            return await next();
        }
    }
}