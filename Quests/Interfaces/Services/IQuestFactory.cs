using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Quests.Services;

namespace Micky5991.Quests.Interfaces.Services;

/// <summary>
/// Factory that creates and initializes any registered quests. Quests should be registered inside <see cref="QuestRegistry"/>.
/// </summary>
public interface IQuestFactory
{
    /// <summary>
    /// Builds a quest from the given type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Quest type that should be built.</typeparam>
    /// <returns>Newly created quest instance.</returns>
    IQuestRootNode BuildQuest<T>()
        where T : IQuestRootNode;
}
