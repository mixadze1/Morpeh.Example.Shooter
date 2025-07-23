using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Scripts.Core.Systems.Bullets
{
    public sealed class BulletSystem : ISystem
    {
        private Filter _filter;
        private Stash<BulletComponent> _bulletStash;
        private Stash<TransformComponent> _transformStash;
        private readonly WeaponsConfig _weaponsConfig;

        public World World { get; set; }

        public BulletSystem(WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BulletComponent>().Build();
            _bulletStash = World.GetStash<BulletComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var bullet = ref _bulletStash.Get(entity);

                bullet.TimeAlive += deltaTime;

                ref var transformComponent = ref _transformStash.Get(entity);
                
                if (bullet.TimeAlive >= bullet.Lifetime)
                {
                    Object.Destroy(transformComponent.Transform.gameObject);
                    continue;
                }

                var bulletGameObject = transformComponent.Transform.gameObject;
                if (bulletGameObject == null)
                {
                    Debug.LogError($"Not find bullet gameObject on {entity.Id}!");
                    continue;
                }

                var position = bulletGameObject.transform.position;
                var direction = bulletGameObject.transform.forward;

                if (Physics.Raycast(position, direction, out var hitInfo, 0.5f))
                {
                    if (hitInfo.collider.gameObject.layer == 6)
                    {
                        if (_weaponsConfig.Weapons.TryGetValue(bullet.WeaponType, out var weaponCfg))
                        {
                            const float DecalOffset = 0.01f;
                            var hitPoint = hitInfo.point + hitInfo.normal * DecalOffset;
                            var instance = Object.Instantiate(weaponCfg.DecalWallProvider, hitPoint,
                                Quaternion.LookRotation(hitInfo.normal));
                          
                            ref var component = ref instance.GetData();
                            component.Lifetime = _weaponsConfig.LifetimeDecal;
                        }

                        Object.Destroy(bulletGameObject);
                    }
                }
            }
        }

        public void Dispose() { }
    }
}
