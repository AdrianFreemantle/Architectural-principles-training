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
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserSession currentUser;
        private readonly HashSet<object> handlers;
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(CommandPublisher));

        public CommandPublisher(IUnitOfWork unitOfWork, ICurrentUserSession currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            handlers = new HashSet<object>();
        }       

        public virtual void Publish(ICommand command)
        {
            try
            {
                Logger.Verbose("Publishing command {0}", command.GetType().Name);
                Authorize(command);
                RetryPublish(command);
                Logger.Verbose("Completed publishing command {0}", command.GetType().Name);
            }
            catch (Exception ex)
            {
                Logger.Error("Error: {0}", ex.Message);
                throw;
            }
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
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private void RetryPublish(ICommand command)
        {
            var attribute = Attribute.GetCustomAttributes(command.GetType())
                                     .FirstOrDefault(a => a is CommandRetryAttribute) as CommandRetryAttribute;

            if (attribute == null)
            {
                TryPublish(command);
            }
            else
            {
                var publishAction = new Action(() => TryPublish(command));
                Retry.Action(publishAction, attribute.RetryCount, attribute.RetryMilliseconds);
            }
        }

        public virtual void Subscribe(object handler)
        {
            handlers.Add(handler);

            foreach (var type in GetCommandHandlerTypes(handler))
            {
                Logger.Verbose("Subscribed handler for command {0}", type.GenericTypeArguments[0].Name);
            }
        }

        private void Authorize(ICommand command)
        {
            var authorizeAttribute = Attribute.GetCustomAttributes(command.GetType())
                .FirstOrDefault(a => a is CommandAuthorizeAttribute) as CommandAuthorizeAttribute;

            if (authorizeAttribute == null || currentUser.IsInRole(authorizeAttribute.Roles))
            {
                return;
            }

            string message = String.Format("User {0} attempted to execute command {1} which requires roles {2}",
                                           currentUser, command.GetType().Name, authorizeAttribute);

            throw new Exception(message);
        }

        private static IEnumerable<Type> GetCommandHandlerTypes(object handler)
        {
            return handler.GetType()
                          .GetInterfaces()
                          .Where(x =>
                                 x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == typeof(IHandleCommand<>));
        }
    }
}
