using System;
using MessageBroker.Enum;
using MessageBroker.Model;
using MessageBroker.Util;
using Ninject;
using Ninject.Extensions.Conventions;

namespace MessageBroker
{
    public static class CommonConnection
    {
        public static StandardKernel Load()
        {
            var kernel = new StandardKernel();

            kernel.Bind(x =>
            {
                x.FromAssembliesMatching("Global.Flight.Data.dll")
                 .SelectAllClasses()
                 .BindAllInterfaces()
                 .Configure(b => b.InTransientScope());
            });

            kernel.Bind(x =>
            {
                x.FromThisAssembly()
                 .SelectAllClasses()
                 .BindAllInterfaces()
                 .Configure(b => b.InTransientScope());
            });

            kernel.Bind<IMessageBroker>().To<KafkaMessageBroker>().Named(MessageBrokerEnum.Kafka.ToString());
            kernel.Bind<IMessageBroker>().To<RabbitMessageBroker>().Named(MessageBrokerEnum.RabbitMQ.ToString());
            kernel.Bind<IMessageBroker>().To<RedisMessageBroker>().Named(MessageBrokerEnum.Redis.ToString());

            kernel.Bind<IPublisher>().To<KafkaProducer>().Named(MessageBrokerEnum.Kafka.ToString());
            kernel.Bind<IPublisher>().To<RabbitProducer>().Named(MessageBrokerEnum.RabbitMQ.ToString());
            kernel.Bind<IPublisher>().To<RedisProducer>().Named(MessageBrokerEnum.Redis.ToString());

            kernel.Bind<ISubscriber>().To<KafkaConsumer>().Named(MessageBrokerEnum.Kafka.ToString());
            kernel.Bind<ISubscriber>().To<RabbitConsumer>().Named(MessageBrokerEnum.RabbitMQ.ToString());
            kernel.Bind<ISubscriber>().To<RedisConsumer>().Named(MessageBrokerEnum.Redis.ToString());


            return kernel;
        }
    }
}
