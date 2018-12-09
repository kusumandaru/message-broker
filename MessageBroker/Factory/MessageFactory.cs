using System;
using MessageBroker.Model;
using Ninject;

namespace MessageBroker.Factory
{
    public class MessageFactory : IMessageFactory
    {

        public IMessageBroker Create(ConnectionConfig config)
        {
            string databaseType = config.BrokerType.ToString();
            var kernel = CommonConnection.Load();
            return kernel.Get<IMessageBroker>(databaseType).Setup(config);
        }
    }
}
