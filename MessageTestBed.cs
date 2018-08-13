using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;
using SequentialMessages.MessageSenders;
using SequentialMessages.Helpers;

namespace SequentialMessages
{
    public interface IMessageTestBed
    {
        void AddSender(IMessageSender sender);
        void StartSending(TimeSpan period);
        Task StopSending();
    }

    public class MessageTestBed : IMessageTestBed
    {
        private readonly ILogger _logger;
        private readonly IMessageReceiver _messageReceiver;
        private readonly IMessageBuilder _messageBuilder;
        private readonly List<IMessageSender> _senders;
        private readonly List<Task> _periodicSendTasks;
        private CancellationTokenSource _stopSendingTokenSource;

        public MessageTestBed(
            ILogger logger, 
            IMessageReceiver messageReceiver,
            IMessageBuilder messageBuilder
            )
        {
            _logger = logger;
            _messageReceiver = messageReceiver;
            _messageBuilder = messageBuilder;
            _senders = new List<IMessageSender>();
            _periodicSendTasks = new List<Task>();
        }

        public void AddSender(IMessageSender sender)
        {
            _logger.Info(this, $"Adding {sender.GetType().Name} sender '{sender.SenderName}'");
            _senders.Add(sender);
        }

        public void StartSending(TimeSpan period)
        {
            _logger.Info(this, "Starting message senders...");

            _stopSendingTokenSource = new CancellationTokenSource();
            foreach (var sender in _senders)
            {
                var periodicSendTask = sender.SendPeriodically(period, _stopSendingTokenSource.Token);
                _periodicSendTasks.Add(periodicSendTask);
            }
        }

        public async Task StopSending()
        {
            _logger.Info(this, "Stopping message senders...");

            try
            {
                _stopSendingTokenSource.Cancel();
                await Task.WhenAll(_periodicSendTasks);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _periodicSendTasks.Clear();
            }
        } 
    }
}