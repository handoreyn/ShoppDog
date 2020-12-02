using System;
using RabbitMQ.Client;

namespace ShoppDog.BuildingBlocks.Buss.EventBussRabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}