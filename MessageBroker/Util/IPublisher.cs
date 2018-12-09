using MessageBroker.Model;

namespace MessageBroker.Util
{
    public interface IPublisher
    {
        bool Publish(string channel, object message);
        void Setup(ConnectionConfig connection);
        void Release();
    }
}
