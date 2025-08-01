using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct TakeDamageEvent : IEventData
    {
        public Entity Target { get; private set; }
        public int Damage { get; private set; }

        public TakeDamageEvent(Entity to, int damage)
        {
            Target = to;
            Damage = damage;
        }
    }
}