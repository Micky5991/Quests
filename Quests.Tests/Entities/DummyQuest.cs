using System;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class DummyQuest : QuestRootNode
{
    public bool Initialized { get; private set; }

    public DummyQuest(string title, Func<DummyQuest, IQuestChildNode> setup)
        : this(title)
    {
        this.SetChildQuests(setup(this));
    }

    public DummyQuest(string title)
    {
        this.Title = title;
    }

    public DummyQuest(Func<DummyQuest, IQuestChildNode> setup)
        : this("Example Quest", setup)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        this.Initialized = true;
    }
}
