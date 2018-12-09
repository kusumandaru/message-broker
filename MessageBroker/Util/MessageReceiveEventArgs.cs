using System;

namespace MessageBroker.Util
{
    public class MessageReceiveEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageReceiveEventArgs(string iMessage)
        {
            Message = iMessage;
        }
    }
}
