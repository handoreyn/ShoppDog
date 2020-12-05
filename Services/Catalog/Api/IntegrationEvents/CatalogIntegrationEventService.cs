using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppDog.BuildingBlocks.Buss.EventBuss.Abstractions;
using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;
using ShoppDog.BuildingBlocks.Buss.IntegrationEventLog.Services;
using ShoppDog.BuildingBlocks.Buss.IntegrationEventLog.Utilities;
using ShoppDog.Services.Catalog.Api.Infrastructure;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents
{
    public class CatalogIntegrationEventService : ICatalogIntegrationEventService, IDisposable
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogService;
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _catalogContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<CatalogIntegrationEventService> _logger;
        private volatile bool disposedValue;

        public CatalogIntegrationEventService(
            ILogger<CatalogIntegrationEventService> logger,
            IEventBus eventBus,
            CatalogContext catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _integrationEventLogService = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing) (_eventLogService as IDisposable).Dispose();
            disposedValue = true;
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation($"----- Publishing integration event : {evt.Id} - AppName - ({evt})");
                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR Publishing integration event: {evt.Id} from AppName - ({evt})");
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation($"----- CatalogIntegrationEventService - Saving changes and integrationEvent: {evt.Id}");

            await ResilientTransaction.New(_catalogContext).ExecuteAsync(async () =>
            {
                await _catalogContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _catalogContext.Database.CurrentTransaction);
            });
        }
    }
}