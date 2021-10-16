using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using Dawn;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;
using Microsoft.Extensions.Logging;

namespace Micky5991.Quests.Example.Quests
{
    public class KillQuest : QuestBase
    {
        public const int RequiredKills = 5;

        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<KillQuest> logger;

        private int kills;

        public int Kills
        {
            get => this.kills;
            private set
            {
                if (this.kills == value)
                {
                    return;
                }

                Guard.Argument(value, nameof(value)).Min(0);

                this.kills = value;

                this.OnPropertyChanged();
            }
        }

        public override void Initialize(IImmutableDictionary<string, object>? initialContext = null)
        {
            base.Initialize(initialContext);

            this.PropertyChanged += this.HandlePropertyChange;
        }

        public KillQuest(IEventAggregator eventAggregator, ILogger<KillQuest> logger)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;

            this.Title = "Kill enemies";

            this.UpdateDescription();
        }

        protected override IEnumerable<ISubscription> AddEventListeners()
        {
            yield return this.eventAggregator.Subscribe<KillEvent>(this.OnPlayerKill);
        }

        private void UpdateDescription()
        {
            this.Description = $"Kill {RequiredKills - this.kills} enemies";
        }

        private void OnPlayerKill(KillEvent eventdata)
        {
            if (this.Status == QuestStatus.Locked)
            {
                return;
            }

            this.logger.LogInformation("Kill registered");

            this.Kills = Math.Min(RequiredKills, this.Kills + 1);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.PropertyChanged -= this.HandlePropertyChange;
        }

        private void HandlePropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.Kills):
                    this.UpdateDescription();

                    break;
            }
        }
    }
}
