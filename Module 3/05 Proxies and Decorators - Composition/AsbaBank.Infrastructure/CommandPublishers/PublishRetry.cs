using System;
using System.Linq;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public interface IPublishRetry
    {
        void Retry(Action<ICommand> publish, ICommand command);
    }

    public class PublishRetry : IPublishRetry
    {
        public void Retry(Action<ICommand> publish, ICommand command)
        {
            var attribute = Attribute.GetCustomAttributes(command.GetType())
                                     .FirstOrDefault(a => a is CommandRetryAttribute) as CommandRetryAttribute;

            if (attribute == null)
            {
                publish(command);
            }
            else
            {
                Core.Retry.Action(() => publish(command), attribute.RetryCount, attribute.RetryMilliseconds);
            }
        }
    }
}