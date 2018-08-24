using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Helpers;
using SequentialMessages.Messages;

namespace SequentialMessages.MessageReceivers
{
    public class OrderedMessageReceiver : IMessageReceiver
    {
        public event EventHandler<Message> MessageReady;
        private readonly ILogger _logger;
        private readonly object _mutex;
        private readonly MessageReceiverStats _stats;
        private readonly List<Message> _outOfOrderMessages;
        private long _nextSeqId;
        
        public OrderedMessageReceiver(ILogger logger)
        {
            _logger = logger;
            _mutex = new object();
            _outOfOrderMessages = new List<Message>();
            _nextSeqId = 0;
            _stats = new MessageReceiverStats();
        }

        public void Post(Message message)
        {
            lock (_mutex)
            {
                using (_stats.StartHandling())
                {
                    HandlePostedMessage(message);
                }
            }
        }

        private void HandlePostedMessage(Message message)
        {
            _stats.TotalReceived++;

            if (IsOldMessage(message))
            {
                _stats.Ignored++;
                _logger.Info(this, $"Ignoring old message from {message.Source}.");
            }
            else if (IsOutOfOrder(message))
            {
                _stats.OutOfOrder++;
                _logger.Info(this, $"Stashing out-of-order message from {message.Source}.");
                _outOfOrderMessages.Add(message);
            }
            else
            {
                _stats.Immediate++;
                HandleNextMessage(message);
                FastForward();
            }
        }

        private bool IsOldMessage(Message message)
        {
            return message.SeqId < _nextSeqId;
        }

        private bool IsOutOfOrder(Message message)
        {
            return message.SeqId > _nextSeqId;
        }

        private bool IsNextMessage(Message message)
        {
            return message.SeqId == _nextSeqId;
        }

        private void HandleNextMessage(Message message)
        {
            _stats.TotalForwarded++;
            FireMessageReady(message);
            UpdateNextSeqId(message);
        }

        private void FastForward()
        {
            if (!_outOfOrderMessages.Any())
            {
                return;
            }

            _stats.FastForwards++;

            foreach (var message in _outOfOrderMessages.OrderBy(m => m.SeqId))
            {
                if (IsNextMessage(message))
                {
                    HandleNextMessage(message);
                    _outOfOrderMessages.Remove(message);
                }
            }
        }

        private void FireMessageReady(Message message)
        {
            MessageReady?.Invoke(this, message);
        }

        private void UpdateNextSeqId(Message message)
        {
            _stats.LastSeqId = message.SeqId;
            _nextSeqId = message.SeqId + 1;
        }

        public MessageReceiverStats GetStats()
        {
            return _stats.Copy();
        }
    }
}
