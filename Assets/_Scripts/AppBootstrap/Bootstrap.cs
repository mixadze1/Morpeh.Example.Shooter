using _Scripts.Core.Configs;
using _Scripts.Core.Configs.PlayerConfigs;
using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Systems.Bullets;
using _Scripts.Core.Systems.Other;
using _Scripts.Core.Systems.PlayerBaseSystems;
using _Scripts.Core.Systems.PlayerInteractSystems;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Scripts.AppBootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private World _world;
        private CameraConfig _cameraConfig;
        private MovementConfig _movementConfig;
        private PlayerInput _playerInput;
        private PlayerInteractConfig _playerInteractConfig;
        private WeaponsConfig _weaponsConfig;
        private HealthConfig _healthConfig;

        [Inject]
        public void Construct(PlayerInput input, CameraConfig cameraConfig, MovementConfig movementConfig,
            PlayerInteractConfig playerInteractConfig, WeaponsConfig weaponsConfig, HealthConfig healthConfig)
        {
            _healthConfig = healthConfig;
            _weaponsConfig = weaponsConfig;
            _playerInteractConfig = playerInteractConfig;
            _playerInput = input;
            _movementConfig = movementConfig;
            _cameraConfig = cameraConfig;
        }
        
        private void Start()
        {
            _world = World.Default;

            var systemsGroup = _world.CreateSystemsGroup();
            
            systemsGroup.AddSystem(new PlayerMovementSystem(_movementConfig, _playerInput));
            systemsGroup.AddSystem(new PlayerLookSystem(_cameraConfig, _playerInput));
            systemsGroup.AddSystem(new PlayerInteractSystem(_playerInteractConfig, _playerInput));
            systemsGroup.AddSystem(new ShowWeaponInteractViewSystem());
            systemsGroup.AddSystem(new PlayerSpawnWeaponSystem(_weaponsConfig));
            systemsGroup.AddSystem(new WeaponShootSystem(_playerInput, _weaponsConfig));
            systemsGroup.AddSystem(new AnimationWeaponSystem());
            systemsGroup.AddSystem(new ViewAmmoSystem());
            systemsGroup.AddSystem(new InspectWeaponSystem(_playerInput));
            systemsGroup.AddSystem(new BulletSystem(_weaponsConfig));
            systemsGroup.AddSystem(new DecalBulletSystem());
            systemsGroup.AddSystem(new CursorLockSystem());
            systemsGroup.AddSystem(new HealthSystem(_healthConfig));

            this._world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
