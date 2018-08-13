namespace SequentialMessages.Messages
{
    public interface IMessageBuilder
    {
        Message Build(string source);
    }
    
    public class MessageBuilder : IMessageBuilder
    {
        private long _seqId;

        public MessageBuilder()
        {
            _seqId = 0;
        }

        public Message Build(string source)
        {
            var nextSeqId = NextSequenceId();
            return new Message(nextSeqId, source);
        }

        private long NextSequenceId()
        {
            return _seqId++;
        }
    }
}
