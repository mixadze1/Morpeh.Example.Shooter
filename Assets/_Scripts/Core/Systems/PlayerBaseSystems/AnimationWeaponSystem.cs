using _Scripts.Core.Events;
using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public sealed class AnimationWeaponSystem : ISystem
    {
        private Filter _filter;
        private Stash<AnimancerCustomComponent> _animancerStash;
        private Stash<PlayerComponent> _playerStash;
        private Event<WeaponEvent> _weaponEvents;
        private Event<AnimationEvents> _animationEvents;
        private Event<PlayerSpawnWeaponEvent> _spawnWeapon;
        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().Build();
            _animancerStash = World.GetStash<AnimancerCustomComponent>();
            _playerStash = World.GetStash<PlayerComponent>();
            
           _weaponEvents = World.GetEvent<WeaponEvent>();
           _weaponEvents.Subscribe(OnTrigger);

           _animationEvents = World.GetEvent<AnimationEvents>();

           _spawnWeapon = World.GetEvent<PlayerSpawnWeaponEvent>();
           _spawnWeapon.Subscribe(OnSpawnWeapon);
        }

        private void OnSpawnWeapon(FastList<PlayerSpawnWeaponEvent> triggers)
        {
            foreach (var t in triggers)
            {
                ref var animancerComponent = ref _animancerStash.Get(t.WeaponEntity);
                animancerComponent.InitAnimations(t.WeaponConfig.AnimationConfig);
                AddAnimationTransition(animancerComponent, Trigger.Get, Trigger.Idle);
                AddAnimationTransition(animancerComponent, Trigger.Shoot, Trigger.Idle);
                AddAnimationTransition(animancerComponent, Trigger.Idle_Other, Trigger.Idle);
                AddAnimationTransition(animancerComponent, Trigger.Reload, Trigger.Idle);
                AddAnimationDispatch(animancerComponent, Trigger.Reload, AnimationTrigger.ReloadEnd, t.WeaponEntity);
            }
        }

        private void AddAnimationDispatch(AnimancerCustomComponent animancerComponent, Trigger fromTrigger,
            AnimationTrigger trigger, Entity weaponEntity)
        {
            var transition = animancerComponent.GetAnimation(fromTrigger);
            transition.Events.OnEnd +=
                () => _animationEvents.NextFrame(new AnimationEvents(trigger, weaponEntity));
        }

        private void OnTrigger(FastList<WeaponEvent> triggers)
        {
            foreach (var t in triggers)
            {
                ref var animancerComponent = ref _animancerStash.Get(t.Entity);
                PlayAnimation(animancerComponent, t.Trigger);
            }
        }

        public AnimancerState PlayAnimation(AnimancerCustomComponent animancerComponent, Trigger trigger)
        {
            var transition = animancerComponent.GetAnimation(trigger);
            var state = animancerComponent.AnimancerComponent.Play(transition);
            state.Time = 0f;
            animancerComponent.CurrentAnim = trigger;
            CustomDebug.Log($"[Animation] Play animation {trigger}", Color.green);
            return state;
        }

        public void AddAnimationTransition(AnimancerCustomComponent animancerComponent, Trigger trigger, Trigger endState)
        {
            var transition = animancerComponent.GetAnimation(trigger);
            transition.Events.OnEnd += () => PlayAnimation(animancerComponent, endState);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
        }
    }
}