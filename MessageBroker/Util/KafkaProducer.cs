using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using MessageBroker.Enum;
using MessageBroker.Log;
using MessageBroker.Model;
using Newtonsoft.Json;

namespace MessageBroker.Util
{
    public class KafkaProducer : IPublisher
    {
        private readonly ILogging _logging;
        private string _brokerList;
        private Producer<string, string> _producer;
        private Dictionary<string, object> _config;

        public KafkaProducer(ILogging logging)
        {
            _logging = logging;
        }

        public void Setup(ConnectionConfig connection)
        {
            _brokerList = string.Format("{0}:{1}", connection.Url, connection.Port);
            _config = createConfigKafka();
            _producer = new Producer<string, string>(_config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8));
        }

        public bool Publish(string channel, object message)
        {
            if (message == null) throw new PublishMessageNullException();

            try
            {
                var deliveryReport = publishAsync(channel, null, message);
            }
            catch (Exception e)
            {
                _producer = new Producer<string, string>(_config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8));
                var deliveryReport = publishAsync(channel, null, message);
                _logging.Error(e);
            }

            return true;
        }

        private Dictionary<string, object> createConfigKafka()
        {
            var config = new Dictionary<string, object>();
            config.Add("bootstrap.servers", _brokerList);
            config.Add("socket.blocking.max.ms", 1);
            config.Add("socket.nagle.disable", true);
            return config;
        }

        private Message<string, string> publishAsync(string channel, string key, object message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            _logging.Info(String.Format("Publishing '{0}' to '{1}' using '{2}'", messageJson, channel, MessageBrokerEnum.Kafka));
            return _producer.ProduceAsync(channel, key, messageJson).Result;
        }

        public void Release()
        {
            _producer.Dispose();
        }
    }
}
