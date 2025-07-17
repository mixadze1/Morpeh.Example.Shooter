using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct PlayerLookInteractItemEvent : IEventData
    {
        public Entity InteractedWith { get; private set; }

        public PlayerLookInteractItemEvent(Entity entity)
        {
            InteractedWith = entity;
        }
    }
}