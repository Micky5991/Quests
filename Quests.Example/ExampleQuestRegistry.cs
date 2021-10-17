using System.Collections.Generic;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Example.Quests;
using Micky5991.Quests.Services;

namespace Micky5991.Quests.Example
{
    public class ExampleQuestRegistry : QuestRegistry
    {
        protected override IEnumerable<QuestMeta> BuildAvailableQuestMeta()
        {
            yield return this.BuildQuest<KillQuest>();
        }
    }
}
