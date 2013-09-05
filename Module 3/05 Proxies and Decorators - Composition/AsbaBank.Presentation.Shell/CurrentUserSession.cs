using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using AsbaBank.Core;

namespace AsbaBank.Presentation.Shell
{
    public class CurrentUserSession : ICurrentUserSession
    {
        private readonly HashSet<string> allowedRoles;
        public IIdentity Identity { get; private set; }
        

        public CurrentUserSession(IIdentity identity, params string[] roles)
        {
            Identity = identity;
            allowedRoles = new HashSet<string>(roles.Select(s => s.ToUpper()));
        }

        public bool IsInRole(string role)
        {
            return allowedRoles.Contains(role.ToUpper());
        }

        public bool IsInRole(IEnumerable<string> roles)
        {
            return roles.Any(s => allowedRoles.Contains(s.ToUpper()));
        }

        public override string ToString()
        {
            return String.Format("{0} with roles {1}", Identity.Name, String.Join(", ", allowedRoles));
        }
    }
}