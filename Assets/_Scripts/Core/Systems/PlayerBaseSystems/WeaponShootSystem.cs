using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public class WeaponShootSystem : ISystem
    {
        private readonly PlayerInput _playerInput;
        private Filter _filter;
        private Stash<PlayerComponent> _playerStash;
        private Stash<WeaponComponent> _weaponsStash;

        private Event<AnimationEvents> _animationEvents;
        private Event<WeaponEvent> _weaponEvent;
        
        private readonly WeaponsConfig _weaponsConfig;

        public World World { get; set; }

        public WeaponShootSystem(PlayerInput playerInput, WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
            _playerInput = playerInput;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().Build();
            _weaponsStash = World.GetStash<WeaponComponent>();
            _playerStash = World.GetStash<PlayerComponent>();

            _weaponEvent = World.GetEvent<WeaponEvent>();
            _animationEvents = World.GetEvent<AnimationEvents>();
            _animationEvents.Subscribe(OnTriggersAnimation);
            _playerInput.OnFoot.Shoot.started += OnShoot;
            _playerInput.OnFoot.Reload.performed += Reload;
        }

        private void OnTriggersAnimation(FastList<AnimationEvents> events)
        {
            foreach (var e in events)
            {
                if (e.Trigger == AnimationTrigger.ReloadEnd)
                {
                    CompleteReload();
                }
            }
        }

        private void CompleteReload()
        {
            foreach (var playerEntity in _filter)
            {
                ref var playerComponent = ref _playerStash.Get(playerEntity);
                var weaponEntity = playerComponent.WeaponEntity;
                ref WeaponComponent weaponComponent = ref _weaponsStash.Get(weaponEntity);
                if (!weaponComponent.TryReload())
                {
                    return;
                }

                weaponComponent.Reload();
            }
        }

        private void Reload(InputAction.CallbackContext context)
        {
            foreach (var playerEntity in _filter)
            {
                ref var playerComponent = ref _playerStash.Get(playerEntity);
                var weaponEntity = playerComponent.WeaponEntity;
                
                if (WeaponIsNotExist(weaponEntity)) 
                    continue;
                
                ref WeaponComponent weaponComponent = ref _weaponsStash.Get(weaponEntity);
                if (!weaponComponent.TryReload())
                {
                    CustomDebug.Log("Can't Reload. Is Empty!", Color.yellow);
                    return;
                }
            
                _weaponEvent.NextFrame(new WeaponEvent(Trigger.Reload, weaponEntity));
            }
        }

        private void OnShoot(InputAction.CallbackContext obj)
        {
            foreach (var playerEntity in _filter)
            {
                ref var playerComponent = ref _playerStash.Get(playerEntity);
                var weaponEntity = playerComponent.WeaponEntity;

                if (WeaponIsNotExist(weaponEntity)) 
                    continue;

                ref var weaponComponent = ref _weaponsStash.Get(weaponEntity);
                var weaponType = weaponComponent.TypeWeapon;

                if (!_weaponsConfig.Weapons.TryGetValue(weaponType, out var config))
                {
                    Debug.LogWarning($"Weapon config not found for {weaponType}!");
                    continue;
                }

                if (!weaponComponent.TryShoot())
                {
                    CustomDebug.Log("Is Empty! Try Reload [R]!", Color.yellow);
                    return;    
                }
                
                Shoot(ref weaponComponent, config);
                _weaponEvent.NextFrame(new WeaponEvent(Trigger.Shoot, weaponEntity));
            }
        }

        private bool WeaponIsNotExist(Entity weaponEntity)
        {
            if (weaponEntity.IsNullOrDisposed() || !_weaponsStash.Has(weaponEntity))
            {
                CustomDebug.Log("Weapon entity has no PlayerWeaponComponent. Maybe no have Weapon!", Color.yellow);
                return true;
            }

            return false;
        }

        private static void Shoot(ref WeaponComponent weaponComponent, WeaponConfig config)
        {
            weaponComponent.OnShoot();
            var shootPoint = weaponComponent.ShootPoint;
            var bulletPrefab = config.BulletPrefab;
            var speed = config.SpeedBullet;

            if (bulletPrefab == null || shootPoint == null)
            {
                Debug.LogError("Missing bullet prefab or shoot point.");
                return;
            }

            var bullet = Object.Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            if (bullet.TryGetComponent<Rigidbody>(out var rb))
            {
                var direction = shootPoint.forward.normalized;
                rb.velocity = direction * speed;
            }
            bullet.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
            _playerInput.OnFoot.Shoot.started -= OnShoot;
            _playerInput.OnFoot.Reload.performed -= Reload;
        }
    }
}