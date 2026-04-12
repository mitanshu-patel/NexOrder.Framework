using FluentValidation;
using NexOrder.Framework.Core.Contracts;

namespace NexOrder.Framework.Core.Common
{
    public abstract class RequestHandlerBase<TCommand, TResult> : IHandler<TCommand, TResult>
         where TCommand : class
    {
        public async Task<TResult> Handle(TCommand command)
        {
            var validator = GetValidator();
            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                return GetValidationFailedResult(validationResult);
            }

            return await ExecuteCommandAsync(command);
        }

        protected abstract IValidator<TCommand> GetValidator();
        protected abstract TResult GetValidationFailedResult(FluentValidation.Results.ValidationResult validationResult);
        protected abstract Task<TResult> ExecuteCommandAsync(TCommand command);
    }
}
