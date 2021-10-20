using System;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Tests.Entities;

public class ExampleQuest : QuestRootNode
{
    public bool Initialized { get; private set; }

    public ExampleQuest(string title, Func<ExampleQuest, IQuestChildNode> setup)
        : this(title)
    {
        this.SetChildQuests(setup(this));
    }

    public ExampleQuest(string title)
    {
        this.Title = title;
    }

    public override void Initialize()
    {
        base.Initialize();

        this.Initialized = true;
    }
}
