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
    public bool Completed => this.Status is QuestStatus.Success or QuestStatus.Failure;

    /// <inheritdoc />
    public IQuestRootNode RootNode { get; }

    /// <inheritdoc />
    public bool Deactivated => this.Status != QuestStatus.Active;

    /// <inheritdoc />
    public virtual bool CanActivate()
    {
        return this.Deactivated && this.Completed == false;
    }

    /// <inheritdoc />
    public virtual bool CanDeactivate()
    {
        return this.Deactivated == false && this.Completed == false;
    }

    /// <inheritdoc />
    public virtual void Activate()
    {
        if (this.CanActivate() == false)
        {
            return;
        }

        this.Status = QuestStatus.Active;
    }

    /// <inheritdoc />
    public virtual void Deactivate()
    {
        if (this.CanDeactivate() == false)
        {
            return;
        }

        this.Status = QuestStatus.Sleeping;
    }

    /// <inheritdoc />
    public abstract void Initialize();

    /// <inheritdoc />
    public abstract void Dispose();

    /// <summary>
    /// Invocator for the interface <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <param name="propertyName">Name of the property that called this method.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
