using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SequentialMessages.Helpers;
using SequentialMessages.Messages;
using SequentialMessages.MessageReceivers;
using SequentialMessages.MessageSenders;

namespace SequentialMessages
{
    public class Application
    {
        private readonly ILogger _logger;
        private readonly IMessageReceiver _receiver;
        private readonly IMessageReceiverStatsPrinter _statsPrinter;
        private readonly IMessageBuilder _builder;
        private readonly IMessageSenderFactory _messageSenderFactory;
        private readonly IMessageTestBed _testBed;
        
        public Application()
        {
            _logger = new ConsoleLogger();
            _receiver = new OrderedMessageReceiver(_logger);
            _statsPrinter = new MessageReceiverStatsPrinter(_logger);
            _builder = new MessageBuilder();
            _messageSenderFactory = new MessageSenderFactory(_builder, _receiver);
            _testBed = new MessageTestBed(_logger, _receiver, _builder);
        }
        
        public async Task RunAsync()
        {
            _receiver.MessageReady += OnMessageReady;

            foreach (var sender in GetMessageSenders())
            {
                _testBed.AddSender(sender);
            }

            _testBed.StartSending(TimeSpan.FromSeconds(1));

            Console.WriteLine("Press ENTER to stop sending.");
            Console.ReadLine();

            await _testBed.StopSending();

            _statsPrinter.Print(_receiver.GetStats());
            
            Console.WriteLine("Finished. Press ENTER to exit.");
            Console.ReadLine();
        }

        private IEnumerable<IMessageSender> GetMessageSenders()
        {
            return new List<IMessageSender> {
                _messageSenderFactory.CreateSender("jane"),
                _messageSenderFactory.CreateSlowSender("bill", TimeSpan.FromMilliseconds(500)),
                _messageSenderFactory.CreateDuplicateSender("fred")
            };
        }

        private void OnMessageReady(object sender, Message message)
        {
            _logger.Info(typeof(Program).Name, $"Received {message.SeqId} from {message.Source}");
        }
    }
}