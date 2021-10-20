using Micky5991.EventAggregator.Elements;

namespace Micky5991.Quests.Tests.Entities;

public class TestEvent : EventBase
{
    public bool WasSuccess { get; }

    public TestEvent(bool wasSuccess)
    {
        this.WasSuccess = wasSuccess;
    }
}
