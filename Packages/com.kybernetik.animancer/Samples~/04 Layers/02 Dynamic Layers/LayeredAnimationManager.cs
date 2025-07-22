// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.Layers
{
    /// <summary>
    /// Demonstrates how to use layers to play multiple independent animations
    /// at the same time on different body parts.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/layers/dynamic">
    /// Dynamic Layers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Layers/LayeredAnimationManager
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Layers - Layered Animation Manager")]
    [AnimancerHelpUrl(typeof(LayeredAnimationManager))]
    public class LayeredAnimationManager : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AvatarMask _ActionMask;
        [SerializeField, Seconds] private float _ActionFadeDuration = AnimancerGraph.DefaultFadeDuration;

        private AnimancerLayer _BaseLayer;
        private AnimancerLayer _ActionLayer;
        private bool _CanPlayActionFullBody;

        public AnimancerComponent Animancer
            => _Animancer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _BaseLayer = _Animancer.Layers[0];
            _ActionLayer = _Animancer.Layers[1];

            _ActionLayer.Mask = _ActionMask;
            _ActionLayer.SetDebugName("Action Layer");
        }

        /************************************************************************************************************************/

        public void PlayBase(ITransition transition, bool canPlayActionFullBody)
        {
            _CanPlayActionFullBody = canPlayActionFullBody;

            if (_CanPlayActionFullBody && _ActionLayer.TargetWeight > 0)
            {
                PlayActionFullBody(_ActionFadeDuration);
            }
            else
            {
                _BaseLayer.Play(transition);
            }
        }

        /************************************************************************************************************************/

        public void PlayAction(ITransition transition)
        {
            _ActionLayer.Play(transition);

            if (_CanPlayActionFullBody)
                PlayActionFullBody(transition.FadeDuration);
        }

        /************************************************************************************************************************/

        private void PlayActionFullBody(float fadeDuration)
        {
            AnimancerState actionState = _ActionLayer.CurrentState;
            AnimancerState baseState = _BaseLayer.Play(actionState, fadeDuration);
            baseState.Time = actionState.Time;
        }

        /************************************************************************************************************************/

        public void FadeOutAction()
        {
            _ActionLayer.StartFade(0, _ActionFadeDuration);
        }

        /************************************************************************************************************************/
    }
}
