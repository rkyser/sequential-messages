using System;
using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;

namespace SequentialMessages.MessageSenders
{
    public class SlowMessageSender : MessageSender
    {
        private readonly TimeSpan _sendDelay;

        public SlowMessageSender(
            TimeSpan sendDelay,
            string senderName,
            IMessageBuilder messageBuilder,
            IMessageReceiver messageReceiver)
            : base(senderName, messageBuilder, messageReceiver)
        {
            _sendDelay = sendDelay;
        }

        protected override async Task PostToReceiver(Message message, CancellationToken token)
        {
            await Task.Delay(_sendDelay, token);
            _messageReceiver.Post(message);
        }
    }
}