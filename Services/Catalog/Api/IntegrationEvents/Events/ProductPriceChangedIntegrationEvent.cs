using ShoppDog.BuildingBlocks.Buss.EventBuss.EventBuss;

namespace ShoppDog.Services.Catalog.Api.IntegrationEvents.Events
{
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; private set; }
        public decimal NewPrice { get; private set; }
        public decimal OldPrice { get; private set; }

        public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, int oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}