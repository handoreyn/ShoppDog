using System.Threading.Tasks;
using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents
{
    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}