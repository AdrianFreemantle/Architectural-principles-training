namespace AsbaBank.Core.Commands
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}