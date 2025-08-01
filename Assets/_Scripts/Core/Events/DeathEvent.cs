using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct DeathEvent : IEventData
    {
        public Entity Entity { get; private set; }

        public DeathEvent(Entity e)
        {
            Entity = e;
        }
    }
}