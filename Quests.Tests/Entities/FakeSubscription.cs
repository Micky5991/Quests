using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;

namespace Micky5991.Quests.Tests.Entities;

public class FakeSubscription : ISubscription
{
    public bool IgnoreCancelled { get; }

    public EventPriority Priority { get; }

    public ThreadTarget ThreadTarget { get; }

    public bool IsDisposed { get; private set; }

    public int DisposeAmount { get; private set; }

    public void Dispose()
    {
        this.IsDisposed = true;
        this.DisposeAmount += 1;
    }
}
