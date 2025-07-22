using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct WeaponEvent : IEventData
    {
        public Trigger Trigger { get; private set; }
        public Entity Entity { get; private set; }

        public WeaponEvent(Trigger trigger, Entity entity)
        {
            Trigger = trigger;
            Entity = entity;
        }
    }
}