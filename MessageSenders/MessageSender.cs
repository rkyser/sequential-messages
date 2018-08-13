using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;

namespace SequentialMessages.MessageSenders
{
    public class MessageSender : IMessageSender
    {
        public string SenderName { get; private set; }
        private readonly IMessageBuilder _messageBuilder;
        protected readonly IMessageReceiver _messageReceiver;

        public MessageSender(
            string senderName,
            IMessageBuilder messageBuilder,
            IMessageReceiver messageReceiver
            )
        {
            SenderName = senderName;
            _messageBuilder = messageBuilder;
            _messageReceiver = messageReceiver;
        }

        public async Task SendPeriodically(TimeSpan period, CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancelToken);

                var message = _messageBuilder.Build(SenderName);

                await PostToReceiver(message, cancelToken);
            }
        }

        protected virtual Task PostToReceiver(Message message, CancellationToken cancelToken)
        {
            _messageReceiver.Post(message);
            return Task.CompletedTask;
        }
    }
}