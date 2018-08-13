using System;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;

namespace SequentialMessages.MessageSenders
{
    public interface IMessageSenderFactory
    {
        IMessageSender CreateSlowSender(string senderName, TimeSpan sendDelay);
        IMessageSender CreateDuplicateSender(string senderName);
        IMessageSender CreateSender(string senderName);
    }

    public class MessageSenderFactory : IMessageSenderFactory
    {
        private readonly IMessageBuilder _messageBuilder;
        private readonly IMessageReceiver _messageReceiver;
        public MessageSenderFactory(
            IMessageBuilder messageBuilder,
            IMessageReceiver messageReceiver
            )
        {
            _messageBuilder = messageBuilder;
            _messageReceiver = messageReceiver;
        }

        public IMessageSender CreateSender(string senderName)
        {
            return new MessageSender(senderName, _messageBuilder, _messageReceiver);
        }

        public IMessageSender CreateDuplicateSender(string senderName)
        {
            return new DuplicateMessageSender(senderName, _messageBuilder, _messageReceiver);
        }

        public IMessageSender CreateSlowSender(string senderName, TimeSpan sendDelay)
        {
            return new SlowMessageSender(sendDelay, senderName, _messageBuilder, _messageReceiver);
        }
    }
}
