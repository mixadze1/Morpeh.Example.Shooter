using _Scripts.Core.Events;
using _Scripts.Core.Providers.View;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public class ViewAmmoSystem : ISystem
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
            _weaponEvent.Subscribe(OnWeaponEvents);

            _animationEvents = World.GetEvent<AnimationEvents>();
            _animationEvents.Subscribe(OnEndReload);
            
            _onSpawnWeapon = World.GetEvent<PlayerSpawnWeaponEvent>();
            _onSpawnWeapon.Subscribe(OnSpawnWeapon);

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

        private void OnEndReload(FastList<AnimationEvents> events)
        {
            foreach (var e in events)
            {
                ref var weaponComponent = ref _weaponStash.Get(e.WeaponEntity);
                UpdateWeaponView(ref weaponComponent);
            }
        }

        private void OnSpawnWeapon(FastList<PlayerSpawnWeaponEvent> events)
        {
            foreach (var e in events)
            {
                ref var weaponComponent = ref _weaponStash.Get(e.WeaponEntity);
                UpdateWeaponView(ref weaponComponent);
            }
        }

        private void OnWeaponEvents(FastList<WeaponEvent> events)
        {
            foreach (var e in events)
            {
                if (e.Trigger == Trigger.Shoot)
                {
                    ref var weaponComponent = ref _weaponStash.Get(e.Entity);
                    UpdateWeaponView(ref weaponComponent);
                }
            }
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

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
        }
    }
}