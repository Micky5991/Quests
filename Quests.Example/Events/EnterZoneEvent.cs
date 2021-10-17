using Micky5991.EventAggregator.Elements;
using Micky5991.Quests.Example.Entities;

namespace Micky5991.Quests.Example.Events;

public class EnterZoneEvent : EventBase
{
    public EnterZoneEvent(Player player, int zoneId)
    {
        this.Player = player;
        this.ZoneId = zoneId;
    }

    public Player Player { get; }

    public int ZoneId { get; }
}
