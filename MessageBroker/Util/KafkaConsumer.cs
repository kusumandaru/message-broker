using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using MessageBroker.Log;
using MessageBroker.Model;

namespace MessageBroker.Util
{
    public class KafkaConsumer : ISubscriber, ICommonConsumer
    {
        private readonly ILogging _logging;
        private string _brokerList;
        private string _executorID;

        private Consumer<Null, string> _consumerKafka;

        public event EventHandler<MessageReceiveEventArgs> OnMessageReceivedEventHandler;
        public event EventHandler<SubscribeDoneEventArgs> OnSubscribeDoneEventHandler;

        public KafkaConsumer(ILogging logging)
        {
            _logging = logging;
        }

        public void Setup(ConnectionConfig connection)
        {
            _brokerList = string.Format("{0}:{1}", connection.Url, connection.Port);
            _executorID = string.Format("Kafka Processor :{0}", Guid.NewGuid().ToString());
        }

        public void Subscribe(string channel)
        {
            using (_consumerKafka =
                    new Consumer<Null, string>(constructConfig(_brokerList, true, new Guid().ToString()), null, new StringDeserializer(Encoding.UTF8)))
            {
                // Note: All event handlers are called on the main thread.

                _consumerKafka.OnMessage += (_, msg)
                    =>
                {
                    _logging.Info($"Executor ID : {_executorID} Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

                    MessageReceiveEventArgs receivedMsg = new Util.MessageReceiveEventArgs(msg.Value);
                    OnMessageReceivedEventHandler(this, receivedMsg);
                };

                _consumerKafka.Subscribe(channel);

                if (OnSubscribeDoneEventHandler != null)
                {
                    SubscribeDoneEventArgs subscribeDoneEventMsg = new SubscribeDoneEventArgs(true);
                    OnSubscribeDoneEventHandler(this, subscribeDoneEventMsg);
                }


                var cancelled = false;
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                while (!cancelled)
                {
                    _consumerKafka.Poll(TimeSpan.FromMilliseconds(10));
                }
            }
        }

        public void Unsubscribe(string channel)
        {
            _consumerKafka.Unsubscribe();
        }

        private Dictionary<string, object> constructConfig(string brokerList, bool enableAutoCommit, string groupId)
        {
            return new Dictionary<string, object>
           {
                { "socket.blocking.max.ms", 1 },
                { "group.id", groupId },
                { "enable.auto.commit", enableAutoCommit },
                { "auto.commit.interval.ms", 10 },
                { "statistics.interval.ms", 60000 },
                { "bootstrap.servers", brokerList },
                { "default.topic.config", new Dictionary<string, object>
                    {
                        { "auto.offset.reset", "smallest" }
                    }
                }
           };
        }
    }
}
