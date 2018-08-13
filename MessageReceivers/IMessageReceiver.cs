using System;
using SequentialMessages.Messages;

namespace SequentialMessages.MessageReceivers
{
    public interface IMessageReceiver
    {
        MessageReceiverStats GetStats();
        void Post(Message message);
        event EventHandler<Message> MessageReady;
    }
}