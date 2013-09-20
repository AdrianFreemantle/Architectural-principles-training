using System;
using Entities;

namespace Shell.Domain
{
    public class LedgerId : Identity<Guid>
    {
        public LedgerId(Guid id)
            : base(id)
        {
        }
    }
}