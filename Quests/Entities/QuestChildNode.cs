using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <inheritdoc />
[PublicAPI]
public abstract class QuestChildNode : IQuestChildNode
{
    private string title = string.Empty;

    private QuestStatus status = QuestStatus.Sleeping;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestChildNode"/> class.
    /// </summary>
    /// <param name="rootNode">Reference to root node.</param>
    public QuestChildNode(IQuestRootNode rootNode)
    {
        this.RootNode = rootNode;
    }

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
        private set
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
    public bool Finished => this.Status is QuestStatus.Success or QuestStatus.Failure;

    /// <inheritdoc />
    public IQuestRootNode RootNode { get; }

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
    public abstract void Dispose();

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

    /// <inheritdoc />
    protected virtual void MarkAsSuccess()
    {
        if (this.CanMarkAsSuccess() == false)
        {
            return;
        }

        this.Status = QuestStatus.Success;
    }

    protected virtual bool CanMarkAsSuccess()
    {
        return this.Status is QuestStatus.Active or QuestStatus.Sleeping;
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

    protected virtual void OnStatusChanged(QuestStatus newStatus)
    {
        // Empty
    }
}
