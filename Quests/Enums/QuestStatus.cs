namespace Micky5991.Quests.Enums;

/// <summary>
/// List of states available of the current quest node.
/// </summary>
public enum QuestStatus
{
    /// <summary>
    /// Quest is currently not ready to listen to events and waits for activation.
    /// </summary>
    Sleeping,

    /// <summary>
    /// Quest is waiting to be completed and listens for events.
    /// </summary>
    Active,

    /// <summary>
    /// Quest has been finished successfully.
    /// </summary>
    Success,

    /// <summary>
    /// Quest has been finished, but to a failed status.
    /// </summary>
    Failure,
}
