using System;
using MessageBroker.Log;
using MessageBroker.Model;
using ServiceStack.Redis;

namespace MessageBroker.Util
{
    public class RedisConsumer : ISubscriber, ICommonConsumer
    {
        private ConnectionConfig _connection;
        private readonly ILogging _logging;
        private IRedisSubscription _subscription;
        public event EventHandler<MessageReceiveEventArgs> OnMessageReceivedEventHandler;
        public event EventHandler<SubscribeDoneEventArgs> OnSubscribeDoneEventHandler;

        public RedisConsumer(ILogging logging)
        {
            _logging = logging;
        }

        public void Setup(ConnectionConfig connection)
        {
            _connection = connection;
        }

        public void Subscribe(string channelName)
        {
            using (var redisClient = new RedisClient(_connection.Url, _connection.Port))
            using (_subscription = redisClient.CreateSubscription())
            {

                _subscription.OnSubscribe = channel =>
                {
                    _logging.Info(String.Format("Redis Started Listening On to '{0}'", channel));
                };
                _subscription.OnUnSubscribe = channel =>
                {
                    _logging.Info(String.Format("Redis UnSubscribed from '{0}'", channel));
                    redisClient.UnSubscribe();

                };
                _subscription.OnMessage = (channel, msg) =>
                {
                    _logging.Info(String.Format("Received '{0}' from channel '{1}'", msg, channel));

                    MessageReceiveEventArgs receivedMsg = new Util.MessageReceiveEventArgs(msg);
                    OnMessageReceivedEventHandler(this, receivedMsg);
                };

                if (OnSubscribeDoneEventHandler != null)
                {
                    SubscribeDoneEventArgs subscribeDoneEventMsg = new SubscribeDoneEventArgs(true);
                    OnSubscribeDoneEventHandler(this, subscribeDoneEventMsg);
                }

                _subscription.SubscribeToChannels(channelName); //blocking    
            }

        }

        public void Unsubscribe(string channelName)
        {
            try
            {
                _subscription.UnSubscribeFromChannels(channelName);
            }
            catch (Exception e)
            {
                _logging.Error(e);
            }
        }
    }
}
