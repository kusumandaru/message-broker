using System.Collections.Generic;
using MessageBroker.Enum;

namespace MessageBroker.Model
{
    public class RequestMessageData<T>
    {
        public T Data { get; set; }
        public string Language { get; set; }
        public string PublishHostUrl { get; set; }
        public int? PublishHostPort { get; set; }
        public string PublishChannel { get; set; }
        public MessageBrokerEnum PublishType { get; set; }
        public string CacheHostUrl { get; set; }
        public int? CacheHostPort { get; set; }
        public string CacheKey { get; set; }
        public CacheTypeEnum CacheType { get; set; }
        public int CacheTimeoutMiliseconds { get; set; }
        public string SessionIDGateway { get; set; }
        public string UserIDGateway { get; set; }
        public string ClientIDGateway { get; set; }
        public int RetryAttempt { get; set; }
        public RequestMessageData()
        {
        }

        public RequestMessageData(T data)
        {
            Data = data;
        }

        public RequestMessageData(T _data,
            string _publishHostUrl, int? _publishHostPort, string _publishChannel, MessageBrokerEnum _publishType,
            string _cacheHostUrl, int? _cacheHostPort, string _cacheKey, CacheTypeEnum _cacheType, int _cacheTimeoutMiliseconds,
            string _sessionIDGateway, string _userIDGateway = "", string _clientIDGateWay = "", int _retryAttempt = 1, string _lang = "id-id")
        {
            Data = _data;
            Language = _lang;
            PublishHostUrl = _publishHostUrl;
            PublishHostPort = _publishHostPort;
            PublishChannel = _publishChannel;
            PublishType = _publishType;
            CacheHostUrl = _cacheHostUrl;
            CacheHostPort = _cacheHostPort;
            CacheKey = _cacheKey;
            CacheTimeoutMiliseconds = _cacheTimeoutMiliseconds;
            CacheType = _cacheType;
            SessionIDGateway = _sessionIDGateway;
            UserIDGateway = _userIDGateway;
            ClientIDGateway = _clientIDGateWay;
            RetryAttempt = _retryAttempt;
        }
    }
}
