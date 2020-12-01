using System.Threading.Tasks;

namespace ShoppDog.BuildingBlocks.Buss.EventBuss.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}