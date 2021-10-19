using System.Collections.Immutable;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Type that has always success status, unless it fails explicitly.
/// </summary>
public abstract class QuestConditonNode : QuestChildNode, IQuestTaskNode
{
    private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestConditonNode"/> class.
    /// </summary>
    /// <param name="rootNode">Reference to the root node of this quest.</param>
    protected QuestConditonNode(IQuestRootNode rootNode)
        : base(rootNode)
    {
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        this.DetachEventListeners();
    }

    /// <inheritdoc />
    public override bool CanMarkAsSleeping()
    {
        return this.Status is QuestStatus.Success || base.CanMarkAsSleeping();
    }

    /// <inheritdoc />
    public override bool CanMarkAsFailure()
    {
        return this.Status == QuestStatus.Success || base.CanMarkAsFailure();
    }

    protected abstract IEnumerable<ISubscription> GetEventSubscriptions();

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        switch (newStatus)
        {
            case QuestStatus.Active:
                this.MarkAsSuccess();

                break;

            case QuestStatus.Success:
                this.AttachEventListeners();

                break;

            default:
                this.DetachEventListeners();

                break;
        }

        base.OnStatusChanged(newStatus);
    }

    private void AttachEventListeners()
    {
        this.DetachEventListeners();

        this.eventSubscriptions = this.GetEventSubscriptions().ToImmutableHashSet();
    }

    private void DetachEventListeners()
    {
        var subscriptions = this.eventSubscriptions;
        this.eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
    }
}
