using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShoppDog.BuildingBlocks.Buss.EventBuss.Abstractions;
using ShoppDog.Services.Catalog.Api.Infrastructure;
using ShoppDog.Services.Catalog.Api.IntegrationEvents.Events;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly CatalogContext _catalogContext;
        private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

        public OrderStatusChangedToPaidIntegrationEventHandler(CatalogContext context,
            ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
        {
            _catalogContext = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
        {
            _logger.LogInformation($"----- Handling integration event: {@event.Id} at 'AppName' - ({@event})");

            foreach (var item in @event.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(item.ProductId);
                catalogItem.RemoveStock(item.Units);
            }

            await _catalogContext.SaveChangesAsync();
        }
    }
}