namespace Micky5991.Quests.Interfaces.Nodes;

public interface IQuestChildNode : IQuestNode
{
    public IQuestRootNode RootNode { get; }

    public bool CanMarkAsActive();

    public bool CanMarkAsSleeping();

    public bool CanMarkAsFailure();

    public void MarkAsActive();

    public void MarkAsSleeping();

    public void MarkAsFailure();
}
