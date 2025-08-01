using _Scripts.Core.Configs;
using _Scripts.Core.Events;
using _Scripts.Core.Providers.Other;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;

namespace _Scripts.Core.Systems.Other
{
    public sealed class HealthSystem : ISystem
    {
        private Filter _filter;
        private Filter _filterPlayer;
        private Stash<TransformComponent> _transfomrStash;

    
        private Stash<HealthComponent> _healthStash;
        private Stash<ViewTextComponent> _viewTextStash;
        private Event<TakeDamageEvent> _damageEvent;
        private Event<DeathEvent> _deathEvent;
        private readonly HealthConfig _healthConfig;

        public World World { get; set; }

        public HealthSystem(HealthConfig healthConfig)
        {
            _healthConfig = healthConfig;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<ViewTextComponent>().Build();
            _filterPlayer = World.Filter.With<PlayerComponent>().Build();

            _transfomrStash = World.GetStash<TransformComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _viewTextStash = World.GetStash<ViewTextComponent>();
        
            _damageEvent = World.GetEvent<TakeDamageEvent>();
            _deathEvent = World.GetEvent<DeathEvent>();

            foreach (var f in _filter)
            {
                ref var c = ref _viewTextStash.Get(f);
                c.HideImmediate();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var c in _damageEvent.publishedChanges)
            {
                OnDamage(c);
            }
        }

        private void OnDamage(TakeDamageEvent e)
        {
            if (_healthStash.Has(e.Target))
            {
                ref var health = ref _healthStash.Get(e.Target);
                health.GetDamage(e.Damage);

                if (_viewTextStash.Has(e.Target))
                {
                    ViewGamade(e);
                }

                if (health.IsDeath())
                    _deathEvent.NextFrame(new DeathEvent(e.Target));
            }
        }

        private void ViewGamade(TakeDamageEvent e)
        {
            ref var viewTextComponent = ref _viewTextStash.Get(e.Target);
            viewTextComponent.SetupDurations(_healthConfig.DurationShow, _healthConfig.DurationHide);
            viewTextComponent.ShowText($"-{e.Damage}", viewTextComponent.Hide);

            foreach (var p in _filterPlayer)
            {
                ref var c = ref _transfomrStash.Get(p);

                var canvasTransform = viewTextComponent.CanvasGroup.transform;
                canvasTransform.LookAt(c.Transform);
                canvasTransform.Rotate(0, 180, 0);
            }
        }


        public void Dispose() { }
    }
}