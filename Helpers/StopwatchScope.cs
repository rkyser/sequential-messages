using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SequentialMessages.Helpers
{
    public class StopwatchScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        public StopwatchScope(Stopwatch stopwatch)
        {
            _stopwatch = stopwatch;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
        }
    }
}