using System;
using System.Collections.Generic;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Infrastructure.Logging;

namespace AsbaBank.Infrastructure.CommandPublishers
{ 
    public class CommandPublisher : IPublishCommands
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(CommandPublisher));
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandAuthorizer commandAuthorizer;
        private readonly IPublishRetry commandRetry;
        private readonly HashSet<object> handlers;

        public CommandPublisher(IUnitOfWork unitOfWork, ICommandAuthorizer commandAuthorizer, IPublishRetry commandRetry)
        {
            this.unitOfWork = unitOfWork;
            this.commandAuthorizer = commandAuthorizer;
            this.commandRetry = commandRetry;
            handlers = new HashSet<object>();
        }

        public virtual void Subscribe(object handler)
        {
            handlers.Add(handler);
        }

        public virtual void Publish(ICommand command) 
        {
            Logger.Verbose("Authorizing command {0}", command.GetType().Name);

            commandAuthorizer.Authorize(command);

            Logger.Verbose("Publishing command {0}", command.GetType().Name);

            commandRetry.Retry(TryPublish, command); 
        }

        private void TryPublish(ICommand command)
        {
            Type handlerGenericType = typeof(IHandleCommand<>);
            Type handlerType = handlerGenericType.MakeGenericType(new[] { command.GetType() });
            object handler = handlers.Single(handlerType.IsInstanceOfType);

            try
            {
                ((dynamic)handler).Execute((dynamic)command);
                unitOfWork.Commit();

                Logger.Verbose("Published command {0}", command.GetType().Name);
            }
            catch (Exception ex)
            {
                Logger.Verbose("Error: {0}", ex.Message);
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
