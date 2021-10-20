using System.Collections.Immutable;
using System.ComponentModel;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <inheritdoc cref="IQuestRootNode"/>
[PublicAPI]
public abstract class QuestRootNode : QuestNode, IQuestRootNode
{
    /// <inheritdoc />
    public IImmutableDictionary<string, object> Blackboard { get; set; } = ImmutableDictionary<string, object>.Empty;

    /// <inheritdoc />
    public IImmutableDictionary<string, object> Context { get; set; } = ImmutableDictionary<string, object>.Empty;

    /// <inheritdoc />
    public IQuestChildNode? ChildNode { get; private set; }

    /// <inheritdoc />
    public override void Dispose()
    {
        if (this.Disposed)
        {
            throw new ObjectDisposedException("Quest has already been disposed.");
        }

        this.ChildNode?.Dispose();
        this.ChildNode = null;

        base.Dispose();
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        base.Initialize();

        if (this.ChildNode == null)
        {
            return;
        }

        this.ChildNode.Initialize();
        this.AttachChildNodeWatchers();
    }

    /// <inheritdoc />
    protected override void OnStatusChanged(QuestStatus newStatus)
    {
        Guard.Argument(newStatus, nameof(newStatus)).Defined();

        switch (newStatus)
        {
            case QuestStatus.Sleeping:
            case QuestStatus.Active:
            case QuestStatus.Failure:
                this.ChildNode?.SetStatus(newStatus);

                break;

            case QuestStatus.Success:
                // Ignore
                break;
        }

        base.OnStatusChanged(newStatus);
    }

    /// <summary>
    /// Sets the Child Quest object to the given <paramref name="childNode"/>.
    /// </summary>
    /// <param name="childNode">New node to set the <see cref="ChildNode"/> property to.</param>
    /// <exception cref="InvalidOperationException"><see cref="ChildNode"/> has already been set.</exception>
    protected void SetChildQuests(IQuestChildNode childNode)
    {
        Guard.Argument(childNode, nameof(childNode)).NotNull();

        if (this.ChildNode != null)
        {
            throw new InvalidOperationException("Child quest already set.");
        }

        this.ChildNode = childNode;
    }

    private void AttachChildNodeWatchers()
    {
        if (this.ChildNode == null)
        {
            return;
        }

        this.ChildNode.PropertyChanged += this.OnChildPropertyChanged;
    }

    private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Guard.Argument(sender, nameof(sender)).NotNull();
        Guard.Argument(e, nameof(e)).NotNull();

        if (this.ChildNode == null)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status):
                switch (this.ChildNode.Status)
                {
                    case QuestStatus.Active:
                    case QuestStatus.Sleeping:
                    case QuestStatus.Failure:
                        this.SetStatus(this.ChildNode.Status);

                        break;

                    case QuestStatus.Success:
                        this.MarkAsSuccess();

                        break;
                }

                break;
        }
    }
}
