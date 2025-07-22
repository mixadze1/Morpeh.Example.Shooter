using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct WeaponEvent : IEventData
    {
        public Trigger Trigger { get; private set; }
        public Entity Entity { get; private set; }

        public bool IsMainPlayer { get; private set; }
        
        public WeaponEvent(Trigger trigger, Entity entity, bool isPlayer = true)
        {
            Trigger = trigger;
            Entity = entity;
            IsMainPlayer = true;
        }
    }
}