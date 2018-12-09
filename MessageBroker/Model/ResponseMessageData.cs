using System;
namespace MessageBroker.Model
{
    public class ResponseMessageData<T>
    {
        public T Data { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool AllowRetry { get; set; }
        public int RetryAttempt { get; set; }
        public ResponseMessageData(T _data, string _code = "", string _message = "", bool _allowRetry = false, int _retryAttempt = 1)
        {
            Data = _data;
            if (!String.IsNullOrEmpty(_code)) Code = _code;
            if (!String.IsNullOrEmpty(_message)) Message = _message;
            AllowRetry = _allowRetry;
            RetryAttempt = _retryAttempt;
        }
    }
}
