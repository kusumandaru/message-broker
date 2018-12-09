using System;
using MessageBroker.Enum;
using MessageBroker.Log;
using MessageBroker.Model;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace MessageBroker.Util
{
    public class RedisProducer : IPublisher
    {
        private ConnectionConfig _connection;
        private readonly ILogging _logging;
        private RedisClient _redis;

        public RedisProducer(ILogging logging)
        {
            _logging = logging;
        }

        public void Setup(ConnectionConfig connection)
        {
            _connection = connection;
            _redis = new RedisClient(_connection.Url, _connection.Port);
        }

        public bool Publish(string channel, object message)
        {
            if (message == null) throw new PublishMessageNullException();
            try
            {
                publish(channel, message);
            }
            catch (Exception e)
            {
                _redis = new RedisClient(_connection.Url, _connection.Port);
                publish(channel, message);
                _logging.Error(e);
            }
            return true;
        }

        private long publish(string channel, object message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            _logging.Info(String.Format("Publishing '{0}' to '{1}' using '{2}'", messageJson, channel, MessageBrokerEnum.Redis));
            return _redis.PublishMessage(channel, messageJson);
        }

        public void Release()
        {
            _redis.Dispose();
        }
    }
}
