using Micky5991.EventAggregator.Elements;
using Micky5991.Quests.Example.Entities;

namespace Micky5991.Quests.Example.Events
{
    public class KillEvent : EventBase
    {
        public Entity Killer { get; }

        public Entity Victim { get; }

        public int Weapon { get; }

        public KillEvent(Entity killer, Entity victim, int weapon)
        {
            this.Killer = killer;
            this.Victim = victim;
            this.Weapon = weapon;
        }
    }
}
