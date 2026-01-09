using MediatR;

namespace Aroundu.SharedKernel.Interfaces
{
    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
    }

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {
    }
}
