using System;
using System.Collections.Generic;

namespace AsbaBank.Presentation.Shell
{
    public abstract class ConsoleView<T> : IConsoleView
    {
        public abstract string Key { get; }
        public abstract string Usage { get; }
        public abstract void Print(string[] args);

        protected abstract string GetLine(T item);
        protected abstract string GetHeading();

        protected virtual void Print(IEnumerable<T> model)
        {
            PrintBorder();
            Console.WriteLine(GetHeading());
            PrintBorder();

            foreach (var item in model)
            {
                Console.WriteLine(GetLine(item));
            }

            PrintBorder();
        }

        private void PrintBorder()
        {
            Console.WriteLine("=======================================================================================");
        }
    }
}