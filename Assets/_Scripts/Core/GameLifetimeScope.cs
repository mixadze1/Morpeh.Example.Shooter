using _Scripts.Core.Configs;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Core
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private HealthConfig _healthConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private PlayerInteractConfig _interactConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_movementConfig);
            builder.RegisterInstance(_healthConfig);
            builder.RegisterInstance(_cameraConfig);
            builder.RegisterInstance(_interactConfig);
            builder.Register<PlayerInput>(Lifetime.Singleton);
        }
    }
}
