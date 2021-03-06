﻿using System;
using Newtonsoft.Json;
using Ninject;
using System.Threading;
using System.Threading.Tasks;
using MessageBroker.Util;

namespace MessageBroker.Model
{
    public class RabbitMessageBroker : IMessageBroker
    {
        private ConnectionConfig _connection;
        private IPublisher _publisher;
        private ISubscriber _subscriber;
        private CancellationTokenSource _tokenSource;

        public RabbitMessageBroker(
            [Named("RabbitMQ")] IPublisher publisher,
            [Named("RabbitMQ")] ISubscriber subscriber
        )
        {
            _publisher = publisher;
            _subscriber = subscriber;
            _tokenSource = new CancellationTokenSource();
        }

        public IMessageBroker Setup(ConnectionConfig connection)
        {
            _connection = connection;
            _publisher.Setup(_connection);
            _subscriber.Setup(_connection);
            return this;
        }

        public bool PublishRequestMessage<T>(string channel, RequestMessageData<T> message)
        {
            return _publisher.Publish(channel, message);
        }

        public bool PublishResponseMessage<T>(string channel, ResponseMessageData<T> message)
        {
            return _publisher.Publish(channel, message);
        }

        public void Subscribe(string channel)
        {
            _subscriber.Subscribe(channel);
        }

        public void Unsubscribe(string channel)
        {
            _subscriber.Unsubscribe(channel);
        }

        public object GetProducer()
        {
            return _publisher;
        }

        public object GetConsumer()
        {
            return _subscriber;
        }

        public void Start(string channel)
        {
            Task.Factory.StartNew(() => { this.Subscribe(channel); });
        }

        public void ReleaseProducer() => throw new NotImplementedException();
    }
}
