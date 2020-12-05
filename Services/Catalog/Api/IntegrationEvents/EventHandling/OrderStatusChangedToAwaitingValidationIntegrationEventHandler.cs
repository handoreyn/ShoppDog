using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShoppDog.BuildingBlocks.Buss.EventBuss.Abstractions;
using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;
using ShoppDog.Services.Catalog.Api.Infrastructure;
using ShoppDog.Services.Catalog.Api.IntegrationEvents.Events;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler
        : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly CatalogContext _catalogContext;
        private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
            CatalogContext context,
            ICatalogIntegrationEventService catalogIntegrationEventService,
            ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _catalogContext = context;
            _catalogIntegrationEventService = catalogIntegrationEventService;
            _logger = logger;
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
        {
            _logger.LogInformation($"----- Handling integration event: {@event.Id} - AppName - {@event}");
            var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();
            foreach (var item in @event.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(item.ProductId);
                var hasStock = catalogItem.AvailableStock >= item.Units;
                var confirmedItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);
                confirmedOrderStockItems.Add(confirmedItem);
            }

            var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
             ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems) :
            new OrderStockConfirmedIntegrationEvent(@event.OrderId);

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
            await _catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);
        }
    }
}