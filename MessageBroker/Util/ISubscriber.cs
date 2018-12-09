using MessageBroker.Model;

namespace MessageBroker.Util
{
    public interface ISubscriber
    {
        void Setup(ConnectionConfig connection);
        void Subscribe(string channel);
        void Unsubscribe(string channel);
    }
}
