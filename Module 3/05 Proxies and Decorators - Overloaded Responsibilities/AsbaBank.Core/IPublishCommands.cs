using AsbaBank.Core.Commands;

namespace AsbaBank.Core
{
    public interface IPublishCommands
    {
        void Publish(ICommand command);
        void Subscribe(object handler);
    }
}