
using System;
using System.Text.Json.Serialization;

namespace ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss
{
    public class IntegrationEvent
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; set; }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
