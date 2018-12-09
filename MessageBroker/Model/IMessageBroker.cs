using System;
namespace MessageBroker.Model
{
    public interface IMessageBroker
    {
        object GetProducer();
        object GetConsumer();

        IMessageBroker Setup(ConnectionConfig connection);
        bool PublishRequestMessage<T>(string channel, RequestMessageData<T> message);
        bool PublishResponseMessage<T>(string channel, ResponseMessageData<T> message);
        void Subscribe(string channel);
        void Unsubscribe(string channel);
        void Start(string channel);
        void ReleaseProducer();
    }
}
