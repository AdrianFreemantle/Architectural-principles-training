using System;
using System.Globalization;

namespace AsbaBank.Infrastructure
{
    internal static class LoggingExtensionMethods
    {
        private const string MessageFormat = "{0:yyyy/MM/dd HH:mm:ss.ff} - {1} - {2}";

        public static string FormatMessage(this string message, Type typeToLog, params object[] values)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                MessageFormat,
                DateTime.UtcNow,
                typeToLog.Name,
                string.Format(CultureInfo.InvariantCulture, message, values));
        }
    }
}