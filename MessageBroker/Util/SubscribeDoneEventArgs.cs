using System;

namespace MessageBroker.Util
{
    public class SubscribeDoneEventArgs : EventArgs
    {
        public bool status { get; set; }

        public SubscribeDoneEventArgs(bool iStatus)
        {
            status = iStatus;
        }
    }
}