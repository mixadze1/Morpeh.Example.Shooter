// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.Layers
{
    /// <summary>
    /// Demonstrates how to use layers to play multiple
    /// independent animations at the same time on different body parts.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/layers/basic">
    /// Basic Layers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Layers/LayeredCharacterAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Layers - Layered Character Animations")]
    [AnimancerHelpUrl(typeof(LayeredCharacterAnimations))]
    public class LayeredCharacterAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private ClipTransition _Idle;
        [SerializeField] private ClipTransition _Move;
        [SerializeField] private ClipTransition _Action;
        [SerializeField] private AvatarMask _ActionMask;
        [SerializeField, Seconds] private float _ActionFadeOutDuration = AnimancerGraph.DefaultFadeDuration;

        /************************************************************************************************************************/

        private AnimancerLayer _BaseLayer;
        private AnimancerLayer _ActionLayer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _BaseLayer = _Animancer.Layers[0];
            _ActionLayer = _Animancer.Layers[1];// First access to a layer creates it.

            _ActionLayer.Mask = _ActionMask;
            _ActionLayer.SetDebugName("Action Layer");

            _Action.Events.OnEnd = OnActionEnd;
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            UpdateMovement();
            UpdateAction();
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            float forward = SampleInput.WASD.y;
            if (forward > 0)
            {
                _BaseLayer.Play(_Move);
            }
            else
            {
                _BaseLayer.Play(_Idle);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
            {
                _ActionLayer.Play(_Action);
            }
        }

        /************************************************************************************************************************/

        private void OnActionEnd()
        {
            _ActionLayer.StartFade(0, _ActionFadeOutDuration);
        }

        /************************************************************************************************************************/
    }
}
