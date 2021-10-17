using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Interfaces.Services;

public interface IQuestFactory
{
    IQuestRootNode BuildQuest<T>()
        where T : IQuestRootNode;
}
