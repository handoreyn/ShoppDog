using System.Collections.Generic;
using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.Events
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }
        public IEnumerable<OrderStockItem> OrderStockItems { get; }

        public OrderStatusChangedToAwaitingValidationIntegrationEvent(int orderId, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }
    }

    public class OrderStockItem
    {
        public int ProductId { get; set; }
        public int Units { get; set; }

        public OrderStockItem(int productId, int units)
        {
            ProductId = productId;
            Units = units;
        }
    }
}