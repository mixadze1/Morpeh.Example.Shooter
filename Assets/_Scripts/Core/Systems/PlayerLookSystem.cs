using _Scripts.Core.Configs;
using _Scripts.Core.Providers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Scripts.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerLookSystem : ISystem
    {
        private readonly CameraConfig _cameraConfig;
        private Filter _filter;
        private Stash<PlayerCameraComponent> _cameraStash;
        private Stash<TransformComponent> _transfomrComponent;
        private readonly PlayerInput _playerInput;
        public World World { get; set;}

        public PlayerLookSystem(CameraConfig cameraConfig, PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _cameraConfig = cameraConfig;
        }
    
        public void OnAwake() 
        {
            _filter = World.Filter.With<PlayerCameraComponent>().Build();
            _cameraStash = World.GetStash<PlayerCameraComponent>();
            _transfomrComponent = World.GetStash<TransformComponent>();
        }
    
        public void OnUpdate(float deltaTime)
        {
            Vector2 input = _playerInput.OnFoot.Look.ReadValue<Vector2>();
            foreach (var entity in _filter)
            {
                ProcessLook(entity, deltaTime, input);
            }
        }

        public void ProcessLook(Entity entity, float deltaTime, Vector2 input)
        {
            ref PlayerCameraComponent playerCameraComponent = ref _cameraStash.Get(entity);
            ref TransformComponent transformComponent = ref _transfomrComponent.Get(entity);
            ref float xRotation = ref playerCameraComponent.XRotation;
            var camera = playerCameraComponent.VirtualCamera;
            float mouseX = input.x;
            float mouseY = input.y;
            

            var leftRightDirection = Vector3.up * mouseX * _cameraConfig.MouseSensitivity;
            transformComponent.Transform.Rotate(leftRightDirection);
            
            xRotation -= mouseY * _cameraConfig.MouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, _cameraConfig.MinX, _cameraConfig.MaxX);
            camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }

        public void Dispose()
        {

        }
    }
}