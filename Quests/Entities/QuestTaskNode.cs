using System.Collections.Immutable;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <inheritdoc cref="IQuestTaskNode" />
public abstract class QuestTaskNode : QuestChildNode, IQuestTaskNode
{
    private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestTaskNode"/> class.
    /// </summary>
    /// <param name="rootNode"></param>
    public QuestTaskNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }

    /// <inheritdoc />
    public override void Activate()
    {
        if (this.CanActivate() == false)
        {
            return;
        }

        this.eventSubscriptions = this.AttachEventListeners().ToImmutableHashSet();
        base.Activate();
    }

    /// <inheritdoc />
    public override void Deactivate()
    {
        if (this.CanDeactivate() == false)
        {
            return;
        }

        this.DetachEventListeners();
        base.Deactivate();
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        this.DetachEventListeners();
    }

    protected abstract IEnumerable<ISubscription> AttachEventListeners();

    protected virtual void DetachEventListeners()
    {
        var subscriptions = this.eventSubscriptions;
        this.eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
    }
}
