using System;
using System.Collections.Immutable;
using System.Text;
using Micky5991.Quests.Interfaces.Entities;

namespace Micky5991.Quests.Example.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; }

        public IImmutableSet<IQuest> Quests { get; private set; } = ImmutableHashSet<IQuest>.Empty;

        protected Entity()
        {
            this.Id = Guid.NewGuid();
        }

        public void AddQuest(IQuest quest)
        {
            this.Quests = this.Quests.Add(quest);

            Console.WriteLine($"{this.Id} received Quest: {quest.Title} ({quest.Description})");
        }

        public void PrintQuestGoals()
        {
            Console.WriteLine("Quest Goals:");

            var stringBuilder = new StringBuilder();

            foreach (var quest in this.Quests)
            {
                stringBuilder.AppendLine($"Quest: {quest.Title} ({quest.Status.ToString()})");
                stringBuilder.AppendLine($"- {quest.Description}");
            }

            Console.WriteLine(stringBuilder.ToString());
        }

        public override string ToString()
        {
            return $"{this.GetType()}: {this.Id}";
        }
    }
}
