using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventDriven
{
    class Program
    {
        static void Main(string[] args)
        {
            Eventer eventer = new Eventer();


        }
    }

    public class SomeEvent : DomainEvent
    {
    }

    public class OtherEvent : DomainEvent
    {
        
    }


    public class Eventer
    {
        void When(SomeEvent e)
        {
            Console.WriteLine("SomeEvent");
        }

        void When(OtherEvent e)
        {
            Console.WriteLine("OtherEvent");
        }
    }

    public class EventHandlers
    {
        public Dictionary<Type, Action<object>> GetEventHandlers(object target)
        {
            Type EventBaseType = typeof (IDomainEvent);

            var eventHandlers = new Dictionary<Type, Action<object>>();

            var targetType = target.GetType();

            var methodsToMatch = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var matchedMethods = (from method in methodsToMatch
                                 let parameters = method.GetParameters()
                                 where
                                    method.Name.Equals("When", StringComparison.InvariantCulture) &&
                                    parameters.Length == 1 &&
                                    EventBaseType.IsAssignableFrom(parameters[0].ParameterType)
                                 select
                                    new { MethodInfo = method, FirstParameter = method.GetParameters()[0] });

            foreach (var method in matchedMethods)
            {
                var methodCopy = method.MethodInfo;
                Type firstParameterType = methodCopy.GetParameters().First().ParameterType;
                var invokeAction = InvokeAction(methodCopy);
                eventHandlers.Add(firstParameterType, invokeAction);
            }

            return eventHandlers;
        }

        private Action<object> InvokeAction(MethodInfo methodCopy)
        {
            Action<object> invokeAction = (e) => methodCopy.Invoke(this, new[] {e});
            return invokeAction;
        }
    }

}
