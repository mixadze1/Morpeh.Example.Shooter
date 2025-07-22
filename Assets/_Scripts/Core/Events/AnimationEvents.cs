using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct AnimationEvents : IEventData
    {
        public Entity WeaponEntity { get; private set; }
        public AnimationTrigger Trigger { get; private set; }

        public AnimationEvents(AnimationTrigger trigger, Entity weaponEntity)
        {
            WeaponEntity = weaponEntity;
            Trigger = trigger;
        }
    }

    public enum AnimationTrigger
    {
        ReloadEnd
    }
}