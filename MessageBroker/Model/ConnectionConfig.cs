using System;
using MessageBroker.Enum;

namespace MessageBroker.Model
{
    public class ConnectionConfig
    {
        public MessageBrokerEnum BrokerType { get; private set; }
        public string Url { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public ConnectionConfig(MessageBrokerEnum brokerType, string url, int? port = null)
        {
            BrokerType = brokerType;
            Url = url;
            if (port != null) Port = port.GetValueOrDefault();
        }

        public ConnectionConfig(MessageBrokerEnum brokerType, string url, int? port = null, string username = "", string password = "")
        {
            BrokerType = brokerType;
            Url = url;
            if (port != null) Port = port.GetValueOrDefault();
            if (!String.IsNullOrEmpty(username)) Username = username;
            if (!String.IsNullOrEmpty(password)) Password = password;
        }
    }
}
