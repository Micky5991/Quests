using System.ComponentModel;
using Dawn;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Quests.Childs
{
    public class KillTask : QuestTaskNode
    {
        public const int RequiredKills = 5;

        private readonly IEventAggregator eventAggregator;

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

        public KillTask(IQuestRootNode rootNode, IEventAggregator eventAggregator)
            : base(rootNode)
        {
            this.eventAggregator = eventAggregator;
        }

        public override void Initialize()
        {
            this.PropertyChanged += this.HandlePropertyChange;

            this.UpdateTitle();
        }

        protected override IEnumerable<ISubscription> AttachEventListeners()
        {
            yield return this.eventAggregator.Subscribe<KillEvent>(this.OnPlayerKill);
        }

        private void UpdateTitle()
        {
            this.Title = $"Kill {RequiredKills - this.kills} enemies";
        }

        private void OnPlayerKill(KillEvent eventdata)
        {
            this.Kills = Math.Min(RequiredKills, this.Kills + 1);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            this.PropertyChanged -= this.HandlePropertyChange;
        }

        private void HandlePropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.Kills):
                    this.UpdateTitle();

                    break;
            }
        }
    }
}
