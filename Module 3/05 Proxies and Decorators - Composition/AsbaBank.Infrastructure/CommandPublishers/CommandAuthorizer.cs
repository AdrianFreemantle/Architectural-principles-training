using System;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandPublishers
{
    public interface ICommandAuthorizer
    {
        void Authorize(ICommand command);
    }

    public class CommandAuthorizer : ICommandAuthorizer
    {
        private readonly ICurrentUserSession currentUser;

        public CommandAuthorizer(ICurrentUserSession currentUser)
        {
            this.currentUser = currentUser;
        }

        public void Authorize(ICommand command)
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
    }
}