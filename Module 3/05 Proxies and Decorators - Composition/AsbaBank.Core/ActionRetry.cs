using System;
using System.Threading;

namespace AsbaBank.Core
{
    public static class Retry
    {
        public static void Action(Action action, int retryAttempts, int retryMilliseconds)
        {
            Action(action, (ex) => { }, retryAttempts, retryMilliseconds);
        }

        public static void Action(Action action, Action<Exception> onError, int retryAttempts, int retryMilliseconds)
        {
            Mandate.ParameterNotNull(action, "action");

            do
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    onError(ex);

                    if (retryAttempts <= 0)
                    {
                        throw;
                    }

                    Thread.Sleep(retryMilliseconds);
                }
            } while (retryAttempts-- > 0);
        }
    }
}
