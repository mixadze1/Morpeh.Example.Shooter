using _Scripts.Core.Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Core
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private HealthConfig _healthConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_movementConfig);
            builder.RegisterInstance(_healthConfig);
            builder.RegisterInstance(_cameraConfig);
            builder.Register<PlayerInput>(Lifetime.Singleton);
        }
    }
}
