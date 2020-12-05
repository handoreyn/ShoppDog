using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.Events
{
    public class OrderStockConfirmedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }
        public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
    }
}