using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SequentialMessages.Helpers
{
    public interface ILogger
    {
        void Info(object source, string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(object source, string message)
        {
            Console.WriteLine($"INFO {source.GetType().Name} {message}");
        }
    }
}