using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct PlayerClickInteractedEvent : IEventData
    {
        public Entity InteractedWith { get; private set; }

        public PlayerClickInteractedEvent(Entity entity)
        {
            InteractedWith = entity;
        }
    }
}