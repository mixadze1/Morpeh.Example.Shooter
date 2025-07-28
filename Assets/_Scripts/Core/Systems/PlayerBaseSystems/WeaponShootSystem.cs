using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class WeaponShootSystem : ISystem
    {
        private readonly PlayerInput _playerInput;
        private Filter _filter;
        private Stash<PlayerComponent> _playerStash;
        private Stash<WeaponComponent> _weaponsStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;

        private Event<AnimationEvents> _animationEvents;
        private Event<WeaponEvent> _weaponEvent;
        
        private readonly WeaponsConfig _weaponsConfig;
        private readonly Dictionary<Entity, float> _lastShootTimes = new();

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
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();

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
                    return;

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

                float shootDelay = config.ShootPerSecond;
                float currentTime = Time.time;
                _lastShootTimes.TryGetValue(weaponEntity, out var lastShootTime);

                CustomDebug.Log($"[Shoot Info] Delay Shoot: {currentTime - lastShootTime} < {shootDelay}", new Color(1f, 0.91f, 0.38f));
                if (currentTime - lastShootTime < shootDelay)
                    continue;

                if (!weaponComponent.TryShoot())
                {
                    CustomDebug.Log("Is Empty! Try Reload [R]!", Color.yellow);
                    return;
                }

                Shoot(ref weaponComponent, config);
                _lastShootTimes[weaponEntity] = currentTime;

                _weaponEvent.NextFrame(new WeaponEvent(Trigger.Shoot, weaponEntity));
            }
        }

        private bool WeaponIsNotExist(Entity weaponEntity)
        {
            if (World.IsDisposed(weaponEntity) || !_weaponsStash.Has(weaponEntity))
            {
                CustomDebug.Log("Weapon entity has no PlayerWeaponComponent. Maybe no have Weapon!", Color.yellow);
                return true;
            }

            return false;
        }

        private void Shoot(ref WeaponComponent weaponComponent, WeaponConfig config)
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

            var instanceBullet = Object.Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            ref var bulletComponent = ref instanceBullet.GetData();
            bulletComponent.Lifetime = _weaponsConfig.LifetimeBullet;
            bulletComponent.WeaponType = weaponComponent.TypeWeapon;

            ref var rigidbodyComponent = ref _rigidbodyStash.Get(instanceBullet.Entity);
            var rb = rigidbodyComponent.Rigidbody;

            var direction = shootPoint.forward.normalized;
            rb.velocity = direction * speed;
            instanceBullet.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        public void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            _playerInput.OnFoot.Shoot.started -= OnShoot;
            _playerInput.OnFoot.Reload.performed -= Reload;
        }
    }
}
