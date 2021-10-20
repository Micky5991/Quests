using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Type that implements basic behavior of all nodes.
/// </summary>
public abstract class QuestNode : IQuestNode
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
            Guard.Argument(value, nameof(value)).NotNull();

            if (this.title == value)
            {
                return;
            }

            this.title = value;
            this.OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public bool Finished => this.Status is QuestStatus.Success or QuestStatus.Failure;

    /// <inheritdoc />
    public bool Disposed { get; private set; }

    /// <inheritdoc />
    public QuestStatus Status
    {
        get => this.status;
        private set
        {
            Guard.Argument(value, nameof(value)).Defined();

            if (this.status == value)
            {
                return;
            }

            this.status = value;
            this.OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public virtual void Initialize()
    {
        this.PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(this.Status):
                    this.OnStatusChanged(this.Status);

                    break;
            }
        };
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        this.Disposed = true;
    }

    /// <inheritdoc />
    public virtual void MarkAsActive()
    {
        if (this.CanMarkAsActive() == false)
        {
            return;
        }

        this.Status = QuestStatus.Active;
    }

    /// <inheritdoc />
    public virtual void MarkAsSleeping()
    {
        if (this.CanMarkAsSleeping() == false)
        {
            return;
        }

        this.Status = QuestStatus.Sleeping;
    }

    /// <inheritdoc />
    public virtual void MarkAsFailure()
    {
        if (this.CanMarkAsFailure() == false)
        {
            return;
        }

        this.Status = QuestStatus.Failure;
    }

    /// <inheritdoc />
    public virtual bool CanMarkAsActive()
    {
        return this.Status == QuestStatus.Sleeping;
    }

    /// <inheritdoc />
    public virtual bool CanMarkAsSleeping()
    {
        return this.Status == QuestStatus.Active;
    }

    /// <inheritdoc />
    public virtual bool CanMarkAsFailure()
    {
        return this.Status is QuestStatus.Active or QuestStatus.Sleeping;
    }

    /// <summary>
    /// Marks the current node as successful.
    /// </summary>
    protected virtual void MarkAsSuccess()
    {
        if (this.CanMarkAsSuccess() == false)
        {
            return;
        }

        this.Status = QuestStatus.Success;
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

    /// <summary>
    /// Will be called when the status of this node has been changed.
    /// </summary>
    /// <param name="newStatus">New status of this node.</param>
    /// <exception cref="ArgumentException">Value <paramref name="newStatus"/> is not defined in <see cref="QuestStatus"/>.</exception>
    protected virtual void OnStatusChanged(QuestStatus newStatus)
    {
        // Empty
    }

    /// <summary>
    /// Indicates if the current node can be marked as success.
    /// </summary>
    /// <returns>Returns true if the node can be marked as success.</returns>
    protected virtual bool CanMarkAsSuccess()
    {
        return this.Status is QuestStatus.Active or QuestStatus.Sleeping;
    }
}
