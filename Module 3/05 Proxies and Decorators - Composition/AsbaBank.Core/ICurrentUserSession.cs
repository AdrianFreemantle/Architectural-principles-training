using System.Collections.Generic;
using System.Security.Principal;

namespace AsbaBank.Core
{
    public interface ICurrentUserSession: IPrincipal
    {
        //here we can define additional properties such as user preferences, session keys etc.

        bool IsInRole(IEnumerable<string> roles);
    }
}