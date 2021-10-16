using System;

namespace Micky5991.Quests.Entities
{
    public class QuestMeta
    {
        public Type Type { get; }

        public QuestMeta(Type type)
        {
            this.Type = type;
        }
    }
}
