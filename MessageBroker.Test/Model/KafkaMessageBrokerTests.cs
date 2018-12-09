using System;
using MessageBroker.Model;
using MessageBroker.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MessageBroker.Test.Model
{
    [TestClass()]
    public class KafkaMessageBrokerTests
    {

        private Mock<IPublisher> _mockPublisher;

        [TestInitialize]
        public void KafkaMessageBrokerTest()
        {
            _mockPublisher = new Mock<IPublisher>();

            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<string>(), It.IsAny<RequestMessageData<object>>()))
                .Returns((string channel, RequestMessageData<object> message) => {

                    if (message == null) return false;
                    if (message.Data == null) return false;
                    return true;
                });
        }

        [TestMethod()]
        public void PublishNullTest()
        {
            var producer = _mockPublisher.Object;

            string message = null;
            string channel = "test_channel";

            RequestMessageData<string> cm = new RequestMessageData<string>(message);
            var isSend = producer.Publish(channel, cm);

            Assert.IsFalse(isSend);
        }

    }
}
