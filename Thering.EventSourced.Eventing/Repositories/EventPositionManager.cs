namespace Thering.EventSourced.Eventing.Repositories
{
    using System.Collections.Generic;

    public class EventPositionManager : IEventPositionManager
    {
        private readonly IEventPositionRepository eventPositionRepository;
        private readonly IDictionary<int, int> positions = new Dictionary<int, int>();

        public EventPositionManager(IEventPositionRepository eventPositionRepository)
        {
            this.eventPositionRepository = eventPositionRepository;
        }

        public void Create(int eventPosition, int eventHandlerCount)
        {
            this.positions.Add(eventPosition, eventHandlerCount);
        }

        public void Decrement(int eventPosition)
        {
            this.positions[eventPosition]--;
            if (this.positions[eventPosition] == 0)
            {
                this.positions.Remove(eventPosition);
                this.eventPositionRepository.SavePosition(eventPosition);
            }
        }
    }
}