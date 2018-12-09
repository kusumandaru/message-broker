using System;
using System.Collections.Generic;
using System.Threading;
using MessageBroker.Enum;
using MessageBroker.Factory;
using MessageBroker.Model;
using MessageBroker.Util;
using Ninject;

namespace MessageBroker.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[{0}] Start Application", DateTime.Now.ToString("hh.mm.ss.ffffff"));

            MessageFactory messageFactory = CommonConnection.Load().Get<MessageFactory>();

            /** connection example **/

            var channel = "secret_channel"; //channel for rabbitmq or topic for kafka       

            //subscribe message rabbit
            ConnectionConfig connection = new ConnectionConfig(MessageBrokerEnum.RabbitMQ, "127.0.0.1", null);
            IMessageBroker messageBrokerRabbitMq = messageFactory.Create(connection);
            ((ICommonConsumer)messageBrokerRabbitMq.GetConsumer()).OnMessageReceivedEventHandler += OnMessageReceivedEventHandler;

            //subscribe message redis
            ConnectionConfig connectionRedis = new ConnectionConfig(MessageBrokerEnum.Redis, "127.0.0.1", 6379);
            IMessageBroker messageBrokerRedis = messageFactory.Create(connectionRedis);
            ((ICommonConsumer)messageBrokerRedis.GetConsumer()).OnMessageReceivedEventHandler += OnMessageReceivedEventHandler;

            //subscribe message kafka
            ConnectionConfig connectionKafka = new ConnectionConfig(MessageBrokerEnum.Kafka, "127.0.0.1", 9092);
            IMessageBroker messageBrokerKafka = messageFactory.Create(connectionKafka);
            ((ICommonConsumer)messageBrokerKafka.GetConsumer()).OnMessageReceivedEventHandler += OnMessageReceivedEventHandler;

            messageBrokerKafka.Start(channel);
            messageBrokerRedis.Start(channel);
            messageBrokerRabbitMq.Start(channel);

            /** end of connection example **/


            //publish Message
            RequestMessageData<SomeMessage> objectToSend = new RequestMessageData<SomeMessage>(new SomeMessage(1, "message dikirim", DateTime.Now, new List<string> { "ratna", "santi" })); //some class or object
            bool isPublish = messageBrokerRedis.PublishRequestMessage(channel, objectToSend);
            isPublish &= messageBrokerKafka.PublishRequestMessage<SomeMessage>(channel, objectToSend); //could adding additional key for kafka on third parameter (optional)
            isPublish &= messageBrokerRabbitMq.PublishRequestMessage(channel, objectToSend);

            // example of publish bulk
            //publishBulk(messageBrokerRabbitMq, channel, objectToSend);
            //publishBulk(messageBrokerRedis, channel, objectToSend);
            //publishBulk(messageBrokerKafka, channel, objectToSend);

            Console.WriteLine("after task start");

            Thread.Sleep(10000);
            Console.WriteLine("cancel start");

            messageBrokerRabbitMq.Unsubscribe(channel);
            messageBrokerKafka.Unsubscribe(channel);
            messageBrokerRedis.Unsubscribe(channel);

            Console.WriteLine("cancel finished");


        }


        private static void publishBulk(IMessageBroker messageBroker, string channel, RequestMessageData<SomeMessage> objectToSend)
        {
            var resultDiff = new List<double>();
            for (int i = 0; i < 40; i++)
            {
                var start = DateTime.Now;
                messageBroker.PublishRequestMessage<SomeMessage>(channel, objectToSend);
                var end = DateTime.Now;
                var diffTime = (end - start).TotalMilliseconds;
                resultDiff.Add(diffTime);
            }
            Console.WriteLine(resultDiff);
        }

        private static void publishBulkSimple(IMessageBroker messageBroker, string channel, RequestMessageData<string> objectToSend)
        {
            var resultDiff = new List<double>();
            for (int i = 0; i < 40; i++)
            {
                var start = DateTime.Now;
                messageBroker.PublishRequestMessage<string>(channel, objectToSend);
                var end = DateTime.Now;
                var diffTime = (end - start).TotalMilliseconds;
                resultDiff.Add(diffTime);
            }
            Console.WriteLine(resultDiff);
        }


        private static void OnMessageReceivedEventHandler(object sender, MessageReceiveEventArgs e)
        {
            Console.WriteLine("Message fire reached: {0} on {1}", e.Message, DateTime.Now.ToString("hh.mm.ss.ffffff"));
        }

        //some random class
        private class SomeMessage
        {
            public int Number { get; private set; }
            public string Message { get; private set; }
            public DateTime StartDate { get; private set; }
            public List<string> MemberList { get; private set; }
            public SomeMessage(int number, string message, DateTime startDate, List<string> memberList)
            {
                Number = number;
                Message = message;
                StartDate = startDate;
                MemberList = memberList;
            }
        }
    }
}
