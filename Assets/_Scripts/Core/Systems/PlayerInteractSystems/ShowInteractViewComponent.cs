using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace _Scripts.Core.Systems.PlayerInteractSystems
{
    public sealed class ShowWeaponInteractViewSystem : ISystem
    {
        private Stash<WeaponInteractComponent> _interactWeaponStash;
        private Stash<ShowInteractViewComponent> _viewInteractStash;

        private Event<PlayerLookInteractItemEvent> _eventLook;

        public World World { get; set; }

        public void OnAwake()
        {
            _interactWeaponStash = World.GetStash<WeaponInteractComponent>();
            _viewInteractStash = World.GetStash<ShowInteractViewComponent>();
            _eventLook = World.GetEvent<PlayerLookInteractItemEvent>();
            
            Hide();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in _eventLook.publishedChanges)
            {
                OnTrigger(e);
            }
        }

        private void OnTrigger(PlayerLookInteractItemEvent trigger)
        {
                var entity = trigger.InteractedWith;

                if (World.IsDisposed(entity))
                {
                    Hide();
                    return;
                }

                if (_interactWeaponStash.Has(entity))
                {
                    ref var interactWeapon = ref _interactWeaponStash.Get(entity);
                    string itemName  = interactWeapon.Type.ToString();
                    Show(itemName);
                }
                else
                {
                    Hide();
                }
        }

        private void Show(string itemName)
        {
            foreach (var v in _viewInteractStash) 
                v.Show($"Click [E], to Get: {itemName}");
        }

        private void Hide()
        {
            foreach (var v in _viewInteractStash) 
                v.Hide();
        }

        public void Dispose()
        {
        }
    }
}
