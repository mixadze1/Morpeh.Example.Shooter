using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class InspectWeaponSystem : ISystem
    {
        private readonly PlayerInput _playerInput;
        private Filter _filter;
        private Stash<PlayerComponent> _playerStash;
        private Event<WeaponEvent> _weaponEvent;

        public World World { get; set; }

        public InspectWeaponSystem(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().Build();
            _playerStash = World.GetStash<PlayerComponent>();

            _weaponEvent = World.GetEvent<WeaponEvent>();
            
            _playerInput.OnFoot.InspectWeapon.performed += OnInspectWeapon;
        }

        private void OnInspectWeapon(InputAction.CallbackContext context)
        {
            CustomDebug.Log("Inspect weapon");
            foreach (var e in _filter)
            {
                ref var playerComponent = ref _playerStash.Get(e);
                if (playerComponent.WeaponEntity.IsNullOrDisposed())
                {
                    CustomDebug.Log($"Not find current weapon! Maybe is empty!");
                    continue;
                }

                _weaponEvent.NextFrame(new WeaponEvent(Trigger.Idle_Other, playerComponent.WeaponEntity));
            }
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
            _playerInput.OnFoot.InspectWeapon.performed -= OnInspectWeapon;

        }
    }
}