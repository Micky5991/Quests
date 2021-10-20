using Dawn;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Entities;

/// <summary>
/// Basic quest meta information model. Will be used to register all quests to the DI container.
/// </summary>
public class QuestMeta
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestMeta"/> class.
    /// </summary>
    /// <param name="type">Implementation type of this quest.</param>
    public QuestMeta(Type type)
    {
        Guard.Argument(type, nameof(type)).NotNull();

        this.Type = type;
    }

    /// <summary>
    /// Gets the implementation type of specific quest to use.
    /// </summary>
    public Type Type { get; }
}
