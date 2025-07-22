using _Scripts.Core.Configs;
using _Scripts.Core.Configs.WeaponConfigs;
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
        [SerializeField] private PlayerInteractConfig  _interactConfig;
        [SerializeField] private WeaponsConfig  _weaponsConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_movementConfig);
            builder.RegisterInstance(_healthConfig);
            builder.RegisterInstance(_cameraConfig);
            builder.RegisterInstance(_interactConfig);
            builder.RegisterInstance(_weaponsConfig);
            builder.Register<PlayerInput>(Lifetime.Singleton);
        }
    }
}
