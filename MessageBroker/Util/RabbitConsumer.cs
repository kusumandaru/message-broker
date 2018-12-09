using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MessageBroker.Log;
using MessageBroker.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker.Util
{
    public class RabbitConsumer : ISubscriber, ICommonConsumer
    {
        private ConnectionFactory _connectionFactory;
        private readonly ILogging _logging;

        private IModel _channelMq;

        public event EventHandler<MessageReceiveEventArgs> OnMessageReceivedEventHandler;
        public event EventHandler<SubscribeDoneEventArgs> OnSubscribeDoneEventHandler;
        private bool cancelled;

        public RabbitConsumer(ILogging logging)
        {
            _logging = logging;
        }

        public void Setup(ConnectionConfig connection)
        {
            if (connection.Port == 0)
            {
                _connectionFactory = new ConnectionFactory { HostName = connection.Url };
            }
            else
            {
                _connectionFactory = new ConnectionFactory { HostName = connection.Url, Port = connection.Port };
            }
        }

        public void Subscribe(string channel)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (_channelMq = connection.CreateModel())
            {
                cancelled = false;

                _channelMq.QueueDeclare(channel, true, false, false, null);

                var consumer = new EventingBasicConsumer(_channelMq);
                EventHandler<BasicDeliverEventArgs> queueMsgReceived = (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    _logging.Info("Rabbit Received {0}", message);

                    MessageReceiveEventArgs receivedMsg = new Util.MessageReceiveEventArgs(message);
                    OnMessageReceivedEventHandler(this, receivedMsg);
                };
                consumer.Received += queueMsgReceived;
                string c = _channelMq.BasicConsume(channel, true, consumer);

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                if (OnSubscribeDoneEventHandler != null)
                {
                    SubscribeDoneEventArgs subscribeDoneEventMsg = new SubscribeDoneEventArgs(true);
                    OnSubscribeDoneEventHandler(this, subscribeDoneEventMsg);
                }

                while (!cancelled)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                }

                consumer.Received -= queueMsgReceived;
            }


        }

        public void Unsubscribe(string channel)
        {
            cancelled = true;
            _channelMq.Abort();
        }

        private Dictionary<string, object> constructConfig(string brokerList, bool enableAutoCommit, string groupId)
        {
            return new Dictionary<string, object>
           {
                { "group.id", groupId },
                { "enable.auto.commit", enableAutoCommit },
                { "auto.commit.interval.ms", 5000 },
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
