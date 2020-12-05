using System.Collections.Generic;
using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.Events
{
    public class OrderStockRejectedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }
        public IEnumerable<ConfirmedOrderStockItem> OrderStockItems { get; }

        public OrderStockRejectedIntegrationEvent(int orderId, IEnumerable<ConfirmedOrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }
    }

    public class ConfirmedOrderStockItem
    {
        public int ProductId { get; }
        public bool HasStock { get; }
        public ConfirmedOrderStockItem(int productId, bool hasStock)
        {
            ProductId = productId;
            HasStock = hasStock;
        }
    }
}