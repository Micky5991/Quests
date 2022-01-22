using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities
{
    /// <summary>
    /// Type that implements basic behavior of all nodes.
    /// </summary>
    public abstract class QuestNode : IQuestNode
    {
        private string title = string.Empty;

        private QuestStatus status = QuestStatus.Sleeping;

        private bool disposed;

        private bool initialized;

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
        public bool Initialized
        {
            get => this.initialized;
            private set
            {
                if (this.initialized == value)
                {
                    return;
                }

                this.initialized = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool Disposed
        {
            get => this.disposed;
            private set
            {
                if (this.disposed == value)
                {
                    return;
                }

                this.disposed = value;
                this.OnPropertyChanged();
            }
        }

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
                this.OnStatusChanged(value);
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Finished));
            }
        }

        /// <inheritdoc />
        public virtual void Initialize()
        {
            this.Initialized = true;
        }

        /// <inheritdoc />
        public void SetStatus(QuestStatus newStatus)
        {
            Guard.Argument(newStatus, nameof(newStatus)).Defined().NotEqual(QuestStatus.Success);

            if (this.CanSetToStatus(newStatus) == false)
            {
                return;
            }

            this.Status = newStatus;
        }

        /// <inheritdoc />
        public virtual bool CanSetToStatus(QuestStatus newStatus)
        {
            switch (newStatus)
            {
                case QuestStatus.Sleeping:
                    return this.Status == QuestStatus.Active;

                case QuestStatus.Active:
                    return this.Status == QuestStatus.Sleeping;

                case QuestStatus.Success:
                    return false;

                case QuestStatus.Failure:
                    return this.Status is QuestStatus.Active or QuestStatus.Sleeping;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            this.Disposed = true;
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
}
