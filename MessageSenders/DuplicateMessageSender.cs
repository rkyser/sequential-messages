using System.Threading;
using System.Threading.Tasks;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;

namespace SequentialMessages.MessageSenders
{
    public class DuplicateMessageSender : MessageSender
    {
        public DuplicateMessageSender(
            string senderName,
            IMessageBuilder messageBuilder,
            IMessageReceiver messageReceiver)
            : base(senderName, messageBuilder, messageReceiver)
        {
        }
        
        protected override Task PostToReceiver(Message message, CancellationToken token)
        {
            _messageReceiver.Post(message);
            _messageReceiver.Post(message);
            return Task.CompletedTask;       
        }
    }
}