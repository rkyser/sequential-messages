using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Helpers;

namespace SequentialMessages.MessageReceivers
{
    public interface IMessageReceiverStatsPrinter
    {
        void Print(MessageReceiverStats stats);
    }

    public class MessageReceiverStatsPrinter : IMessageReceiverStatsPrinter
    {
        private readonly ILogger _logger;

        public MessageReceiverStatsPrinter(ILogger logger)
        {
            _logger = logger;
        }

        public void Print(MessageReceiverStats stats)
        {
            foreach (var prop in stats.GetType().GetProperties()) 
            {
                _logger.Info(this, $"  {prop.Name}={prop.GetValue(stats, null)}");
            }
        }
    }
}