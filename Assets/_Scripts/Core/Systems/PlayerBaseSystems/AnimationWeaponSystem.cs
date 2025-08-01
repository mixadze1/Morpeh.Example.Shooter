using System;
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
        private Stash<AnimancerCustomComponent> _animancerStash;
        private Event<WeaponEvent> _weaponEvents;
        private Event<AnimationEvents> _animationEvents;
        private Event<PlayerSpawnWeaponEvent> _spawnWeapon;
        public World World { get; set; }

        public void OnAwake()
        {
            _animancerStash = World.GetStash<AnimancerCustomComponent>();
            
           _weaponEvents = World.GetEvent<WeaponEvent>();

           _animationEvents = World.GetEvent<AnimationEvents>();

           _spawnWeapon = World.GetEvent<PlayerSpawnWeaponEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in _spawnWeapon.publishedChanges) 
                OnSpawnWeapon(e);
            
            foreach (var e in _weaponEvents.publishedChanges) 
                OnTriggerAnim(e);
        }

        private void OnSpawnWeapon(PlayerSpawnWeaponEvent e)
        {
            ref var animancerComponent = ref _animancerStash.Get(e.WeaponEntity);
            animancerComponent.InitAnimations(e.WeaponConfig.AnimationConfig);
            AddAnimationTransition(animancerComponent, Trigger.Get, Trigger.Idle);
            AddAnimationTransition(animancerComponent, Trigger.Shoot, Trigger.Idle);
            AddAnimationTransition(animancerComponent, Trigger.Idle_Other, Trigger.Idle);
            AddAnimationTransition(animancerComponent, Trigger.Reload, Trigger.Idle);
            AddAnimationDispatch(animancerComponent, Trigger.Reload, AnimationTrigger.ReloadEnd, e.WeaponEntity);
        }

        private void AddAnimationDispatch(AnimancerCustomComponent animancerComponent, Trigger fromTrigger,
            AnimationTrigger trigger, Entity weaponEntity)
        {
            var transition = animancerComponent.GetAnimation(fromTrigger);
            transition.Events.OnEnd +=
                () => _animationEvents.NextFrame(new AnimationEvents(trigger, weaponEntity));
        }

        private void OnTriggerAnim(WeaponEvent e)
        {
            ref var animancerComponent = ref _animancerStash.Get(e.Entity);
            var state = PlayAnimation(animancerComponent, e.Trigger);
        }

        public AnimancerState PlayAnimation(AnimancerCustomComponent animancerComponent, Trigger trigger)
        {
            var transition = animancerComponent.GetAnimation(trigger);
            var state = animancerComponent.Animancer.Play(transition);
            state.Time = 0f;
            animancerComponent.CurrentAnim = trigger;
            CustomDebug.Log($"[Animation] Play animation {trigger}", new Color(0.53f, 1f, 0.51f));
            return state;
        }

        public void AddAnimationTransition(AnimancerCustomComponent animancerComponent, Trigger trigger, Trigger endState)
        {
            var transition = animancerComponent.GetAnimation(trigger);
            transition.Events.OnEnd += () => PlayAnimation(animancerComponent, endState);
        }

        public void Dispose()
        {
            
        }
    }
}