# message-broker
Publish Subscribe either Kafka, Redis, RabbitMQ on dotnet core

<img src="https://ci.appveyor.com/api/projects/status/github/kusumandaru/message-broker?branch=master&svg=true" alt="Project Build">
<img src="https://ci.appveyor.com/api/projects/status/github/kusumandaru/message-broker?svg=true&passingText=Test%20-%20Passed" alt="Test Status">

example located in MessageBroker.Example/Program.cs

## set connection like:
ConnectionConfig connection = new ConnectionConfig(MessageBrokerEnum.RabbitMQ, "127.0.0.1", null);

IMessageBroker broker = messageFactory.Create(connection);

## than start subscribe:
broker.Start("channel_name");

## publish like: 
broker.PublishRequestMessage(channel, objectToSend);
