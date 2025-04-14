using System.Threading.Tasks;
using EduOutcome.Domain.Events;
using Volo.Abp.EventBus;

namespace PSA.EduOutcome.EventHandlers
{
    public class LearningOutcomeAddedEventHandler : ILocalEventHandler<LearningOutcomeAddedEvent>
    {
        public async Task HandleEventAsync(LearningOutcomeAddedEvent eventData)
        {
            // Example: Log the event or trigger cross-aggregate updates.
            // This handler runs in its own transactional context.
            // Implement required behavior, for instance, updating overall statistics.
            await Task.CompletedTask;
        }
    }
}
