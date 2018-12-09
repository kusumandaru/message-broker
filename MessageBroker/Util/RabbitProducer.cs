using System;
using System.Text;
using MessageBroker.Enum;
using MessageBroker.Log;
using MessageBroker.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageBroker.Util
{
    public class RabbitProducer : IPublisher
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly ILogging _logging;
        private IModel _rabbitChannel;

        public RabbitProducer(ILogging logging)
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
            initRabbit();
        }

        public bool Publish(string channel, object message)
        {
            if (message == null) throw new PublishMessageNullException();

            if (_rabbitChannel == null || _rabbitChannel.IsClosed)
            {
                initRabbit();
                _rabbitChannel.QueueDeclare(channel, true, false, false, null);
            }

            IBasicProperties props = createProperties();

            try
            {
                publishAsync(channel, message, props);
            }
            catch (Exception e)
            {
                _logging.Error(e);
            }

            return true;
        }

        private void initRabbit()
        {
            _connection = _connectionFactory.CreateConnection();
            _rabbitChannel = _connection.CreateModel();

        }

        private IBasicProperties createProperties()
        {
            IBasicProperties props = _rabbitChannel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;
            return props;
        }

        private void publishAsync(string channel, object message, IBasicProperties props)
        {
            var jsonString = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            _logging.Info(String.Format("Publishing '{0}' to '{1}' using '{2}'", jsonString, channel, MessageBrokerEnum.RabbitMQ));
            _rabbitChannel.QueueDeclare(channel, true, false, false, null);
            _rabbitChannel.BasicPublish(string.Empty, channel, props, body);
        }

        public void Release()
        {
            _rabbitChannel.Close();
        }
    }
}
