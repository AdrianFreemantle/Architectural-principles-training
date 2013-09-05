using System;

namespace AsbaBank.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandRetryAttribute : Attribute
    {
        public int RetryCount { get; set; }
        public int RetryMilliseconds { get; set; }
    }


}