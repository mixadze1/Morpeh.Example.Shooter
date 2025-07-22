using System;
using System.Collections.Generic;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    public class AnimancerProvider : MonoProvider<AnimancerCustomComponent>
    {
        public void Reset()
        {
            ref var data = ref GetData();
            data.AnimancerComponent = GetComponent<AnimancerComponent>();
        }
    }

    [Serializable]
    public struct AnimancerCustomComponent : IComponent
    {
        public AnimancerComponent AnimancerComponent;
        private Dictionary<Trigger, ClipTransition> _transitions;
        public Trigger CurrentAnim { get; set; }

        public void InitAnimations(Dictionary<Trigger, AnimationData> animations)
        {
            _transitions = new(10);
            foreach (var data in animations)
            {
                var transition = new ClipTransition();
                transition.Clip = data.Value.AnimationClip;
                transition.Speed = data.Value.Speed;
                transition.FadeDuration = data.Value.FadeTime;
                _transitions[data.Key] = transition;
            }
            CustomDebug.Log($"Init animations {_transitions.Count}", Color.white);
        }


        public ClipTransition GetAnimation(Trigger trigger)
        {
            if (_transitions.ContainsKey(trigger))
            {
                return _transitions[trigger];
            }
            else
            {
                Debug.LogError($"Animation \"{trigger.ToString()}\" is not registered");
                return null;
            }
        }
    }

    [Serializable]
    public class AnimationData
    {
        public AnimationClip AnimationClip;
        public float FadeTime = 0.25f;
        public float Speed = 1f;
    }

    public enum Trigger
    {
        Default,
        Reload,
        Shoot,
        Walk,
        Idle,
        Idle_Other,
        Get
    }
}