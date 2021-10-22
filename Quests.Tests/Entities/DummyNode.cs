using Micky5991.Quests.Entities;
using Micky5991.Quests.Enums;

namespace Micky5991.Quests.Tests.Entities;

public class DummyNode : QuestNode
{
    public void ForceSetState(QuestStatus newStatus)
    {
        if (newStatus == QuestStatus.Success)
        {
            this.MarkAsSuccess();
        }
        else
        {
            this.SetStatus(newStatus);
        }
    }

    public bool ForceCanSetState(QuestStatus newStatus)
    {
        if (newStatus == QuestStatus.Success)
        {
            return this.CanMarkAsSuccess();
        }

        return this.CanSetToStatus(newStatus);
    }

    public void SetTitle(string? title)
    {
        this.Title = title!;
    }
}
