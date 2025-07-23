using _Scripts.Core.Configs;
using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Systems;
using _Scripts.Core.Systems.Bullets;
using _Scripts.Core.Systems.PlayerBaseSystems;
using _Scripts.Core.Systems.PlayerInteractSystems;
using _Scripts.Core.Systems.UI;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Scripts.Core
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private float _time;
        private World _world;
        private HealthConfig _healthConfig;
        private CameraConfig _cameraConfig;
        private MovementConfig _movementConfig;
        private PlayerInput _playerInput;
        private PlayerInteractConfig _playerInteractConfig;
        private WeaponsConfig _weaponsConfig;

        [Inject]
        public void Construct(PlayerInput input, HealthConfig healthConfig, CameraConfig cameraConfig, MovementConfig movementConfig,
            PlayerInteractConfig playerInteractConfig, WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
            _playerInteractConfig = playerInteractConfig;
            _playerInput = input;
            _movementConfig = movementConfig;
            _cameraConfig = cameraConfig;
            _healthConfig = healthConfig;
        }
        
        private void Start() {
            this._world = World.Default;

            Time.timeScale = _time;
            var systemsGroup = _world.CreateSystemsGroup();
            
            systemsGroup.AddSystem(new HealthSystem(_healthConfig));
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

            this._world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
