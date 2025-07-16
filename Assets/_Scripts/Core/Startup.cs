using _Scripts.Core.Configs;
using _Scripts.Core.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Scripts.Core
{
    public class Startup : MonoBehaviour
    {
        private World world;
        private HealthConfig _healthConfig;
        private CameraConfig _cameraConfig;
        private MovementConfig _movementConfig;
        private PlayerInput _playerInput;

        [Inject]
        public void Construct(PlayerInput input, HealthConfig healthConfig, CameraConfig cameraConfig, MovementConfig movementConfig)
        {
            _playerInput = input;
            _movementConfig = movementConfig;
            _cameraConfig = cameraConfig;
            _healthConfig = healthConfig;
        }
        
        private void Start() {
            this.world = World.Default;
        
            var systemsGroup = this.world.CreateSystemsGroup();
            systemsGroup.AddSystem(new HealthSystem(_healthConfig));
            systemsGroup.AddSystem(new PlayerMovementSystem(_movementConfig, _playerInput));
            systemsGroup.AddSystem(new PlayerLookSystem(_cameraConfig, _playerInput));

            this.world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
