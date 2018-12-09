using System;
using MessageBroker.Model;

namespace MessageBroker.Factory
{
    public interface IMessageFactory
    {
        IMessageBroker Create(ConnectionConfig config);
    }
}
