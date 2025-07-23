using _Scripts.Core.Configs;
using _Scripts.Core.Events;
using _Scripts.Core.Providers;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Core.Systems.PlayerInteractSystems
{
    public sealed class PlayerInteractSystem : ISystem
    {
        private readonly PlayerInteractConfig _playerInteractConfig;
        private Stash<InteractComponent> _interactStash;
        
        private Filter _filterPlayer;
        private Stash<PlayerCameraComponent> _playerCameraStash;

        private Event<PlayerClickInteractedEvent> _eventClick;
        private Event<PlayerLookInteractItemEvent> _eventLook;
        private readonly PlayerInput _playerInput;
        
        private Entity _lastLookedEntity;
        
        public World World { get; set; }

        public PlayerInteractSystem(PlayerInteractConfig playerInteractConfig, PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _playerInteractConfig = playerInteractConfig;
        }

        public void OnAwake()
        {
            _eventClick = World.GetEvent<PlayerClickInteractedEvent>();
            _eventLook = World.GetEvent<PlayerLookInteractItemEvent>();
            
            _interactStash = World.GetStash<InteractComponent>();

            _filterPlayer = World.Filter.With<PlayerCameraComponent>().Build();
            _playerCameraStash = World.GetStash<PlayerCameraComponent>();

            _playerInput.OnFoot.Interact.performed += OnClick;
        }

        public void OnUpdate(float deltaTime)
        {
            Entity lookedEntity;
            if (TryInteract(out lookedEntity))
            {
                if (lookedEntity != _lastLookedEntity)
                {
                    _eventLook.NextFrame(new PlayerLookInteractItemEvent(lookedEntity));
                    _lastLookedEntity = lookedEntity;
                    CustomDebug.Log($"[Update Player] Look on Item: {lookedEntity.Id}", Color.white);
                }
                return; 
            }

            if (_lastLookedEntity != default && !World.IsDisposed(_lastLookedEntity))
            {
                _eventLook.NextFrame(new PlayerLookInteractItemEvent(default));
                _lastLookedEntity = default;
                CustomDebug.Log($"[Update Player] Look cleared", Color.gray);
            }
        }

        private void OnClick(InputAction.CallbackContext obj)
        {
            CustomDebug.Log("[Player] Click E", Color.white);
            if (TryInteract(out Entity entity))
            {
                _eventClick.NextFrame(new PlayerClickInteractedEvent(entity));
                CustomDebug.Log($"[Player] Interact item {entity.Id}", Color.white);
            }
        }

        private bool TryInteract(out Entity entity)
        {
            entity = default;

            foreach (var player in _filterPlayer)
            {
                ref var cameraComponent = ref _playerCameraStash.Get(player);
                GetOriginAndDirection(ref cameraComponent, out Vector3 origin, out Vector3 direction);
                Debug.DrawRay(origin, direction * _playerInteractConfig.InteractDistance, Color.yellow);
                if (Physics.Raycast(origin, direction, out var hit, _playerInteractConfig.InteractDistance,
                        _playerInteractConfig.InteractLayerMask))
                {
                    var interactProvider = hit.collider.GetComponent<InteractProvider>();
                    if (interactProvider != null)
                    {
                        entity = interactProvider.Entity;
                        if (_interactStash.Has(entity))
                        {
                            return true;
                        }
                    }
                }
            }

            return false; 
        }

        private void GetOriginAndDirection(ref PlayerCameraComponent cameraComponent, out Vector3 origin, out Vector3 direction)
        {
            var cameraTransform = cameraComponent.VirtualCamera.transform;
            origin = cameraTransform.position;
            direction = cameraTransform.forward;
        }

        public void Dispose()
        {
            _playerInput.OnFoot.Interact.performed -= OnClick;
        }
    }
}