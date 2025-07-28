using _Scripts.Core.Configs;
using _Scripts.Core.Configs.PlayerConfigs;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class PlayerMovementSystem : ISystem
    {
        private readonly MovementConfig _movementConfig;
        private PlayerInput.OnFootActions _onFoot;
        public World World { get; set; }

        private Filter _filter;
        private Stash<CharacterComponent> _characterComponent;
        private Stash<TransformComponent> _transformComponent;

        public PlayerMovementSystem(MovementConfig movementConfig, PlayerInput playerInput)
        {
            _onFoot = playerInput.OnFoot;
            _movementConfig = movementConfig;
        }

        public void OnAwake() 
        {
            _onFoot.Enable();
            _filter = World.Filter.With<PlayerComponent>().Build();
            _characterComponent = World.GetStash<CharacterComponent>();
            _transformComponent = World.GetStash<TransformComponent>();
            _onFoot.Jump.performed += OnJump;
        }

        public void OnUpdate(float deltaTime) 
        {
            var input = _onFoot.Movement.ReadValue<Vector2>();
            foreach (var entity in _filter)
            {
                ProcessMove(entity, deltaTime, input);
            }
        }

        private void ProcessMove(Entity entity, float deltaTime, Vector2 input)
        {
            ref var characterComponent = ref _characterComponent.Get(entity);
            ref var transformComponent = ref _transformComponent.Get(entity);
            
            var transform = transformComponent.Transform;
            var controller = characterComponent.CharacterController;

            Vector3 move = transform.TransformDirection(new Vector3(input.x, 0, input.y)) * _movementConfig.SpeedValue;
            Vector3 copyVelocity = characterComponent.PlayerVelocity;

            copyVelocity.y += _movementConfig.Gravity * deltaTime;
            if (characterComponent.IsGround && copyVelocity.y < 0)
                copyVelocity.y = _movementConfig.OnGroundDefaultGravity;

            Vector3 final = move + new Vector3(0, copyVelocity.y, 0);
            characterComponent.SetVelocity(final);
            controller.Move(final * deltaTime);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            foreach (var entity in _filter)
            {
                ref CharacterComponent characterComponent = ref _characterComponent.Get(entity);
                if (!characterComponent.IsGround)
                    return;
                
                float yValue = Mathf.Sqrt(Mathf.Abs(_movementConfig.JumpHeight * _movementConfig.Gravity));
                characterComponent.SetVelocityY(yValue);
                CustomDebug.Log($"[Player] Jump! Gravity:{_movementConfig.Gravity}, Jump Height: {_movementConfig.JumpHeight}", new Color(0.4f, 0.93f, 1f));
            }
        }
        
        public void Dispose()
        {
            _onFoot.Jump.performed -= OnJump;
            _onFoot.Disable();
        }
    }
}