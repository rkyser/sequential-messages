namespace SequentialMessages.Messages
{
    public class Message
    {
        public long SeqId { get; private set; }
        public string Source { get; private set; }

        public Message(long seqId, string source)
        {
            SeqId = seqId;
            Source = source;
        }
    }
}
