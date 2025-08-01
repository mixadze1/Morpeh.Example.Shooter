using _Scripts.Core.Events;
using _Scripts.Core.Providers.View;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class ViewAmmoSystem : ISystem
    {
        private Filter _filter;
        private Stash<WeaponComponent> _weaponStash;
        private Stash<AmmoViewComponent> _viewAmmoStash;
        private Event<WeaponEvent> _weaponEvent;
        private Event<PlayerSpawnWeaponEvent> _onSpawnWeapon;
        private Event<AnimationEvents> _animationEvents;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter.With<AmmoViewComponent>().Build();
            _weaponStash = World.GetStash<WeaponComponent>();
            _viewAmmoStash = World.GetStash<AmmoViewComponent>();
            
            _weaponEvent = World.GetEvent<WeaponEvent>();
            _animationEvents = World.GetEvent<AnimationEvents>();
            _onSpawnWeapon = World.GetEvent<PlayerSpawnWeaponEvent>();
            Hide();
        }

        private void Hide()
        {
            foreach (var c in _filter)
            {
                ref var viewAmmoComponent = ref _viewAmmoStash.Get(c);
                viewAmmoComponent.Hide();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in _weaponEvent.publishedChanges) 
                OnWeaponEvents(e);
            
            foreach (var e in _animationEvents.publishedChanges) 
                OnEndReload(e);
            
            foreach (var e in _onSpawnWeapon.publishedChanges) 
                OnSpawnWeapon(e);
        }

        private void OnEndReload(AnimationEvents e)
        {
            ref var weaponComponent = ref _weaponStash.Get(e.WeaponEntity);
            UpdateWeaponView(ref weaponComponent);
        }

        private void OnSpawnWeapon(PlayerSpawnWeaponEvent e)
        {
            ref var weaponComponent = ref _weaponStash.Get(e.WeaponEntity);
            UpdateWeaponView(ref weaponComponent);
        }

        private void OnWeaponEvents(WeaponEvent e)
        {
            if (e.Trigger != Trigger.Shoot)
                return;
            
            ref var weaponComponent = ref _weaponStash.Get(e.Entity);
            UpdateWeaponView(ref weaponComponent);
        }

        private void UpdateWeaponView(ref WeaponComponent weaponComponent)
        {
            foreach (var c in _filter)
            {
                ref var viewAmmoComponent = ref _viewAmmoStash.Get(c);
                viewAmmoComponent.Show();
                viewAmmoComponent.UpdateView(weaponComponent.AmountAmmo, weaponComponent.MaxAmmoInMagazine,
                    weaponComponent.AmountMagazines);
            }
        }

        public void Dispose()
        {
        }
    }
}