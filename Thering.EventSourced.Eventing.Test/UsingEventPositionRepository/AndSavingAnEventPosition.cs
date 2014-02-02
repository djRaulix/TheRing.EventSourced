namespace Thering.EventSourced.Eventing.Test.UsingEventPositionRepository
{
    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    public class AndSavingAnEventPosition : UsingEventPositionRepository
    {
        private readonly Type eventHandlerType = typeof(FakeEventHandler);

        private const int SavedPosition = 18;

        protected override void BecauseOf()
        {
            base.EstablishContext();
            this.EventPositionRepository.Save(this.eventHandlerType, SavedPosition);
        }

        [Test]
        public void ThenShouldRaiseAnAggregateEventHandledEvent()
        {
            this.EventStreamRepository.CallsTo(streamRepo => 
                streamRepo.Save(
                    A<string>.That.Matches(sId => sId.Equals(StreamId.EventPositionStream)),
                    A<AggregateEventHandled>.That.Matches(@event => @event.EventHandlerFullTypeName.Equals(this.eventHandlerType.FullName) && @event.Position == SavedPosition),
                    null, 
                    null))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}