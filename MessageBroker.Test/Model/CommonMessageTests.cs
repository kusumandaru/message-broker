using System;
using MessageBroker.Enum;
using MessageBroker.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageBroker.Test.Model
{
    [TestClass]
    public class CommonMessageTests
    {
        [TestMethod()]
        public void Connection_CommonMessageDefaultTest()
        {
            var message = new RequestMessageData<string>("varA");

            Assert.IsNotNull(message);
            Assert.IsNull(message.PublishChannel);
            Assert.IsNull(message.CacheKey);
            Assert.AreEqual(message.Data, "varA");
            Assert.IsTrue(message.Data is String);
        }

        [TestMethod()]
        public void Connection_CommonMessageObjectTest()
        {
            var content = new SomeMessage(1, "varA");

            var message = new RequestMessageData<SomeMessage>(content);

            Assert.IsNotNull(message);
            Assert.IsNull(message.PublishChannel);
            Assert.IsNull(message.CacheKey);
            Assert.AreEqual(message.Data, content);
            Assert.IsTrue(message.Data is SomeMessage);
        }

        [TestMethod()]
        public void Connection_CommonMessageDefaultCompleteTest()
        {
            var message = new RequestMessageData<string>(
                _data: "versionTest.serviceNameTest.channelTest",
                _publishHostUrl: "publishResponseHostUrlTest",
                _publishHostPort: 123,
                _publishChannel: "publishResponseChannelTest",
                _publishType: MessageBrokerEnum.Redis,
                _cacheHostUrl: "publishResponseCacheHostUrlTest",
                _cacheHostPort: 456,
                _cacheKey: "publishResponseCacheKeyTest",
                _cacheType: CacheTypeEnum.MemoryCache,
                _cacheTimeoutMiliseconds: 10,
                _sessionIDGateway: "sessionIdGatewayTest",
                _userIDGateway: "userIDGatewayTest"
                );

            Assert.IsNotNull(message);
            Assert.IsNotNull(message.PublishChannel);
            Assert.IsNotNull(message.CacheKey);
            Assert.AreEqual(message.Data, "versionTest.serviceNameTest.channelTest");
            Assert.IsTrue(message.Data is String);
        }

        [TestMethod()]
        public void Connection_CommonMessageObjectCompleteTest()
        {
            var content = new SomeMessage(1, "varA");

            var message = new RequestMessageData<SomeMessage>(
                _data: content,
                _publishHostUrl: "publishResponseHostUrlTest",
                _publishHostPort: 123,
                _publishChannel: "publishResponseChannelTest",
                _publishType: MessageBrokerEnum.Redis,
                _cacheHostUrl: "publishResponseCacheHostUrlTest",
                _cacheHostPort: 456,
                _cacheKey: "publishResponseCacheKeyTest",
                _cacheType: CacheTypeEnum.MemoryCache,
                _cacheTimeoutMiliseconds: 10,
                _sessionIDGateway: "sessionIdGatewayTest",
                _userIDGateway: "userIDGatewayTest"
                );

            Assert.IsNotNull(message);
            Assert.IsNotNull(message.PublishChannel);
            Assert.IsNotNull(message.CacheKey);
            Assert.AreEqual(message.Data, content);
            Assert.IsTrue(message.Data is SomeMessage);
        }

        private class SomeMessage
        {
            public int Number { get; private set; }
            public string Message { get; private set; }
            public SomeMessage(int number, string message)
            {
                Number = number;
                Message = message;
            }
        }
    }
}
