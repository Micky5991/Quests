namespace Micky5991.Quests.Interfaces.Nodes;

public interface IQuestChildNode : IQuestNode
{
    public IQuestRootNode RootNode { get; }

    public bool Deactivated { get; }

    public bool CanActivate();

    public bool CanDeactivate();

    public void Activate();

    public void Deactivate();
}
