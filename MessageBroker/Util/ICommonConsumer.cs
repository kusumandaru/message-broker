using System;

namespace MessageBroker.Util
{
    public interface ICommonConsumer
    {
        event EventHandler<MessageReceiveEventArgs> OnMessageReceivedEventHandler;

        event EventHandler<SubscribeDoneEventArgs> OnSubscribeDoneEventHandler;
    }
}
