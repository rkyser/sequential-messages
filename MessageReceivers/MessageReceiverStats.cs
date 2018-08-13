using System;
using System.Diagnostics;
using SequentialMessages.Helpers;

namespace SequentialMessages.MessageReceivers
{
    public class MessageReceiverStats
    {
        public int Ignored { get; set; }
        public int Futured { get; set; }
        public int Immediate { get; set; }
        public int TotalReceived { get; set; }
        public int TotalForwarded { get; set; }
        public int FastForwards { get; set; }
        public long LastSeqId { get; set; }
        public TimeSpan TotalTimeHandling => GetTotalTimeHandling();
        public TimeSpan AvgTimePerMessage => GetAvgTimePerMessage();
        private readonly Stopwatch _timeHandling;

        public MessageReceiverStats()
        {
            _timeHandling = new Stopwatch();
        }

        public TimeSpan GetTotalTimeHandling()
        {
            return _timeHandling.Elapsed;
        }

        public TimeSpan GetAvgTimePerMessage()
        {
            return _timeHandling.Elapsed / TotalForwarded;
        }

        public IDisposable StartHandling()
        {
            _timeHandling.Start();
            return new StopwatchScope(_timeHandling);
        }

        public MessageReceiverStats Copy()
        {
            return (MessageReceiverStats)MemberwiseClone();
        }
    }

}
