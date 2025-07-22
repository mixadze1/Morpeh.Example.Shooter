using _Scripts.Core.Configs.WeaponConfigs;
using Scellecs.Morpeh;

namespace _Scripts.Core.Events
{
    public struct PlayerSpawnWeaponEvent : IEventData
    {
        public Entity WeaponEntity { get; private set; }
        public WeaponConfig WeaponConfig { get; private set; }

        public PlayerSpawnWeaponEvent(Entity entity, WeaponConfig weaponConfig)
        {
            WeaponEntity = entity;
            WeaponConfig = weaponConfig;
        }
    }
}