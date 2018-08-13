using System;
using System.Threading;
using System.Threading.Tasks;

namespace SequentialMessages.MessageSenders
{
    public interface IMessageSender
    {
        string SenderName { get; }
        Task SendPeriodically(TimeSpan period, CancellationToken stopToken);
    }
}