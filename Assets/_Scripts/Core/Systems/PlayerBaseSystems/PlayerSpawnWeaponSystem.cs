using System.Collections.Generic;
using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class PlayerSpawnWeaponSystem : ISystem
    {
        private readonly WeaponsConfig _weaponsConfig;

        private Stash<WeaponInteractComponent> _interactWeaponStash;
        private Stash<PlayerComponent> _playerComponentStash;
        private Stash<AnimancerCustomComponent> _animancerStash;

        private Filter _filter;

        private PlayerWeaponProvider _previousWeapon;
        private Event<PlayerSpawnWeaponEvent> _weaponSpawnEvent;
        private Event<WeaponEvent> _weaponEvent; 

        public World World { get; set; }

        public PlayerSpawnWeaponSystem(WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().Build();

            _interactWeaponStash = World.GetStash<WeaponInteractComponent>();
            _playerComponentStash = World.GetStash<PlayerComponent>();
            _animancerStash = World.GetStash<AnimancerCustomComponent>();

            _weaponSpawnEvent = World.GetEvent<PlayerSpawnWeaponEvent>();
            _weaponEvent = World.GetEvent<WeaponEvent>();

            var onClick = World.GetEvent<PlayerClickInteractedEvent>();
            onClick.Subscribe(OnClickInteractItem);
        }

        private void OnClickInteractItem(FastList<PlayerClickInteractedEvent> triggers)
        {
            foreach (var trigger in triggers)
            {
                var entity = trigger.InteractedWith;

                if (entity.IsNullOrDisposed() || !_interactWeaponStash.Has(entity))
                    continue;

                var type = _interactWeaponStash.Get(entity).Type;
                SpawnWeapon(type);
            }
        }

        private void SpawnWeapon(TypeWeapon type)
        {
            if (!_weaponsConfig.Weapons.TryGetValue(type, out var config))
            {
                Debug.LogError($"Weapon config not found for {type}");
                return;
            }

            foreach (var e in _filter)
            {
                ref var player = ref _playerComponentStash.Get(e);
                
                DestroyOldWeapon(type);
                
                var weapon = Object.Instantiate(config.WeaponPreset, player.PointForSpawnWeapon);
                weapon.transform.localPosition = Vector3.zero;

                _previousWeapon = weapon;
                ref var weaponComponent = ref weapon.GetData();
                weaponComponent.InitializeAmmo(config.MagazineConfig);
                player.WeaponEntity = weapon.Entity;
                
                _weaponSpawnEvent.NextFrame(new PlayerSpawnWeaponEvent(weapon.Entity, config));
                _weaponEvent.NextFrame(new WeaponEvent(Trigger.Get, weapon.Entity));
            }
        }

        private void DestroyOldWeapon(TypeWeapon type)
        {
            if (_previousWeapon != null)
            {
                Debug.Log($"Previous Weapon: {_previousWeapon}");
                Object.Destroy(_previousWeapon.gameObject);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            
        }

        public void Dispose()
        {

        }
    }
}
