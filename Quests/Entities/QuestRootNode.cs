using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <inheritdoc cref="IQuestRootNode"/>
[PublicAPI]
public abstract class QuestRootNode : IQuestRootNode
{
    private string title = string.Empty;

    private QuestStatus status = QuestStatus.Sleeping;

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc />
    public string Title
    {
        get => this.title;
        protected set
        {
            if (this.title == value)
            {
                return;
            }

            this.title = value;
            this.OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public QuestStatus Status
    {
        get => this.status;
        protected set
        {
            if (this.status == value)
            {
                return;
            }

            this.status = value;
            this.OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public bool Finished => this.Status == QuestStatus.Failure || this.Status == QuestStatus.Success;

    /// <inheritdoc />
    public IImmutableDictionary<string, object> Blackboard { get; set; } = ImmutableDictionary<string, object>.Empty;

    /// <inheritdoc />
    public IImmutableDictionary<string, object> Context { get; set; } = ImmutableDictionary<string, object>.Empty;

    /// <inheritdoc />
    public IQuestChildNode? ChildNode { get; private set; }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        this.ChildNode?.Dispose();
    }

    /// <inheritdoc />
    public virtual void Initialize()
    {
        if (this.ChildNode == null)
        {
            return;
        }

        this.ChildNode.Initialize();

        this.AttachChildNodeWatchers();

        this.ChildNode.MarkAsActive();
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

    /// <summary>
    /// Invocator for the interface <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <param name="propertyName">Name of the property that called this method.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        if (this.ChildNode == null)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(IQuestChildNode.Status):
                this.Status = this.ChildNode.Status;

                break;
        }
    }
}
