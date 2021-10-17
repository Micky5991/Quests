using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Exceptions;
using Micky5991.Quests.Interfaces.Entities;

namespace Micky5991.Quests.Entities
{
    /// <summary>
    /// Base type for quests with different tools to run a quest.
    /// </summary>
    public abstract class QuestBase : IQuest
    {
        private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        private QuestStatus status;

        private IImmutableDictionary<string, object> context = ImmutableDictionary<string, object>.Empty;

        private string title = string.Empty;

        private string description = string.Empty;

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
        public string Description
        {
            get => this.description;
            protected set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;

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
        public IImmutableDictionary<string, object> Context
        {
            get => this.context;
            protected set
            {
                this.context = value;

                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public virtual void Initialize(IImmutableDictionary<string, object>? initialContext = null)
        {
            this.eventSubscriptions = this.AddEventListeners().ToImmutableHashSet();

            if (initialContext != null)
            {
                this.Context = initialContext;
            }
        }

        /// <inheritdoc />
        public void TransitionTo(QuestStatus newStatus)
        {
            if (newStatus == this.Status)
            {
                return;
            }

            switch (newStatus)
            {
                case QuestStatus.Active when new[]
                    {
                        QuestStatus.Locked,
                        QuestStatus.Done,
                        QuestStatus.Failed,
                    }
                    .Contains(this.Status):

                case QuestStatus.Locked when new[]
                    {
                        QuestStatus.Active,
                        QuestStatus.Done,
                        QuestStatus.Failed,
                    }
                    .Contains(this.Status):

                    this.Status = newStatus;

                    break;

                default:
                    throw new QuestTransitionException(this, newStatus, this.Status);
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            this.RemoveEventListeners();
        }

        /// <summary>
        /// Adds event listeners to the given event aggregator.
        /// </summary>
        /// <returns>List of resulting cancelable subscriptions.</returns>
        protected abstract IEnumerable<ISubscription> AddEventListeners();

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
        /// Unsubscribes from all listening events.
        /// </summary>
        protected void RemoveEventListeners()
        {
            var subscriptions = this.eventSubscriptions;

            this.eventSubscriptions = this.eventSubscriptions.Clear();

            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }
    }
}
