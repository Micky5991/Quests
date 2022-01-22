using System;
using System.Collections.Immutable;
using System.Text;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Quests.Example.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; }

        public IImmutableSet<IQuestRootNode> Quests { get; private set; } = ImmutableHashSet<IQuestRootNode>.Empty;

        protected Entity()
        {
            this.Id = Guid.NewGuid();
        }

        public void AddQuest(IQuestRootNode quest)
        {
            this.Quests = this.Quests.Add(quest);

            Console.WriteLine($"{this.Id} received Quest: {quest.Title}");
        }

        public void PrintQuestGoals()
        {
            Console.WriteLine("== Current Quests ==");

            var stringBuilder = new StringBuilder();

            void PrintQuestGoal(IQuestChildNode childNode, int depth)
            {
                stringBuilder.AppendLine($"{new string('|', depth)} {childNode.Title} ({childNode.Status.ToString()})");

                if (childNode is not IQuestCompositeNode compositeNode)
                {
                    return;
                }

                foreach (var compositeNodeChild in compositeNode.ChildNodes)
                {
                    PrintQuestGoal(compositeNodeChild, depth + 1);
                }
            }

            foreach (var quest in this.Quests)
            {
                stringBuilder.AppendLine($"Quest: {quest.Title} ({quest.Status.ToString()})");

                if (quest.ChildNode == null)
                {
                    continue;
                }

                PrintQuestGoal(quest.ChildNode, 1);
            }

            Console.WriteLine(stringBuilder.ToString());
        }

        public override string ToString()
        {
            return $"{this.GetType()}: {this.Id}";
        }
    }
}
