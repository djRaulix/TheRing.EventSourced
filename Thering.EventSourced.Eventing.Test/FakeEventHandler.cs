
namespace Thering.EventSourced.Eventing.Test
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading;

    using Thering.EventSourced.Eventing;

    using TheRing.Test.Fakes;

    public class FakeEventHandler :  IHandlesEvent<FakeEvent>, IHandlesEvent<IntFakeEvent>
    {
        public FakeEvent LastHandledEvent { get; private set; }

        public ICollection<FakeEvent> HandledEvents { get; private set; }
        public IDictionary<int,IntFakeEvent> HandledIntFakeEvent { get; private set; }

        private int count; 

        public FakeEventHandler()
        {
            this.HandledEvents = new Collection<FakeEvent>();
            this.HandledIntFakeEvent = new Dictionary<int, IntFakeEvent>();
        }

        public virtual void Handles(FakeEvent @event)
        {
            this.LastHandledEvent = @event;
            this.HandledEvents.Add(@event);
            Trace.WriteLine("Fake Event Hanlded Id : " + @event.No);
        }

        public virtual void Handles(IntFakeEvent @event)
        {
            this.HandledIntFakeEvent.Add(++this.count, @event);
            Thread.Sleep(100); //Latency simulation
            Trace.WriteLine("Int Fake Event Hanlded ! Id : " + @event.No);
        }
    }
}