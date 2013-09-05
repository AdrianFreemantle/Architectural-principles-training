using System;
using System.Collections.Generic;
using System.Linq;

namespace AsbaBank.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAuthorizeAttribute : Attribute
    {
        public HashSet<string> Roles { get; private set; }

        public CommandAuthorizeAttribute(string role)
        {
            Roles = new HashSet<string>
            {
                role
            };
        }

        public CommandAuthorizeAttribute(params string[] roles)
        {
            Roles = new HashSet<string>(roles);
        }

        public override string ToString()
        {
            return String.Join(", ", Roles.Select(s => s.ToUpper()));
        }
    }
}
