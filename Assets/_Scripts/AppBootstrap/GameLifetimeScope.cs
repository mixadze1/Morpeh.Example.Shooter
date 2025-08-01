using _Scripts.Core.Configs;
using _Scripts.Core.Configs.PlayerConfigs;
using _Scripts.Core.Configs.WeaponConfigs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.AppBootstrap
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private PlayerInteractConfig  _interactConfig;
        [SerializeField] private WeaponsConfig  _weaponsConfig;
        [SerializeField] private HealthConfig  _healthConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_movementConfig);
            builder.RegisterInstance(_cameraConfig);
            builder.RegisterInstance(_interactConfig);
            builder.RegisterInstance(_weaponsConfig);
            builder.RegisterInstance(_healthConfig);
            builder.Register<PlayerInput>(Lifetime.Singleton);
        }
    }
}
