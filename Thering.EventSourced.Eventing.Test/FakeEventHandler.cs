
namespace Thering.EventSourced.Eventing.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading;

    using Thering.EventSourced.Eventing;

    using TheRing.Test.Fakes;

    public class FakeEventHandler :  IHandleEvent<FakeEvent>, IHandleEvent<IntFakeEvent>
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

        public virtual void Handle(FakeEvent @event)
        {
            this.LastHandledEvent = @event;
            this.HandledEvents.Add(@event);
            Trace.WriteLine(DateTime.Now + " Fake Event Hanlded Id : " + @event.No);
        }

        public void Handle(IntFakeEvent @event)
        {
            this.HandledIntFakeEvent.Add(++this.count, @event);
            Thread.Sleep(100); //Latency simulation
            Trace.WriteLine(DateTime.Now + " Int Fake Event Hanlded ! Id : " + @event.No);
        }
    }
}